using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstrainPositionLocal : MonoBehaviour
{
    //Convert a rigidbody's FreezePosition values from world space to local space.

    private Rigidbody rb;
    public bool constrainXPos;
    public bool constrainYPos;
    public bool constrainZPos;

    [SerializeField]
    private Vector3 localVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        constrainXPos = (rb.constraints & RigidbodyConstraints.FreezePositionX) == RigidbodyConstraints.FreezePositionX;
        constrainYPos = (rb.constraints & RigidbodyConstraints.FreezePositionY) == RigidbodyConstraints.FreezePositionY;
        constrainZPos = (rb.constraints & RigidbodyConstraints.FreezePositionZ) == RigidbodyConstraints.FreezePositionZ;
        rb.constraints = rb.constraints & ~RigidbodyConstraints.FreezePosition; //save whatever our remaining fields are.

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (rb.IsSleeping()) { print("SLEEP"); return; }
        localVelocity = transform.InverseTransformDirection(rb.velocity);
        if (constrainXPos) { localVelocity.x = 0; }
        if (constrainYPos) { localVelocity.y = 0; }
        if (constrainZPos) { localVelocity.z = 0; }

        //rb.velocity = transform.TransformDirection(localVelocity);
        //rb.AddForce(transform.TransformDirection(localVelocity));
    }
}
