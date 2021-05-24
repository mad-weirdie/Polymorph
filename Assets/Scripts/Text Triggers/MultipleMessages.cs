using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultipleMessages : HelpfulText
{
    public List<string> messages;
    int messageIndex = 0;
    public float textDelay = 5f;

    override public bool conditionsMet()
    {
        return (!messagesBegan);
    }

    override public void Show()
    {
        if (messages != null && messages[0] != null)
        {
            message = messages[0];
        }
        messagesBegan = true;
        print("show");
        StartCoroutine("showMessages");
    }

    IEnumerator showMessages()
    {
        print("show messages:  ");
        print("num messages:  " + messages.Count);
        HelpfulTextObject.CrossFadeAlpha(1.0f, 0.5f, false);
        HelpfulTextObject.gameObject.SetActive(true);
        HelpfulTextObject.SetText(message);
        yield return new WaitForSeconds(TextShowSeconds);
        HelpfulTextObject.CrossFadeAlpha(0.0f, 0.5f, false);
        messageIndex++;
        if (messageIndex < messages.Count)
            message = messages[messageIndex];
        else
            Remove();
        StartCoroutine("textChangeDelay");
    }

    IEnumerator textChangeDelay()
    {
        print("text change delay");
        yield return new WaitForSeconds(textDelay);
        StartCoroutine("showMessages");
    }
}
