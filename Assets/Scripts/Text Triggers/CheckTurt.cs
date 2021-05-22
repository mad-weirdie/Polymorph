using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckTurt : HelpfulText
{
    public PlayerController player;

    override public bool conditionsMet()
    {
        return (player.activeScript.animalName == "Turtle");
    }
}
