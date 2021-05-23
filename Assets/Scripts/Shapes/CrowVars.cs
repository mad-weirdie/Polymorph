using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowVars : ShapeVariables
{
    //Crow-specific details.

    public float flyingHeight = 2f; //force crow jumps with.
    public float glidingForce = 2000f; //force pushing crow up while gliding to counteract gravity.
    [SerializeField]
    private Vector3 height;
    [SerializeField]
    private Vector3 glideForce;
    [SerializeField]
    private bool jumpHeld;
    private float last_y;
    private float falling_time;
    private Animator crowAnimator;

    override public void Start()
    {
        animalName = "Crow";
        base.Start();
        height = new Vector3(0f, flyingHeight, 0f);
        glideForce = new Vector3(0f, glidingForce, 0f);
        crowAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //lord forgive me i'm back on my bs
        if (jumpHeld) {
            script.rigidBody.AddForce(glideForce);
        
        }

    }

    // Update is called once per frame
    override public void OnJump(bool isHeld)
    {
        //Actions that happen on the space bar being pressed, like jumping or flying.
        //print("jump!");
        //print(isHeld);
        jumpHeld = isHeld;
        if (isHeld)
        {
            crowAnimator.Play("Jump");
            crowAnimator.SetBool("IsFlying", true);
        }
        else {
            crowAnimator.SetBool("IsFlying", false);
        }

        Debug.DrawRay(transform.position, Vector3.down * .5f, Color.green, 5f);
        if (Physics.Raycast(transform.position, Vector3.down, .5f))
        {
            script.rigidBody.AddForce(height); //only jump on key down!
        }

    }
}
