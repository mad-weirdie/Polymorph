using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pedestals : MonoBehaviour
{
    public Orbs crystal;
    public portal teleporter;

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Activation Crystal")
        {
            crystal.Activate();
            teleporter.AddCrystal();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Activation Crystal")
        {
            crystal.Deactivate();
            teleporter.AddCrystal();
        }
    }
}
