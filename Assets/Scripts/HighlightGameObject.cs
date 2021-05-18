using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightGameObject
{
    private Renderer renderer;
    private Color oldColor;
    private Color newColor;
    private float currentAmount;
    private bool reverse;
    private float lowValue;
    
    public HighlightGameObject()
    {
        renderer = null;
        newColor = new Color(0.0f, 0.0f, 0.0f, 0.75f);
    }

    public void SetGameObject(GameObject gameObj)
    {
        renderer = gameObj.GetComponent<Renderer>();
        oldColor = renderer.material.color;
        //lowValue = oldColor.g;
        //currentAmount = lowValue;
        newColor.r = oldColor.r;
        newColor.g = oldColor.g;
        newColor.b = oldColor.b;
        renderer.material.color = newColor;
    }

    /*
     * Changes the color until it is about yellow, then scales back down.
     */
    public void ChangeColor()
    {
        /*
        if (currentAmount > 3.0f)
            reverse = true;
        else if (currentAmount < lowValue)
            reverse = false;

        
        if (!reverse)
            currentAmount += 0.01f;
        else
            currentAmount -= 0.01f;
        
        */
        newColor.r += 0.75f;
        newColor.g += 0.75f;
        renderer.material.color = newColor;
    }

    /*
     * Resets the game objects material back to original color.
     */
    public void RemoveGameObject()
    {
        if (renderer != null)
            renderer.material.color = oldColor;
            renderer = null;
    }
}
