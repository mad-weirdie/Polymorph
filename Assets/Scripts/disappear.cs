using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disappear : MonoBehaviour
{
   void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player Camera")
        {
            print("Disappear!");
            transform.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Main Camera")
        {
            transform.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
