using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowAI : MonoBehaviour
{
    public GameObject crowNPC;
    public float jumpInterval;
    public float jumpHeight;
    private Animator crowAnim;
    bool hasBeenCollected;

    // Start is called before the first frame update
    void Start()
    {
        hasBeenCollected = false;
        crowAnim = crowNPC.GetComponent<Animator>();
        StartCoroutine("CrowHop");
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            hasBeenCollected = true;
            other.enabled = false;
        }
    }

    IEnumerator CrowHop()
    {
        if (!hasBeenCollected && Physics.Raycast(transform.position, Vector3.down, .5f))
        {
            crowAnim.Play("Jump");
            crowAnim.SetBool("IsFlying", true);
            crowNPC.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            yield return new WaitForSeconds(jumpInterval);
            StartCoroutine("CrowHop");
        }
        else
        {
            crowAnim.SetBool("IsFlying", false);
        }
    }
}
