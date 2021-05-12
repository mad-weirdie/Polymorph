using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeSaw : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            rb.isKinematic = true;
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            rb.isKinematic = false;
        }
    }

}
