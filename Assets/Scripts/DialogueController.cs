using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public TextAsset textFile;
    string[] dialogueLines;
    public TMP_Text dialogueText;
    public int currentLine;

    // Start is called before the first frame update
    void Start()
    {
        if (textFile != null)
        {
            dialogueLines = (textFile.text.Split('\n'));
        }
        currentLine = 0;
    }

    // Update is called once per frame
    public void Notify()
    {
        if (currentLine < dialogueLines.Length)
        {
            string dialogue = dialogueLines[currentLine];
            dialogueText.text = dialogue;
            currentLine++;
        }
    }
}
