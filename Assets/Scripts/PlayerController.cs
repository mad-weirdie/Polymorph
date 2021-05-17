using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    //Shapeshifting attributes
    [Header("Shapeshift Settings")]
    public GameObject activePlayer;
    private Animator activeAnims;
    public ShapeVariables activeScript; //ref to shape's variables. Used to call per-shape functions.
    public List<GameObject> characters;
    public List<string> charnames; //used to quickly check whether we have a shape or not

    [Header("Movement code")]
    //Movement code attributes
    public float baseSpeed = 10f; //TODO: Character based speeds
    public float turnSpeed = 10f;

    private Vector3 playerMoveInput;
    Quaternion m_Rotation;
    public Rigidbody rigidBody; //public, gotten in code. Could be done via pass by ref instead?

    [Header("Camera code")]
    public GameObject CinemachineCamera;
    private CinemachineFreeLook cam;
    private Transform cameraTrans;
    public float prev_x_speed;
    public float prev_y_speed;
    private float currentZoom = 5f;
    public float minzoom = 1f;
    public float maxzoom = 8f;
    public float zoomSpeed = .5f;

    private bool movementPaused;

    public bool movementEnabled;
    public bool isWalking;
    private bool wasWalking; //Were we walking last frame?
    public bool isGrounded;

    /* Index values in the character array for the animals */
    private int ind_TURTLE = 0;
    private int ind_HORSE = 1;
    private int ind_RACCOON = 2;
    private int ind_CROW = 3;

    public GameObject CharacterWheel;
    public GameObject SettingsMenu;
    public AudioSource ShapeShiftSound;

    public DialogueController wizard;

    private bool isGrabbing = false;
    private Rigidbody movingRigidBodyObject;
    private float normalMass;
    public PuzzleController current_puzzle;

    private Vector3 moveMagnitude;
    private Vector3 facing;
    private Vector3 desiredForward;
    private Vector2 vec;
    private Vector3 grabbedObj;

    private AudioSource walkingAudio;

    private HighlightGameObject highlightObject;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Hold());
        
        movementEnabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        walkingAudio = GetComponent<AudioSource>();
        //Camera obj
        cameraTrans = Camera.main.transform;
        cam = CinemachineCamera.GetComponent<CinemachineFreeLook>();

        prev_x_speed = cam.m_XAxis.m_MaxSpeed;
        prev_y_speed = cam.m_YAxis.m_MaxSpeed;

        // Start as tortoise
        characters[0].SetActive(true);
        activePlayer = characters[0];
        
        rigidBody = GetComponent<Rigidbody>();
        ShapeShiftUpdate();

        for (int i = 1; i < 4; i++)
        {
            characters.Add(null);
        }

        normalMass = rigidBody.mass;
        movingRigidBodyObject = null;

        highlightObject = new HighlightGameObject();
    }

    IEnumerator Hold()
    {
        yield return new WaitForSecondsRealtime(5);
        wizard.Notify();
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
            // Get the player's movement, relative to the camera.
            moveMagnitude = cameraTrans.forward * playerMoveInput.z + cameraTrans.right * playerMoveInput.x;
            moveMagnitude.y = 0f;

            //Instead 
            facing = transform.forward;
            facing.y = 0f;


            desiredForward = Vector3.RotateTowards(facing, moveMagnitude, turnSpeed * Time.deltaTime, 0f);
            //print(desiredForward);
            m_Rotation = Quaternion.LookRotation(desiredForward);
            //Now, actually move!
            rigidBody.MovePosition(rigidBody.position + moveMagnitude * baseSpeed);
            rigidBody.MoveRotation(m_Rotation);

            if (isGrabbing && movingRigidBodyObject != null)
            {
                if (activePlayer.name != "Horse(Clone)")
                {
                    movingRigidBodyObject.useGravity = true;
                    movingRigidBodyObject.MovePosition(movingRigidBodyObject.position + moveMagnitude * baseSpeed);
                }
                else
                {
                    movingRigidBodyObject.useGravity = false;
                    grabbedObj = transform.position;
                    grabbedObj.y += 1.0f;
                    grabbedObj += desiredForward * 1.2f;
                    movingRigidBodyObject.MovePosition(grabbedObj);
                    movingRigidBodyObject.MoveRotation(m_Rotation);
                }

                //highlightObject.ChangeColor();
                
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
            rigidBody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
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

    void OnShapeShiftHorse()
    {
        if (characters[ind_HORSE] != null && CharacterWheel.activeSelf)
        {
            ShapeShiftTo(ind_HORSE);
        }
    }

    void OnShapeShiftTurtle()
    {
        if (characters[ind_TURTLE] != null && CharacterWheel.activeSelf)
        {
            ShapeShiftTo(ind_TURTLE);
        }
    }

    void OnShapeShiftRaccoon()
    {
        if (characters[ind_RACCOON] != null && CharacterWheel.activeSelf)
        {
            ShapeShiftTo(ind_RACCOON);
        }
    }
    void OnShapeShiftCrow()
    {
        if (characters[ind_CROW] != null && CharacterWheel.activeSelf)
        {
            ShapeShiftTo(ind_CROW);
        }
    }

    private void OnGrab()
    {
        isGrabbing = !isGrabbing;
        if (!isGrabbing && movingRigidBodyObject != null)
        {
            highlightObject.RemoveGameObject();
            movingRigidBodyObject.useGravity = true;
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
        
        print("Thing done!");
        activeScript.OnJump(true);
        //isGrounded = false;
    }

    private void OnClick()
    {
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
        print((zoomSpeed * sign));
        currentZoom = Mathf.Clamp(currentZoom + (zoomSpeed * sign), minzoom, maxzoom);


        print(currentZoom);
    }
    

    private void OnTriggerEnter(Collider other)
    {
        // Rigid body reference should only be changed if not set to anything.
        if (movingRigidBodyObject == null) 
        {
            if (other.gameObject.CompareTag("Movable") && isGrabbing)
            {
                // This breaks the seesaw mechanic bc you weigh nothing
                //rigidBody.mass = 0;
                movingRigidBodyObject = other.GetComponent<Rigidbody>();

                // Set the object to highlight.
                highlightObject.SetGameObject(other.gameObject);
                highlightObject.ChangeColor();
            }
        }

        if (other.gameObject.CompareTag("Animal") && !charnames.Contains(other.name))
        {
            int activeIndex;

            if (other.name == "Horse")
            {
                CharacterWheel.transform.GetChild(ind_HORSE).gameObject.SetActive(true);
                characters[ind_HORSE] = GameObject.Instantiate(other.gameObject, this.transform, false);
                activeIndex = ind_HORSE;
            }
            else if (other.name == "Raccoon")
            {
                CharacterWheel.transform.GetChild(ind_RACCOON).gameObject.SetActive(true);
                characters[ind_RACCOON] = GameObject.Instantiate(other.gameObject, this.transform, false);
                activeIndex = ind_RACCOON;
            }
            else if (other.name == "Crow")
            {
                CharacterWheel.transform.GetChild(ind_CROW).gameObject.SetActive(true);
                characters[ind_CROW] = GameObject.Instantiate(other.gameObject, this.transform, false);
                activeIndex = ind_CROW;
            }
            else
                return;

            //other.gameObject.SetActive(false); //Remove animal for consistency + stop clipping issues!
            charnames.Add(other.name);

            // Disable the trigger
            characters[activeIndex].GetComponent<Collider>().isTrigger = false;
            characters[activeIndex].SetActive(false); //disable animal
            characters[activeIndex].transform.parent = transform;
            characters[activeIndex].transform.localRotation = Quaternion.identity;
            characters[activeIndex].transform.localPosition = Vector3.zero;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        // Rigid body reference should only be changed if not set to anything.
        if (movingRigidBodyObject == null)
        {
            if (other.gameObject.CompareTag("Movable") && isGrabbing)
            {
                // This breaks the seesaw mechanic bc you weigh nothing
                //rigidBody.mass = 0;
                movingRigidBodyObject = other.GetComponent<Rigidbody>();

                // Set the object to highlight.
                highlightObject.SetGameObject(other.gameObject);
                highlightObject.ChangeColor();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Gotta decide exactly how we want to do the grab "break" mechanic?
        // Definitely want more 'leniency' than previous
        //isGrabbing = false;
        //rigidBody.mass = normalMass;
        if (movingRigidBodyObject == null) { }
        else if (other.gameObject == movingRigidBodyObject.gameObject)
        {
            movingRigidBodyObject = null;
            isGrabbing = false;

            // Remove the object to highlight.
            highlightObject.RemoveGameObject();
        }
    }

    void OnReset(InputValue input) {
        if (current_puzzle != null) {
            print("RESET!");
            current_puzzle.ResetPuzzle();
        
        }
    
    }

    void OnEscape() {
        print("onEscape called!");
        print("Cursor state:");
        print(Cursor.visible);
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



}