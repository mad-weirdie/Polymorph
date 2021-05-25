using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacconVars : ShapeVariables
{
    //Raccoonn-specific details.

    private RaycastHit hit;
    private Quaternion normalRotation;
    private Rigidbody playerRigidBody;
    private bool prevClimb = false;

    override public void Start()
    {
        base.Start();
        animalName = "Raccoon";
        playerRigidBody = GetComponentInParent<Rigidbody>();
        normalRotation = transform.rotation;
    }

    // Update is called once per frame
    override public void OnJump(bool isHeld)
    {
        //Actions that happen on the space bar being pressed, like jumping or flying.
        if (prevClimb && !isHeld) 
        {
            playerRigidBody.useGravity = true;
            transform.Rotate(Vector3.right, 90);
            prevClimb = false;
        }
        else if (Physics.Raycast(transform.position, transform.forward, out hit, 1.0f))
        {
            if (hit.collider.tag == "Climeable")
            {
                print(hit.collider.tag);
                playerRigidBody.useGravity = false;
                transform.Rotate(Vector3.right, -90);
                prevClimb = true;
            }
        }
        
    }


}
