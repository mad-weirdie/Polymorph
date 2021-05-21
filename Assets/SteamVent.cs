using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamVent : MonoBehaviour
{
    public AudioSource steamSound;
    public ParticleSystem steamParticles;

    public float repeatInterval;
    public float offsetTime;


    public float ventForce = 500f;

    private Vector3 ventMatrix;
    private bool isOn;
    // Start is called before the first frame update
    void Start()
    {
        // This waits 1.5 seconds, then calls TriggerSteam every repeatInterval seconds
        InvokeRepeating("TriggerSteam", 2.5f, repeatInterval);
        InvokeRepeating("SteamSound", 2.5f - offsetTime, repeatInterval);
        InvokeRepeating("ResetSteam", 2.5f, repeatInterval + 8.0f);

        ventMatrix = transform.TransformDirection(Vector3.up * ventForce);

    }

    void TriggerSteam()
    {
        steamParticles.Play();
        isOn = true;
    }

    void SteamSound()
    {
        steamSound.Play();
    }

    void ResetSteam()
    {
        steamParticles.Stop();
        steamSound.Stop();
        isOn = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody && isOn) {
            other.attachedRigidbody.AddForce(ventMatrix);
        
        }
    }

}
