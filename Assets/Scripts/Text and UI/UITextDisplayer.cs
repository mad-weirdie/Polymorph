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
    bool hasKey;
    bool foundLockedDoor;
    bool draggingExplanationStarted;
    bool finishedExplanation;

    public GameObject key;

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
        finishedExplanation = false;
        foundLockedDoor = false;
        hasKey = false;
        draggingExplanationStarted = false;
        lookComplete = false;
        moveComplete = false;
        text.text = InteractText;
        text.gameObject.SetActive(false);
        player.movementEnabled = false;
    }

    IEnumerator Hold()
    {
        yield return new WaitForSecondsRealtime(2);
        dialogue.Notify();
        text.text = "";
    }

    IEnumerator WaitForInstruction()
    {
        draggingExplanationStarted = true;
        yield return new WaitForSecondsRealtime(2);
        text.text = "Press 'F' to grab an object.";

        StartCoroutine(NextInstruction());
    }

    IEnumerator NextInstruction()
    {
        yield return new WaitForSecondsRealtime(3);
        text.text = "Press 'F' again to release.";
        StartCoroutine(EndInstructions());
    }

    IEnumerator EndInstructions()
    {
        yield return new WaitForSecondsRealtime(3);
        text.text = "";
    }

    IEnumerator WaitToForest()
    {
        yield return new WaitForSecondsRealtime(3);
        dialogue.Notify();
    }

    void OnCollisionEnter(Collision interactable)
    {
        if (interactable.gameObject.CompareTag("Object"))
        {
            inRange = true;
            text.gameObject.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        if (!dialogue.IsHappening() && finishedExplanation && !hasKey && foundLockedDoor && !draggingExplanationStarted)
        {
            StartCoroutine(WaitForInstruction());
        }
    }

    void OnTriggerStay(Collider interactable)
    {
        if (dialogue.IsHappening())
        {
            text.text = "";
        }

        if (!dialogue.IsHappening())
            {
                player.movementEnabled = true;
                Rigidbody rb = player.GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.None;
                player.baseSpeed = 0.04f;
                player.turnSpeed = 20f;
                finishedExplanation = true;
            }

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

    void OnTriggerExit(Collider interactable)
    {
        if (interactable.gameObject.name == "Door")
        {
            text.text = "";
            inRange = false;
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
        {
            SceneManager.LoadScene("Forest", LoadSceneMode.Single);
            dialogue.Notify();
            dialogue.Notify();
        }

    }

    void OnSkip()
    {
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
