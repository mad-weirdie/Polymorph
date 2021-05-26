using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckPedestal : HelpfulText
{
    public pedestals pedestal;

    override public bool conditionsMet()
    {
        return (!pedestal.isActivated);
        
    }

    override public void Remove()
    {
        return;
    }
}
