using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    //Shapeshifting attributes
    private GameObject activePlayer;
    private Animator activeAnims;
    public List<GameObject> characters;
    
    
    //Movement code attributes
    public float baseSpeed = 10f; //TODO: Character based speeds
    public float turnSpeed = 10f;

    private Vector3 playerMoveInput;
    Quaternion m_Rotation;
    private Rigidbody rigidBody;

    private Transform cameraTrans;

    // Start is called before the first frame update
    void Start()
    {
        //Camera obj
        cameraTrans = Camera.main.transform;

        // Set all other game objects to not active.
        foreach (GameObject character in characters)
        {
            character.SetActive(false);
        }

        // Start as tortoise
        characters[0].SetActive(true);
        activePlayer = characters[0];
        ShapeShiftUpdate();
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null) { print("AH BEANS"); }

    }

    // Update is called once per frame
    void Update()
    {
        m_Rotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        playerMoveInput.Normalize();
        bool isWalking;
        isWalking = !Mathf.Approximately(playerMoveInput.x, 0f) || !Mathf.Approximately(playerMoveInput.z, 0f);
        activeAnims.SetBool("IsWalking", isWalking);

        Vector3 moveDirection = cameraTrans.forward * playerMoveInput.z + cameraTrans.right * playerMoveInput.x;
        moveDirection.y = 0f;

        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, moveDirection, turnSpeed * Time.deltaTime, 0f);
        //print(desiredForward);
        m_Rotation = Quaternion.LookRotation(desiredForward);
        //print(m_Rotation);
        //Now, actually move!
        

        rigidBody.MovePosition(rigidBody.position + moveDirection* baseSpeed);
        rigidBody.MoveRotation(m_Rotation);

    }

    void OnAnimatorMove()
    {

    }


    void OnShapeShift()
    {
        // Swap between characters.
        Transform pos = activePlayer.transform;
        activePlayer.SetActive(false);

        if (activePlayer.name == characters[0].name)
        {
            activePlayer = characters[1];
        }
        else
        {
            activePlayer = characters[0];
        }

        activePlayer.SetActive(true);
        ShapeShiftUpdate();
    }

    void ShapeShiftUpdate() {

        //Update variables that rely on activePlayer.
        activeAnims = activePlayer.GetComponent<Animator>();
        ShapeVariables activeAbilityScript = activePlayer.GetComponent<ShapeVariables>();
        //get our movement values from the shape
        baseSpeed = activeAbilityScript.animalSpeed;
        turnSpeed = activeAbilityScript.turnSpeed;
    }


    private void OnMove(InputValue input)
    {
        Vector2 vec = input.Get<Vector2>();

        playerMoveInput.x = vec.x;
        playerMoveInput.z = vec.y; //Y is height in 3d, but we want our y to handle movement on the Z plane.
    }

    private void OnJump() {
    }
}
