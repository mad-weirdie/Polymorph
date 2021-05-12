using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialScript : DialogueController
{
    // Array to store lines of dialogue
    string[] dialogueLines;

    // Keep track of which line to display
    // This also enables us to check if we have run out of dialogue
    private int currentLine;

    // Controls the fade in/out of the dialogue box when dialogue is happening
    private Animator dialogueAnim;

    // ========================================================================
    
    void Start()
    {
        dialogueHappening = false;
        // Don't try to retrieve any components if dialogue is disabled
        if (!dialogueEnabled)
        {
            return;
        }
        // Otherwise, read in the lines of dialogue from the text file
        else
        {
            // Get animator component
            dialogueAnim = dialogueBox.GetComponent<Animator>();
            // Turn off the shaded dialogue box initially 
            dialogueAnim.SetBool("dialogueHappening", false);
            // Check a text file actually exists
            if (textFile != null)
            {
                dialogueLines = (textFile.text.Split('\n'));
                currentLine = 0;
                dialogueHappening = true;
                player.movementEnabled = false;
            }
        }

    }

    override public void Notify()
    {
        if (dialogueHappening)
        {
            dialogueAnim.SetBool("dialogueHappening", true);
            if (currentLine < dialogueLines.Length)
            {
                string dialogue = dialogueLines[currentLine];
                if (dialogue[0] == '#')
                {
                    dialogueHappening = false;
                    dialogueAnim.SetBool("dialogueHappening", dialogueHappening);
                    dialogueText.text = "";
                    player.movementEnabled = true;
                }
                else
                    dialogueText.text = dialogue;

                currentLine++;
            }
            else
            {
                dialogueText.text = "";
                dialogueHappening = false;
                dialogueAnim.SetBool("dialogueHappening", dialogueHappening);
                player.movementEnabled = true;
            }
        }
        else
        {
            player.movementEnabled = true;
        }
    }
}

