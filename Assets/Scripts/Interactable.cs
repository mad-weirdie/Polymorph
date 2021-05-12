using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{
    public string InteractText = "Press F to open the door";
    public GameObject player;
    Vector2 mousePosition;
    bool inRange;
    RaycastHit hit;
    Ray ray;
    public Camera mainCamera;
    public TMP_Text text;
     
    // Start is called before the first frame update
    void Start()
    {
        text.text = InteractText;
        text.gameObject.SetActive(false);
        
    }

    void OnCollisionEnter(Collision interactable)
    {
        if (interactable.gameObject.CompareTag("Object"))
        {
            inRange = true;
            text.gameObject.SetActive(true);
        }
        if (interactable.gameObject.CompareTag("Crystal"))
        {
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
}
