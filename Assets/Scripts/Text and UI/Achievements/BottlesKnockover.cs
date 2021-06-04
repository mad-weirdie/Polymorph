using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottlesKnockover : MonoBehaviour
{
    public AchievementController achievements;
    public int numDisplaced = 0;
    // Start is called before the first frame update

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name != "Player")
        {
            numDisplaced++;
            if (numDisplaced == 5)
                achievements.Activate(achievements.A5);
        }
    }
}
