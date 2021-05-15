using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Flicker : MonoBehaviour
{
    public Light crystal;
    public float minIntensity = 0.000001f;
    public float maxIntensity = 0.02f;
    public float speed = 3f;
    public bool flicker = true;
    float noise = 0.2f;

    void Start()
    {
        crystal = GetComponent<Light>();
    }

    void Update()
    {
        if (flicker)
        {
            noise = Random.Range(0, noise);
            crystal.intensity = Mathf.PingPong(Time.time * speed, maxIntensity) + minIntensity + noise;
            
        }
    }

}