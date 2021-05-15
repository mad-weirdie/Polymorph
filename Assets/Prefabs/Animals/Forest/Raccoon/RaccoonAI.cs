using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaccoonAI : MonoBehaviour
{
    public Transform[] targets;
    public float speed = 0.08f;
    private int current;
    public GameObject player;
    Animator anim;
    Vector3 pos;
    Ray towardsPlayer;
    RaycastHit hit;

    void Start()
    {
        current = 0;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        towardsPlayer = new Ray(transform.position, transform.TransformPoint(player.transform.position) - transform.position);
        Debug.DrawRay(transform.position, transform.TransformPoint(player.transform.position));
        if(GetComponent<Collider>().Raycast(towardsPlayer, out hit, 30.0f))
        {
            anim.SetBool("IsSpooked", true);
            print(current);
        }

        if (anim.GetBool("IsSpooked") && (transform.position != targets[current].position))
        {
            pos = Vector3.MoveTowards(transform.position,targets[current].position, speed);
            GetComponent<Rigidbody>().MovePosition(pos);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Waypoint"))
        {
            if (current < targets.Length)
                current = (current + 1);
        }
    }
}
