using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AllDontDestroy : HelpfulText
{
    override public bool conditionsMet()
    {
        return true;
    }

    override public void Remove()
    {
        return;
    }
}
