using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingCrystal : MonoBehaviour
{
    public ExplosionController explosionSite;
    public bool inRange;

    void Start()
    {
        inRange = false;
    }

    void OnCollisionEnter(Collision data)
    {
        print("impact!");
        if (data.impulse.y > 20 && inRange)
        {
            
            explosionSite.checkIfExplode();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Explosion Controller")
        {
            print("crystal in range");
            inRange = true;
        }
    }
}
