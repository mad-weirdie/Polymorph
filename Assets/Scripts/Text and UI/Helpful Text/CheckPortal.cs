using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckPortal : HelpfulText
{
    public Portal portal;

    override public bool conditionsMet()
    {
        print(!portal.isActivated);
        return (!portal.isActivated);
    }

    override public void Remove()
    {
        return;
    }
}
