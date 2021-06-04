using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Achievement
{
    public string text;
    public bool isUnlocked;
    

    // Default constructor
    public Achievement()
    {
        text = "Achievement Unlocked";
        isUnlocked = false;
    }

    // Constructor with specific message
    public Achievement(string message)
    {
        text = "Achievement Unlocked\n" + message;
        isUnlocked = false;
    }

    public virtual bool getAchievementConditions()
    {
        return isUnlocked;
    }
}
