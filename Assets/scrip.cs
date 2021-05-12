using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrip : MonoBehaviour
{
    AudioSource music;
    // Start is called before the first frame update
    void Start()
    {
        music = GetComponent<AudioSource>();
        music.ignoreListenerVolume = true;
        music.volume = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
