using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.SceneManagement; // For level change cheats.

public class PlayerController : MonoBehaviour
{
    //Shapeshifting attributes
    [Header("Shapeshift Settings")]
    public GameObject activePlayer;
    private Animator activeAnims;
    public ShapeVariables activeScript; //ref to shape's variables. Used to call per-shape functions.
    public List<GameObject> characters;
    public List<string> charnames; //used to quickly check whether we have a shape or not
    public List<Listener> Listeners = new List<Listener>();

    [Header("Movement code")]
    //Movement code attributes
    public float baseSpeed = 10f; //TODO: Character based speeds
    public float turnSpeed = 10f;

    private Vector3 playerMoveInput;
    Quaternion m_Rotation;
    public Rigidbody rigidBody; //public, gotten in code. Could be done via pass by ref instead?

    [Header("Camera code")]
    public GameObject CinemachineCamera;
    public CinemachineFreeLook cam;
    private Transform cameraTrans;
    public float prev_x_speed;
    public float prev_y_speed;
    private float currentZoom;
    public float minzoom = 1f;
    public float maxzoom = 8f;
    public float zoomSpeed = .5f;

    public bool movementEnabled;
    public bool isWalking;
    public bool isGrounded;

    public GameObject CharacterWheel;
    public GameObject SettingsMenu;
    public AudioSource ShapeShiftSound;

    public DialogueController wizard;

    private bool isGrabbing = false;
    private bool isClimbing = false;
    private Rigidbody movingRigidBodyObject;
    private float normalMass;
    public PuzzleController current_puzzle;

    private Vector3 moveMagnitude;
    private Vector3 facing;
    private Vector3 desiredForward;
    private Vector2 vec;
    private Vector3 grabbedObj;

    public Vector3 lastCheckpointPos;
    public Quaternion lastCheckpointDir;
    private AudioSource walkingAudio;
    public ParticleSystem magicEffect;

