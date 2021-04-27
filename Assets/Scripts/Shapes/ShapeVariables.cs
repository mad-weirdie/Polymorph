using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeVariables : MonoBehaviour
{

    // This script handles variables related to each form, as well as unique actions.
    public float animalSpeed = 2f;
    public float turnSpeed = 2f; // do we want this?
    public GameObject player; //Ref so that actions can affect the player.

    public PlayerController script;
    public virtual void Start()
    {
        script = player.GetComponent<PlayerController>();
    }

    public virtual void OnJump(bool isHeld) {
        //Actions that happen on the space bar being pressed, like jumping or flying.
        //Override in an shape-specific script.
        
    }


}
