using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamVent : MonoBehaviour
{
    public AudioSource steamSound;
    public ParticleSystem steamParticles;

    // Time to wait after loading the scene before beginning
    public float startDelay;
    // How often to repeat the steam emission
    public float repeatInterval;

    // Offset between time when the sound is played,
    // and the time that the steam vent effect happens
    public float offsetTime;
    // Length of time before we stop both the sound and the vent
    public float steamDuration;
    
    public float ventForce = 400f;

    private Vector3 ventMatrix;
    // Check whether the vent is active, so we know when to apply the force
    private bool isOn;

    // Start is called before the first frame update
    void Start()
    {
        // This waits 1.5 seconds, then calls TriggerSteam every repeatInterval seconds
        InvokeRepeating("TriggerSteam", startDelay, repeatInterval);
        InvokeRepeating("SteamSound", startDelay - offsetTime, repeatInterval);

        ventMatrix = transform.TransformDirection(Vector3.up * ventForce);

    }

    // Trigger the steam particle effect
    void TriggerSteam()
    {
        steamParticles.Play();
        isOn = true;
        print("Steam vent on!");
        StartCoroutine(ResetSteam(steamDuration));
    }

    // Play the sound effect for the steam
    void SteamSound()
    {
        steamSound.Play();
    }

    // Stop both the sound and the steam
    IEnumerator ResetSteam(float numSeconds)
    {
        yield return new WaitForSeconds(numSeconds);
        steamParticles.Stop();
        steamSound.Stop();
        isOn = false;

        print("Steam vent off!");
    }

    // Add force to the steam!
    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody && isOn) {
            other.attachedRigidbody.AddForce(ventMatrix);
        }
    }

}
