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
    private bool show_dialogue_box;

    public bool dialogueHappening;
    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        if (textFile != null)
        {
            dialogueLines = (textFile.text.Split('\n'));
        }
        currentLine = 0;
        dialogueHappening = true;
        show_dialogue_box = true;
        player.movementEnabled = false;
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
        else
        {
            dialogueText.text = "";
            dialogueHappening = false;
            player.movementEnabled = true;
            show_dialogue_box = false;
        }
    }
}
