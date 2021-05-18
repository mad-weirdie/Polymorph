using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbs : MonoBehaviour
{
    public Material offMaterial;
    public Material onMaterial;
    public GameObject activator;

    bool isActivated;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Light>().enabled = false;
        GetComponent<Renderer>().material = offMaterial;
    }

    public void Activate()
    {
        GetComponent<Light>().enabled = true;
        GetComponent<Renderer>().material = onMaterial;
    }

    public void Deactivate()
    {
        GetComponent<Light>().enabled = false;
        GetComponent<Renderer>().material = offMaterial;
    }
}
