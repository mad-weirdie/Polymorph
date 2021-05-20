using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGenericBehavior
{
    //Generic call-response system. SetState controls a state local to whatever script is running.
    void SetState(bool newState);
}
