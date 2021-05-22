using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Listener
{
    public PlayerController playerController;
    public abstract void Notify();
}
