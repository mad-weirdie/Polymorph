using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float radius = 5.0f;
    public float intensity = 10.0f;
    public float projectionForce = 30;
    public bool explosionTime = false;
    public Transform ExplosionCenter;
    public ParticleSystem explosionParticles;
    private Rigidbody explodingObject;

    // Start is called before the first frame update
    void Start()
    {
        explosionTime = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (explosionTime)
        {
            print("EXPLODE!!!");
            Explode();
        }    
            
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !explosionTime)
        {
            print("player entered explode time area");
            explosionTime = true;
        }
    }

    void Explode()
    {
        Vector3 explosionPos = ExplosionCenter.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        explosionParticles.Play();
        foreach (Collider hitObject in colliders)
        {
            if (hitObject.GetComponent<Rigidbody>())
            {
                explodingObject = hitObject.GetComponent<Rigidbody>();
                print("apply force!");
                if (explodingObject.mass <= 1)
                {
                    Destroy(explodingObject.gameObject);
                }
                else if (explodingObject.mass >= 10)
                {
                    explodingObject.isKinematic = false;
                    Transform t = explodingObject.gameObject.GetComponent<Transform>();
                    t.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    t.position += new Vector3(0f, 0.5f, 0f);
                    Instantiate(explodingObject.gameObject);
                    t.position += new Vector3(0f, -1.0f, 0f);
                    Instantiate(explodingObject.gameObject);
                    t.position += new Vector3(0f, 0.5f, 1f);
                    Instantiate(explodingObject.gameObject);
                    explodingObject.AddExplosionForce(intensity, explosionPos, radius, projectionForce);
                    Destroy(explodingObject);
                }
                else
                {
                    explodingObject.isKinematic = false;
                    explodingObject.AddExplosionForce(intensity, explosionPos, radius, projectionForce);
                }
            }
        }

        Destroy(this);
        
    }
}
