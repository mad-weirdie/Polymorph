using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MineEntrance : HelpfulText
{
    bool insideTrigger = false;

    override public bool conditionsMet()
    {
        return insideTrigger;
    }

    override public void Remove()
    {
        return;
    }

    void OnTriggerEnter()
    {
       
        insideTrigger = true;
    }

    void OnTriggerExit()
    {
        insideTrigger = false;
    }
}
