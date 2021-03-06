using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    // Turn off dialogue for debugging
    public bool dialogueEnabled;

    // Input file for dialogue
    // Found in Assets->Text Files
    public TextAsset textFile;
    // Array to store lines of dialogue
    string[] dialogueLines;
    // The Text Mesh Pro GameObject which displays the on-screen text
    public TMP_Text dialogueText;
    // Keep track of which line to display
    // This also enables us to check if we have run out of dialogue
    private int currentLine;

    // Know if we are  in the middle of dialogue right now
    // We use this to pause player and camera movement during "cutscenes"
    public bool dialogueHappening;
    public PlayerController player;

    // Controls the fade in/out of the dialogue box when dialogue is happening
    private Animator dialogueAnim;
    public Animator titleAnim;
    public GameObject dialogueBox;

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
            titleAnim.SetBool("fadeInTime", false);
            // Check a text file actually exists
            if (textFile != null)
            {
                dialogueLines = (textFile.text.Split('\n'));
                currentLine = 0;
                dialogueHappening = true;
            }
        }

    }

    public bool IsHappening()
    {
        return dialogueHappening;
    }

    public void SetHappening(bool state)
    {
        dialogueHappening = state;
    }

    public string GetCurrentLine()
    {
        return dialogueLines[currentLine];
    }


    virtual public void Notify()
    {
        if (dialogueHappening)
        {
            if (currentLine <= 17)
                player.PauseGame();
            player.clickingEnabled = true;
            if (currentLine == 3)
            {
                titleAnim.SetBool("fadeInTime", true);
            }
            dialogueAnim.SetBool("dialogueHappening", true);
            if (currentLine < dialogueLines.Length)
            {
                string dialogue = dialogueLines[currentLine];
                if (dialogue[0] == '#')
                {
                    dialogueHappening = false;
                    dialogueAnim.SetBool("dialogueHappening", dialogueHappening);
                    dialogueText.text = "";
                }    
                else
                    dialogueText.text = dialogue;
                
                currentLine++;
            }
            else
            {
                dialogueText.text = "";
                dialogueHappening = false;
                dialogueAnim.SetBool("dialogueHappening", false);
            }
        }
        else
        {
            player.UnpauseGame();
        }
    }
}
