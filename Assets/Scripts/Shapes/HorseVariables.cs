using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseVariables : ShapeVariables
{
    // When necessary, uncomment this variables.
    // Start is called before the first frame update

    public float jumpForce = 7500f;
    protected Vector3 height;
    override public void Start()
    {
        base.Start();
        height = new Vector3(0f, jumpForce, 0f);
    }

    //Update is called once per frame
    override public void OnJump(bool isHeld)
    {
        //Actions that happen on the space bar being pressed, like jumping or flying.
        if (Physics.Raycast(transform.position, Vector3.down, 1))
        {
            print("jump!");
            script.rigidBody.AddForce(height);
        }

    }
}
