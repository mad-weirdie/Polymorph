using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSounds : MonoBehaviour
{
    public AudioSource drop;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > 2 && !drop.isPlaying)
            drop.Play();
    }
}
