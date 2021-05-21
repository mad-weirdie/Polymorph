using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamVent : MonoBehaviour
{
    public AudioSource steamSound;
    public ParticleSystem steamParticles;

    public float repeatInterval;
    public float offsetTime;

    // Start is called before the first frame update
    void Start()
    {
        // This waits 1.5 seconds, then calls TriggerSteam every repeatInterval seconds
        InvokeRepeating("TriggerSteam", 2.5f, repeatInterval);
        InvokeRepeating("SteamSound", 2.5f - offsetTime, repeatInterval);
        InvokeRepeating("ResetSteam", 2.5f, repeatInterval + 8.0f);
    }

    void TriggerSteam()
    {
        steamParticles.Play();
    }

    void SteamSound()
    {
        steamSound.Play();
    }

    void ResetSteam()
    {
        steamParticles.Stop();
        steamSound.Stop();
    }
}
