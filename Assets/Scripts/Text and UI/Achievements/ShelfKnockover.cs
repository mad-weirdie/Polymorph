using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfKnockover : MonoBehaviour
{
    public AchievementController achievements;
    // Start is called before the first frame update

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Bookshelf")
        {
            achievements.Activate(achievements.A5);
        }
    }
}
