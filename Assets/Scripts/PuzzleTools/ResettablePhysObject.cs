using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResettablePhysObject : resettableObject
{

    private Quaternion initialRotation;
    private Vector3 initialPosition;

    public override bool saveInitials()
    {
        initialRotation = transform.rotation;
        initialPosition = transform.position;
        return true;
        
    }

    public override bool reset()
    {
        print("RESETTING");
        transform.rotation = initialRotation;
        transform.position = initialPosition;
        return true;

    }

    void Start() {
        saveInitials();
    
    }

}