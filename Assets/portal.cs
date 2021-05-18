using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    public List<GameObject> crystals;
    private int numActivated;
    private int numRequired;
    // Start is called before the first frame update
    void Start ()
    {
        numActivated = 0;
        numRequired = 5;
    }

    // Update is called once per frame
    public void AddCrystal()
    {
        numActivated++;
        if (numActivated == numRequired)
        {
            ActivatePortal();
        }
    }

    private void ActivatePortal()
    {
        print("TURN IT ON!");
    }
}
