using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeVariables : MonoBehaviour
{
    // This script handles variables related to each form, as well as unique actions.
    public float animalSpeed = 2f;
    /*
     * CURRENT LIST OF ANIMAL NAMES
     *     "Turtle"
     *     "Horse"
     *     "Crow"
     */
    public string animalName = "";
    public float turnSpeed = 2f; // do we want this?
    public GameObject player; //Ref so that actions can affect the player.

    public float animalMass = 20f; //how heavy we are as an animal. Affects things like see-saws.
    public Vector3 shapeOffsets;
    public float LowCameraYOffset = 1f;

    public ParticleSystem magicAnimation;
    private bool hasPlayed;

    public PlayerController script;
    public virtual void Start()
    {
        hasPlayed = false;
        script = player.GetComponent<PlayerController>();
    }

    public virtual void OnJump(bool isHeld) {
        //Actions that happen on the space bar being pressed, like jumping or flying.
        //Override in an shape-specific script.
        
    }

    public void PlayMagicEffect()
    {
        if (magicAnimation != null)
        {
            magicAnimation.Play();
            hasPlayed = true;
        }
    }

    public void Update()
    {
        if (magicAnimation && hasPlayed)
            if (!magicAnimation.IsAlive())
                Destroy(magicAnimation.gameObject);
    }


}
