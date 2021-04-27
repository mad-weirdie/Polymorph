using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowVars : ShapeVariables
{
    //Crow-specific details.

    public float flyingHeight = 2f; //How high above the ground the crow will fly at.
    private Vector3 height;
    override public void Start()
    {
        base.Start();
        height = new Vector3(0f, flyingHeight, 0f);
    }

    // Update is called once per frame
    override public void OnJump(bool isHeld)
    {
        //Actions that happen on the space bar being pressed, like jumping or flying.
        print("jump!");
        script.rigidBody.AddForce(height);

    }
}
