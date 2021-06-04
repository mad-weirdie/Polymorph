using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipTurt : MonoBehaviour
{
    public AchievementController achievements;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FloorTrigger")
        {
            achievements.Activate(achievements.A7);
        }
    }
}
