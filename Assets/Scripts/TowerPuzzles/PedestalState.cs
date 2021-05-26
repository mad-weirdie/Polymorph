using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestalState : MonoBehaviour
{
    public barricade wall;
    private bool Activated;
    public string crystalColor;
    public AudioSource crystalDing;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == crystalColor && !Activated)
        {
            Destroy(other.gameObject.GetComponent<Rigidbody>());
            other.gameObject.transform.position = new Vector3(gameObject.transform.position.x, other.gameObject.transform.position.y + 0.4f, gameObject.transform.position.z);
            other.isTrigger = false;
            wall.AddCrystal();
            Activated = true;
            crystalDing.Play();
        }
    }
}
