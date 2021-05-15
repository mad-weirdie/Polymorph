using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeSaw : MonoBehaviour
{
    // Start is called before the first frame update
    /*
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (rb.isKinematic == true && other.transform.parent != null && other.transform.parent.CompareTag("Player")) {
            print("Player entered seesaw trigger!");
            rb.isKinematic = false;
        }


    }

    private void OnTriggerExit(Collider other)
    {
        
        if (rb.isKinematic == false && other.transform.parent != null && other.transform.parent.CompareTag("Player"))
        {
            print("Player exited! seesaw trigger!");
            //rb.isKinematic = true;
        }
    }
    */

}
