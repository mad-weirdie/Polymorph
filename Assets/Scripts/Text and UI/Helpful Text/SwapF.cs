using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class swapListener : Listener
{
    public SwapF swapText;
    public swapListener(SwapF s, PlayerController p)
    {
        playerController = p;
        swapText = s;
        listenerType = "Grab";
    }

    public override void Notify()
    {
        if (playerController.isGrabbing)
        {
            swapText.message = swapText.otherText;
        }
        else
        {
            swapText.message = swapText.firstText;
        }
        swapText.Show();
    }
}

public class SwapF : HelpfulText
{
    public string firstText;
    public string otherText;
    public SwapF s;
    public swapListener swapper;

    public void Start()
    {
       swapper = new swapListener(s, player);
       player.AddListener(swapper);
    }

    override public bool conditionsMet()
    {
        return true;
    }

    override public void Remove()
    {
        return;
    }
}
