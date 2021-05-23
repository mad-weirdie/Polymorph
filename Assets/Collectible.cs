using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    private bool isAnimating;
    private float timePlaying;
    private float animationTime = 1f;
    public float animationDuration = 4f;

    public int collectibleNum = 0;

    private Vector3 scaleFactor;
    // Start is called before the first frame update
    void Start()
    {
         scaleFactor = Vector3.one * 1 / (animationDuration - animationTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isAnimating)
        {
            return;

        }

        transform.Rotate(Vector3.up, 360 * Time.deltaTime);
        transform.Translate(Vector3.up * .5f * Time.deltaTime);
        transform.localScale -= scaleFactor * Time.deltaTime;
        animationTime += Time.deltaTime;
        if (animationTime > animationDuration) {
            Destroy(gameObject);
        }

    }

    private void playAnimation() 
    {
    
        
    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            isAnimating = true;
            print(PersistentData.CrystalsCollected == null); 
            PersistentData.CrystalsCollected[collectibleNum] = true;
            print(PersistentData.CrystalsCollected[collectibleNum]);
        
        }
    }

}
