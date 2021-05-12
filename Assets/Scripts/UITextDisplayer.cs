using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class UITextDisplayer : MonoBehaviour
{
    public string InteractText = "Press F to open the door";
    Vector2 mousePosition;
    bool inRange;
    bool lookComplete;
    bool moveComplete;
    RaycastHit hit;
    Ray ray;
    public Camera mainCamera;
    public TMP_Text text;
    public LayerMask ignorePlayer;
    public PlayerController player;
    public DialogueController dialogue;
     
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Hold());
        lookComplete = false;
        moveComplete = false;
        text.text = InteractText;
        text.gameObject.SetActive(false);
    }

    IEnumerator Hold()
    {
        return new WaitForSecondsRealtime(5);
    }

    void OnCollisionEnter(Collision interactable)
    {
        if (interactable.gameObject.CompareTag("Object"))
        {
            inRange = true;
            text.gameObject.SetActive(true);
        }
    }

    void OnTriggerStay(Collider interactable)
    {
        if (interactable.gameObject.name == "Start")
        {
            if (!dialogue.IsHappening() && !lookComplete)
                LookCheck();
            if (!dialogue.IsHappening() && lookComplete && !moveComplete)
            {
                text.text = "Use WASD to get out of bed.";
            }
        }
        else if (interactable.gameObject.name == "LeaveBed Trigger" && !moveComplete)
            {
                if (lookComplete)
                {
                    moveComplete = true;
                    dialogue.SetHappening(true);
                    text.text = "";
                    dialogue.Notify();
                }
            }
    }
    void OnCollisionExit(Collision interactable)
    {
        if (interactable.gameObject.CompareTag("Object"))
        {
            inRange = false;
            text.gameObject.SetActive(false);
        }
    }

    void OnGrab()
    {
        if (inRange)
            SceneManager.LoadScene("Forest", LoadSceneMode.Single);
    }

    public void LookCheck()
    {
        ray = mainCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, mainCamera.nearClipPlane));
        if (Physics.Raycast(ray, out hit, 1000f, ignorePlayer))
        {
            if (hit.transform.name == "Crystal_03")
            {
                lookComplete = true;
                text.gameObject.SetActive(true);
                text.text = "";
                player.movementEnabled = false;
                dialogue.SetHappening(true);
                dialogue.Notify();
            }
        }
        else
        {
            text.text = "Use the mouse to look at the crystal.";
            text.gameObject.SetActive(true);
        }

    }

}
