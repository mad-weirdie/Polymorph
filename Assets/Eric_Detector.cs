using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eric_Detector : MonoBehaviour
{
    public AchievementController achievements;
    private PlayerController player;
    private bool seen = false;
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBecameVisible()
    {
        if (!seen)
        {
            seen = true;
            achievements.Activate(achievements.A6);
        }
    }
}
