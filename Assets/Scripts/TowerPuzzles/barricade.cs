using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barricade : MonoBehaviour
{
    private int ActivatedCrystals;
    public int RequiredCrystals;
    public AllDontDestroy text;

    public void AddCrystal()
    {
        ActivatedCrystals++;
        if (ActivatedCrystals == RequiredCrystals)
        {
            text.HelpfulTextObject.SetText("");
            Destroy(this.gameObject);
        }
    }
}