    private HelpfulText text;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PersistentData.spawnPoint != null)
        {
            PersistentData.Start(); // Make sure our list is initialized.
            // The spawnPoint will be (0,0,0) if we don't want to do anything yet
            if (!(PersistentData.spawnPoint.x == 0f && PersistentData.spawnPoint.y == 0f &&
                PersistentData.spawnPoint.z == 0f))
            {
                // Otherwise, set it to the saved new position from the exit
                // trigger game object 
                activePlayer.transform.position = PersistentData.spawnPoint;
                activePlayer.transform.rotation = PersistentData.spawnRotation;
            }
            print("Crystal statuses!: ");
            for (int i = 0; i < PersistentData.CrystalsCollected.Count; i++) {
                print(PersistentData.CrystalsCollected[i]);
            }
        }

        //Camera obj
        cameraTrans = Camera.main.transform;
        cam = CinemachineCamera.GetComponent<CinemachineFreeLook>();

        lastCheckpointPos = transform.position;
        lastCheckpointDir = transform.rotation;

        currentZoom = minzoom;
        StartCoroutine(Hold());
        
        movementEnabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        walkingAudio = GetComponent<AudioSource>();

        

        prev_x_speed = cam.m_XAxis.m_MaxSpeed;
        prev_y_speed = cam.m_YAxis.m_MaxSpeed;

        // Start as tortoise
        characters[0].SetActive(true);
        activePlayer = characters[0];
        
        rigidBody = GetComponent<Rigidbody>();
        ShapeShiftUpdate();

        // Add usable characters into the list and add the character selection frame.
        for (int i = 0; i < transform.childCount; i++)
        {
            Texture2D animalImage;
            GameObject child = transform.GetChild(i).gameObject;

            if (child.CompareTag("Animal") || (child.CompareTag("Player") && child.name != "Tortoise"))
            {
                characters.Add(child);

                // Load the resource image into the character selection frame.
                animalImage = Resources.Load("Images/" + child.name) as Texture2D;
                CharacterWheel.transform.GetChild(characters.Count - 1).gameObject.SetActive(true);
                GameObject image = CharacterWheel.transform.GetChild(characters.Count - 1).GetChild(0).gameObject;
                image.GetComponent<RawImage>().texture = animalImage;
            }
        }

        normalMass = rigidBody.mass;
        movingRigidBodyObject = null;
    }

    IEnumerator Hold()
    {
        yield return new WaitForSecondsRealtime(5);
        wizard.Notify();
    }

    // Allow other scripts to pause the game
    public void PauseGame()
    {
        movementEnabled = false;
        activeAnims.SetBool("IsWalking", false);
        walkingAudio.Stop();
    }

    public void UnpauseGame()
    {
        movementEnabled = true;
    }

    public void AddListener(Listener newListener)
    {
        Listeners.Add(newListener);
    }

    void Update()
    {
        m_Rotation = Quaternion.identity;

        //print(currentZoom);
        if (!Mathf.Approximately(cam.m_Orbits[0].m_Radius - currentZoom, 0f))
        {
            float newDist = Mathf.Lerp(cam.m_Orbits[0].m_Radius, currentZoom, .1f);

            cam.m_Orbits[0].m_Radius = newDist;
            cam.m_Orbits[1].m_Radius = newDist;
            cam.m_Orbits[2].m_Radius = newDist;
        }
    }

    private void FixedUpdate()
    {
        if (movementEnabled)
        {
            // Reset the camera speed if we had paused it
            if (cam.m_XAxis.m_MaxSpeed == 0.0f && cam.m_YAxis.m_MaxSpeed == 0.0f)
            {
                cam.m_XAxis.m_MaxSpeed = prev_x_speed;
                cam.m_YAxis.m_MaxSpeed = prev_y_speed;
            }

            // Play walking audio if necessary
            if (!isWalking)
            {
                walkingAudio.Stop();
            }
            else if (isWalking && !walkingAudio.isPlaying)
            {
                walkingAudio.Play();
            }

            // Calculate player movement
            playerMoveInput.Normalize();
            isWalking = !Mathf.Approximately(playerMoveInput.x, 0f) || !Mathf.Approximately(playerMoveInput.z, 0f);
            activeAnims.SetBool("IsWalking", isWalking);
            
            //Instead 
            facing = transform.forward;
            facing.y = 0f;

            if (!isClimbing)
            {
                // Get the player's movement, relative to the camera.
                moveMagnitude = cameraTrans.forward * playerMoveInput.z + cameraTrans.right * playerMoveInput.x;
                moveMagnitude.y = 0f;

                desiredForward = Vector3.RotateTowards(facing, moveMagnitude, turnSpeed * Time.deltaTime, 0f);
                m_Rotation = Quaternion.LookRotation(desiredForward);
            }
            else
            {
                // Get the player's movement, relative to the camera.
                moveMagnitude = cameraTrans.up * playerMoveInput.z; // z is the forward movement
                moveMagnitude.z = 0f;
                moveMagnitude.x = 0f;
                cameraTrans.position = rigidBody.position;
                desiredForward = Vector3.RotateTowards(facing, moveMagnitude, 0f, 0f);
                m_Rotation = Quaternion.LookRotation(desiredForward);
            }

            //Now, actually move!
            rigidBody.MovePosition(rigidBody.position + moveMagnitude * baseSpeed);

            rigidBody.MoveRotation(m_Rotation);

            if (isGrabbing && movingRigidBodyObject != null)
            {
                if (activePlayer.name == "Horse(Clone)")
                {
                    movingRigidBodyObject.useGravity = false;
                    grabbedObj = transform.position;
                    grabbedObj.y += 1.0f;
                    grabbedObj += desiredForward * 1.2f;
                    movingRigidBodyObject.MovePosition(grabbedObj);
                    movingRigidBodyObject.MoveRotation(m_Rotation);
                }
                else if (activePlayer.name == "Crow(Clone)")
                {
                    movingRigidBodyObject.useGravity = false;
                    movingRigidBodyObject.mass = 0.1f;
                    grabbedObj = transform.position;
                    grabbedObj.y += 0.5f;
                    grabbedObj += desiredForward * 0.5f;
                    movingRigidBodyObject.MovePosition(grabbedObj);
                    movingRigidBodyObject.MoveRotation(m_Rotation);
                }
                else
                {
                    
                    movingRigidBodyObject.useGravity = true;
                    movingRigidBodyObject.MovePosition(movingRigidBodyObject.position + moveMagnitude * baseSpeed);
                }

            }
        }
        else
        {
            cam.m_XAxis.m_MaxSpeed = 0.0f;
            cam.m_YAxis.m_MaxSpeed = 0.0f;
        }
    }

    void OnCollisionEnter()
    {
        isGrounded = true;
    }

    public void ShapeShiftTo(int animal_index)
    {
        if (characters[animal_index] != activePlayer)
        {
            ShapeShiftSound.Play();
            activePlayer.SetActive(false);
            activePlayer = characters[animal_index];
            activePlayer.SetActive(true);
            ShapeShiftUpdate();
        }
    }

    void ShapeShiftUpdate()
    {
        //Update variables that rely on activePlayer.
        activeAnims = activePlayer.GetComponent<Animator>();
        
        activeAnims.SetBool("IsWalking", isWalking);
        ShapeVariables activeAbilityScript = activePlayer.GetComponent<ShapeVariables>();
        activeScript = activeAbilityScript;

        //get our movement values from the shape
        baseSpeed = activeAbilityScript.animalSpeed;
        turnSpeed = activeAbilityScript.turnSpeed;
        rigidBody.mass = activeAbilityScript.animalMass;

        
        for (int i = 0; i < 3; i++)
        {
            CinemachineComposer comp = cam.GetRig(i).GetCinemachineComponent<CinemachineComposer>();
            comp.m_TrackedObjectOffset.x = activeAbilityScript.shapeOffsets.x;
            comp.m_TrackedObjectOffset.y = activeAbilityScript.shapeOffsets.y;
            comp.m_TrackedObjectOffset.z = activeAbilityScript.shapeOffsets.z;
        }
        
    }

    void OnShapeShift()
    {
        // Open the character wheel
        CharacterWheel.SetActive(!CharacterWheel.activeSelf);
    }

    void OnShapeShift1()
    {
        if (characters[0] != null)
        {
            ShapeShiftTo(0);
        }
    }

    void OnShapeShift2()
    {
        if (characters[1] != null)
        {
            ShapeShiftTo(1);
        }
    }

    void OnShapeShift3()
    {
        if (characters[2] != null)
        {
            ShapeShiftTo(2);
        }
    }


    void OnShapeShift4()
    {
        if (characters[3] != null)
        {
            ShapeShiftTo(3);
        }
    }

    void OnShapeShift5()
    {
        if (characters[4] != null)
        {
            ShapeShiftTo(4);
        }
    }

    void OnShapeShift6()
    {
        if (characters[5] != null)
        {
            ShapeShiftTo(5);
        }
    }

    void OnShapeShift7()
    {
        if (characters[6] != null)
        {
            ShapeShiftTo(6);
        }
    }

    void OnShapeShift8()
    {
        if (characters[7] != null)
        {
            ShapeShiftTo(7);
        }
    }


    private void OnGrab()
    {
        if (isGrabbing && movingRigidBodyObject != null)
        {
            isGrabbing = false;
            movingRigidBodyObject.useGravity = true;
            movingRigidBodyObject.mass = 5.0f;
            print("release");
        }
        else
            isGrabbing = true;

        foreach (Listener scriptObject in Listeners)
        {
            if (scriptObject.listenerType == "Grab")
            {
                scriptObject.Notify();
            }
                
        }
    }

    private void OnMove(InputValue input)
    {
        vec = input.Get<Vector2>();

        playerMoveInput.x = vec.x;
        playerMoveInput.z = vec.y; //Y is height in 3d, but we want our y to handle movement on the Z plane.
    }

    private void OnJump(InputValue input)
    {
        if (activeScript.animalName == "Raccoon")
            isClimbing = !isClimbing;

        activeScript.OnJump(input.Get<float>() > 0f);
    }

    private void OnClick()
    {
        foreach (Listener scriptObject in Listeners)
        {
            if (scriptObject.listenerType == "Click")
                scriptObject.Notify();
        }
        wizard.Notify();
    }

    private void OnZoom(InputValue input)
    {
        Vector2 vec = input.Get<Vector2>();
        int sign = 1;
        if (vec.y == 0)
        {
            return; //0 signals the wheel isn't moving anymore.
        }
        if (vec.y < 0f)
        {
            sign = -1;
        }
        currentZoom = Mathf.Clamp(currentZoom + (zoomSpeed * sign), minzoom, maxzoom);
    }
    
    private IEnumerator waitThenMagic(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        magicEffect.Play();
    }

    private void OnTriggerStay(Collider other)
    {
        // Rigid body reference should only be changed if not set to anything.
        if (movingRigidBodyObject == null)
        {

            if (other.gameObject.CompareTag("Movable") && isGrabbing)
            {
                movingRigidBodyObject = other.GetComponent<Rigidbody>();

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Rigid body reference should only be changed if not set to anything.
        if (movingRigidBodyObject == null) 
        {
            if (other.gameObject.CompareTag("Movable") && isGrabbing)
            {
                movingRigidBodyObject = other.GetComponent<Rigidbody>();

            }
        }

        if (other.gameObject.CompareTag("Animal") && !charnames.Contains(other.name))
        {
            int activeIndex;
            Texture2D animalImage;
            other.gameObject.tag = "Player";

            // Load the new character
            characters.Add(GameObject.Instantiate(other.gameObject, this.transform, false));
            CharacterWheel.transform.GetChild(characters.Count - 1).gameObject.SetActive(true);
            activeIndex = characters.Count - 1;

            // Particle effects for new character as well as the player themselves
            // Particle effects for the other animals are destroyed after being played
            other.gameObject.GetComponent<ShapeVariables>().PlayMagicEffect();
            StartCoroutine(waitThenMagic(1.0f));

            // Load the resource image into the character selection frame.
            animalImage = Resources.Load("Images/" + other.name) as Texture2D;
            GameObject image = CharacterWheel.transform.GetChild(activeIndex).GetChild(0).gameObject;
            image.GetComponent<RawImage>().texture = animalImage;

            charnames.Add(other.name);

            // Disable the trigger
            characters[activeIndex].GetComponent<Collider>().isTrigger = false;
            characters[activeIndex].SetActive(false); //disable animal
            characters[activeIndex].transform.parent = transform;
            characters[activeIndex].transform.localRotation = Quaternion.identity;
            characters[activeIndex].transform.localPosition = Vector3.zero;
        }
        else if (other.gameObject.CompareTag("Dialogue"))
        {
            text = other.gameObject.GetComponent<HelpfulText>();
            if (text.conditionsMet())
            {
                text.Show();
            }  
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Gotta decide exactly how we want to do the grab "break" mechanic?
        // Definitely want more 'leniency' than previous
        if (movingRigidBodyObject == null) { }
        else if (other.gameObject == movingRigidBodyObject.gameObject)
        {
            movingRigidBodyObject.useGravity = true;
            movingRigidBodyObject = null;
            isGrabbing = false;
        }

        if (other.gameObject.CompareTag("Dialogue"))
        {
            text = other.gameObject.GetComponent<HelpfulText>();
        }
    }

    void OnReset(InputValue input) {
        if (current_puzzle != null) {
            print("RESET!");
            current_puzzle.ResetPuzzle();  
        }
    }

    void OnEscape() {
        //print("onEscape called!");
        //print("Cursor state:");
        //print(Cursor.visible);
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            SettingsMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else {
            
            SettingsMenu.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void SetCheckpoint() {
        lastCheckpointPos = transform.position;
        lastCheckpointDir = transform.rotation;
    
    }


    //Various cheats and debug points live here.  
    void OnMineCheat() {

        //Cheat to immediately go to the mines. Keybind: f11
        SceneManager.LoadScene("Mines", LoadSceneMode.Single);

    }

    void OnTowerCheat() {
        //Cheat to immediately go to the tower. keybind: f12
        SceneManager.LoadScene("Tower", LoadSceneMode.Single);
    }

    void OnResetToCheckpoint() {
        //Teleport back to the last checkpoint! Keybind: f1
        transform.position = lastCheckpointPos;
        transform.rotation = lastCheckpointDir;
    
    }

}