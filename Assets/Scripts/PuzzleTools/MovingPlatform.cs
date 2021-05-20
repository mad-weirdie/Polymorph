using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : ResettablePhysObject, IGenericBehavior
{
    public bool state; //State of the platform. FALSE for initial, TRUE for second.
    [SerializeField]
    public float LerpState;
    public float LerpSpeed = .1f; //lerp per fixedUpdate

    //Position to start in.
    [SerializeField]
    private Vector3 startPosition;

    public Transform endPosition;

    private void FixedUpdate()
    {
        if((state == true && LerpState == 1f) || (state == false && LerpState == 0f)){ return; } //If we're at one position, don't eval and return.

        if (state == true)
        {
            LerpState = Mathf.Min(LerpState + LerpSpeed, 1f);
        }
        else {
            LerpState = Mathf.Max(LerpState - LerpSpeed, 0f);
        }

        transform.position = Vector3.Lerp(startPosition, endPosition.position, LerpState);

    }

    public override bool saveInitials()
    {
        //Nothing to save with this one!
        return true;

    }

    public override bool reset()
    {
        state = false;
        transform.position = startPosition;
        return true;

    }

    void Start()
    {
        saveInitials();
        LerpState = 0f;
        startPosition = transform.position;

    }

    public void SetState(bool newState) {
        state = newState;

    }


}