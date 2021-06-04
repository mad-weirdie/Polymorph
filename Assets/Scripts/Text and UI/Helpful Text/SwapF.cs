using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapF : HelpfulText
{
    public string firstText;
    public string otherText;

    override public bool conditionsMet()
    {
        return true;
    }

    override public void Remove()
    {
        return;
    }

    public void OnGrab()
    {
        if (player.isGrabbing)
        {
            message = otherText;
        }
        else
        {
            message = firstText;
        }
    }
}
