using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour
{

    public static PersistentData Instance;

    public List<bool> CrystalsCollected;

    private void Start()
    {
        //Implement a singleton object which keeps track of our data between levels.
        if (Instance == null)
            Instance = this;
        
        else if (Instance != this) {
            Destroy(gameObject);
            return;
        }

        CrystalsCollected = new List<bool>();
        for (int i = 0; i < 4; i++)
        {
            CrystalsCollected.Add(false); //We have no crystals collected at the start!
        }

    }







}
