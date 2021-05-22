using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseVariables : ShapeVariables
{
    public float jumpForce;
    
    [SerializeField]
    protected Vector3 jumpVector;
    private Animator horseAnimator;
    //private float gravity = 1f;

    override public void Start()
    {
        animalName = "Horse";
        base.Start();
        jumpVector = new Vector3(0f, jumpForce, 0f);
        horseAnimator = GetComponent<Animator>();

    }

    override public void OnJump(bool isHeld)
    {
        // Actions that happen on the space bar being pressed, like jumping or flying.
        // This is for debugging
        // Debug.DrawRay(transform.position, Vector3.down * .5f, Color.green, 5f);

        // Check if horse is on the ground
        if (Physics.Raycast(transform.position, Vector3.down, 0.5f))
        {
            // print("jump!");
            script.rigidBody.AddForce(jumpVector, ForceMode.Impulse);
            horseAnimator.Play("Jump");
        }
        
    }
    /*
    void Update()
    {
        if (!Physics.Raycast(transform.position, Vector3.down, 0.5f))
        {
            //script.rigidBody.AddForce(-Vector3.up * gravity, ForceMode.Impulse);
        }
    }*/
}
