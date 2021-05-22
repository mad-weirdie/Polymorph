using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            PlayerController pc = other.gameObject.GetComponentInParent<PlayerController>();
            pc.lastCheckpointPos = other.transform.position;
            pc.lastCheckpointDir = other.transform.rotation;
        
        }
    }

}
