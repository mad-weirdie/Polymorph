using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public Transform goal;

    private bool triggered;
    private bool reached;
    float xPos;
    float zPos;
    float dist;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0.75f;
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered && !reached)
        {
            // Move closer to goal.
            dist = Vector3.Distance(gameObject.transform.position, goal.position);

            if (dist > 0.1f)
            {
                Vector3 dir = goal.position - gameObject.transform.position;
                print(dir);
                gameObject.transform.position = new Vector3(
                    gameObject.transform.position.x + (dir.x * speed * Time.deltaTime),
                    gameObject.transform.position.y,
                    gameObject.transform.position.z + (dir.z * speed * Time.deltaTime));
            }
            else
            {
                reached = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (reached)
        {
            print("reached");
            triggered = false;
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            triggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        triggered = false;
    }
}
