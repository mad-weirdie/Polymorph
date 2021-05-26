using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pedestals : MonoBehaviour
{
    public Orbs crystal;
    public Portal teleporter;
    public bool isActivated;

    void Start()
    {
        isActivated = false;
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Activation Crystal")
        {
            isActivated = true;
            crystal.Activate();
            teleporter.AddCrystal();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Activation Crystal")
        {
            isActivated = false;
            crystal.Deactivate();
            teleporter.RemoveCrystal();
        }
    }
}
