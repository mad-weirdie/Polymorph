using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentData
{
    //Data inherited by the player. Set as static in that script.
    public static List<bool> CrystalsCollected;
    public static bool hasInit;
    public static void Start()
    {



        CrystalsCollected = new List<bool>();
        for (int i = 0; i < 4; i++)
        {
            CrystalsCollected.Add(false); //We have no crystals collected at the start!
        }

    }









}
