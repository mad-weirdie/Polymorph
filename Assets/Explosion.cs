using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float radius = 5.0f;
    public float intensity = 10.0f;
    public float projectionForce = 30;
    public bool explosionTime = false;

    // Start is called before the first frame update
    void Start()
    {
        explosionTime = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (explosionTime)
            Explode();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("player entered explode time area");
            explosionTime = true;
        }
    }

    void Explode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        transform.GetComponent<ParticleSystem>().Play();
        foreach (Collider hitObject in colliders)
        {
            if (hitObject.GetComponent<Rigidbody>())
            {
                hitObject.GetComponent<Rigidbody>().isKinematic = false;
                hitObject.GetComponent<Rigidbody>().AddExplosionForce(intensity, explosionPos, radius, projectionForce);
            }
        }
        
    }
}
