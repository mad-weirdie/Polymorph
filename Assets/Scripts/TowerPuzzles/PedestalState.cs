using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestalState : MonoBehaviour
{
    public barricade wall;
    private bool Activated;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Activation Crystal" && !Activated)
        {
            Destroy(other.gameObject.GetComponent<Rigidbody>());
            other.gameObject.transform.position = new Vector3(gameObject.transform.position.x, other.gameObject.transform.position.y + 0.5f, gameObject.transform.position.z);
            other.isTrigger = false;
            wall.AddCrystal();
            Activated = true;
        }
    }
}
