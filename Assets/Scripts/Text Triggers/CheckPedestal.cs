using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckPedestal : HelpfulText
{
    public Pedestals pedestal;

    override public bool conditionsMet()
    {
        return (!pedestal.isActivated);
        
    }

    override public void Remove()
    {
        return;
    }
}
