using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barricade : MonoBehaviour
{
    private int ActivatedCrystals;
    public int RequiredCrystals; 

    public void AddCrystal()
    {
        ActivatedCrystals++;
        if (ActivatedCrystals == RequiredCrystals)
        {
            Destroy(this.gameObject);
        }
    }
}
