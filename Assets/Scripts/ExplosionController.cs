using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public float radius = 5.0f;
    public float intensity = 10.0f;
    public float projectionForce = 30;
    public bool explosionTime = false;
    public Transform ExplosionCenter;
    public ParticleSystem explosionParticles;
    private Rigidbody explodingObject;

    public bool explosivesInRange;


    public void checkIfExplode()
    {
        Explode();
    }

    void Explode()
    {
        print("EXPLODE!!!!!");
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
                else if (explodingObject.mass >= 12)
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
                    Destroy(explodingObject.gameObject);
                }
                else
                {
                    explodingObject.isKinematic = false;
                    explodingObject.AddExplosionForce(intensity, explosionPos, radius, projectionForce);
                }
            }
        }

        //Destroy(this);

    }
}
