using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    //Shapeshifting attributes
    [Header("Shapeshift Settings")]
    private GameObject activePlayer;
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
    public float minzoom = 5f;
    public float maxzoom = 8f;
    public float zoomSpeed = 20f;


    public bool movementEnabled;
    private bool isWalking;
    private bool wasWalking; //Were we walking last frame?
    public bool isGrounded;

    /* Index values in the character array for the animals */
    private int ind_TURTLE = 0;
    private int ind_HORSE = 1;
    private int ind_RACCOON = 2;
    private int ind_CROW = 3;

    public GameObject CharacterWheel;

    public DialogueController wizard;

    public PuzzleController current_puzzle;

    // Start is called before the first frame update
    void Start()
    { 
        movementEnabled = false;
        //Camera obj
        cameraTrans = Camera.main.transform;
        cam = CinemachineCamera.GetComponent<CinemachineFreeLook>();

        prev_x_speed = cam.m_XAxis.m_MaxSpeed;
        prev_y_speed = cam.m_YAxis.m_MaxSpeed;

        // Start as tortoise
        characters[0].SetActive(true);
        activePlayer = characters[0];
        ShapeShiftUpdate();
        rigidBody = GetComponent<Rigidbody>();

        for (int i = 1; i < 4; i++)
        {
            characters.Add(null);
        }
    }

    void Update()
    {
        m_Rotation = Quaternion.identity;

        //print(currentZoom);
        float newDist = Mathf.Lerp(cam.m_Orbits[0].m_Radius, currentZoom, zoomSpeed);

        cam.m_Orbits[0].m_Radius = newDist;
        cam.m_Orbits[1].m_Radius = newDist;
        cam.m_Orbits[2].m_Radius = newDist;
    }

    private void FixedUpdate()
    {
        if (movementEnabled)
        {
            cam.m_XAxis.m_MaxSpeed = prev_x_speed;
            cam.m_YAxis.m_MaxSpeed = prev_y_speed;

            playerMoveInput.Normalize();
            isWalking = !Mathf.Approximately(playerMoveInput.x, 0f) || !Mathf.Approximately(playerMoveInput.z, 0f);
            activeAnims.SetBool("IsWalking", isWalking);

            //TODO: Figure out slope underneath player and find out if we want them to slide or not. Maybe climbable slope is based on character?
            //guide here for a good way to implement it: http://thehiddensignal.com/unity-angle-of-sloped-ground-under-player/

            Vector3 moveMagnitude = cameraTrans.forward * playerMoveInput.z + cameraTrans.right * playerMoveInput.x;  //Get the player's movement, relative to the camera.
            moveMagnitude.y = 0f;

            //Instead 
            Vector3 facing = transform.forward;
            facing.y = 0f;


            Vector3 desiredForward = Vector3.RotateTowards(facing, moveMagnitude, turnSpeed * Time.deltaTime, 0f);
            //print(desiredForward);
            m_Rotation = Quaternion.LookRotation(desiredForward);
            //Now, actually move!
            rigidBody.MovePosition(rigidBody.position + moveMagnitude * baseSpeed);
            rigidBody.MoveRotation(m_Rotation);
        }
        else
        {
            //prev_x_speed = cam.m_XAxis.m_MaxSpeed;
            //prev_y_speed = cam.m_YAxis.m_MaxSpeed;
            cam.m_XAxis.m_MaxSpeed = 0.0f;
            cam.m_YAxis.m_MaxSpeed = 0.0f;
        }
    }

    public void ShapeShiftTo(int animal_index)
    {
        activePlayer.SetActive(false);
        activePlayer = characters[animal_index];
        activePlayer.SetActive(true);
        ShapeShiftUpdate();
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

        
        for (int i = 0; i < 3; i++)
        {
            print("asdasd");
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
        print("Horse");
        if (characters[ind_HORSE] != null && CharacterWheel.activeSelf)
        {
            ShapeShiftTo(ind_HORSE);
        }
    }

    void OnShapeShiftTurtle()
    {
        print("Turtle");
        if (characters[ind_TURTLE] != null && CharacterWheel.activeSelf)
        {
            ShapeShiftTo(ind_TURTLE);
        }
    }

    void OnShapeShiftRaccoon()
    {
        print("Raccoon");
        if (characters[ind_RACCOON] != null && CharacterWheel.activeSelf)
        {
            ShapeShiftTo(ind_RACCOON);
        }
    }
    void OnShapeShiftCrow()
    {
        print("CROW");
        if (characters[ind_CROW] != null && CharacterWheel.activeSelf)
        {
            ShapeShiftTo(ind_CROW);
        }
    }


    private void OnMove(InputValue input)
    {
        Vector2 vec = input.Get<Vector2>();

        playerMoveInput.x = vec.x;
        playerMoveInput.z = vec.y; //Y is height in 3d, but we want our y to handle movement on the Z plane.
    }

    private void OnJump()
    {
        print("Thing done!");
        activeScript.OnJump(false);
    }

    private void OnClick()
    {
        wizard.Notify();
    }

    private void OnZoom(InputValue input)
    {
        return;
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

            charnames.Add(other.name);

            // Disable the trigger
            characters[activeIndex].GetComponent<Collider>().isTrigger = false;
            characters[activeIndex].SetActive(false); //disable animal
            characters[activeIndex].transform.parent = transform;
            characters[activeIndex].transform.localRotation = Quaternion.identity;
            characters[activeIndex].transform.localPosition = Vector3.zero;
        }
    }

    void OnReset(InputValue input) {
        if (current_puzzle != null) {
            print("RESET!");
            current_puzzle.ResetPuzzle();
        
        }
    
    }

}