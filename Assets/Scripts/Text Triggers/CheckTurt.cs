using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckTurt : HelpfulText
{ 
    override public bool conditionsMet()
    {
        return (player.activeScript.animalName == "Turtle");
    }

    override public void Remove()
    {
        return;
    }
}
