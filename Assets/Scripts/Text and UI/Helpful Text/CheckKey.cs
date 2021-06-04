using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckKey : HelpfulText
{
    public UITextDisplayer display;
    bool hasKey = false;
    public GameObject key;

    override public bool conditionsMet()
    {
        if (keyInRange())
            message = "Press 'F' to leave the house";
        else
            message = "The door is locked. You need to find the key.";

        return true;
    }

    override public void Remove()
    {
        return;
    }

    bool keyInRange()
    {
        float xpos = key.transform.position.x;
        float ypos = key.transform.position.y;
        float zpos = key.transform.position.z;

        if (xpos < 0f && xpos > -2.5f && zpos < 16.0f && zpos > 13.0f)
            hasKey = true;
        else
            hasKey = false;

        display.UpdateKey(hasKey);

        return hasKey;
    }
}
