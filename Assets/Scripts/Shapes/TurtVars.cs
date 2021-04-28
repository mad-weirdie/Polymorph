using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtVars : ShapeVariables
{
    //Tortoise-specific details.

    public float pushStrength = 2f; //How high above the ground the crow will fly at.
    private Vector3 height;
    //bool canPush;

    override public void Start()
    {
        base.Start();
        height = new Vector3(0f, pushStrength, 0f);
       // canPush = true;
    }

    // Update is called once per frame
    override public void OnJump(bool isHeld)
    {
        //Actions that happen on the space bar being pressed, like jumping or flying.
 
    }

    
}
