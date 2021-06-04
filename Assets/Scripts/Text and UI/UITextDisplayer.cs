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
        hasKey = false;
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
        if (interactable.gameObject.name == "Door")
        {
            inRange = true;
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
        if (interactable.gameObject.name == "Door")
        {
            inRange = false;
            text.gameObject.SetActive(false);
        }
    }

    void OnGrab()
    {
        if (inRange && hasKey)
        {
            SceneManager.LoadScene("Forest", LoadSceneMode.Single);
            dialogue.Notify();
            dialogue.Notify();
        }
    }

    public void UpdateKey(bool keyState)
    {
        hasKey = keyState;
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
            lookComplete = true;
            text.gameObject.SetActive(true);
            text.text = "";
            dialogue.SetHappening(true);
            dialogue.Notify();
            //text.text = "Use the mouse to look at the crystal.";
            text.gameObject.SetActive(true);
        }

    }

}
