using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PersistentData
{
    //Data inherited by the player. Set as static in that script.
    public static List<bool> CrystalsCollected;
    public static bool hasInit;
    public static Vector3 spawnPoint;
    public static Quaternion spawnRotation;

    public static void Start()
    {
        
        if (hasInit) { return; }
        CrystalsCollected = new List<bool>();
        for (int i = 0; i < 4; i++)
        {
            CrystalsCollected.Add(false); //We have no crystals collected at the start!
        }

        spawnPoint = new Vector3(0f, 0f, 0f);
        spawnRotation = Quaternion.identity;
        hasInit = true;
    }









}
