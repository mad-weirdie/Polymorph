using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HelpfulText : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController player;
    public string message;
    public TextMeshProUGUI HelpfulTextObject;
    public float TextShowSeconds = 2.0f;

    void Start()
    {
        HelpfulTextObject.CrossFadeAlpha(0.0f, 0.0f, false);
    }

    public virtual bool conditionsMet()
    {
        return false;
    }

    public void Show()
    {
        StartCoroutine(_show());
    }

    private IEnumerator _show()
    {  
        HelpfulTextObject.CrossFadeAlpha(1.0f, 0.5f, false);
        print("Started");
        HelpfulTextObject.gameObject.SetActive(true);
        HelpfulTextObject.SetText(message);
        yield return new WaitForSeconds(TextShowSeconds);
        HelpfulTextObject.CrossFadeAlpha(0.0f, 0.5f, false);
        print("Finished");

    }

    public virtual void Remove()
    {
        print("Removing.");
        Destroy(this.gameObject);
        //HelpfulTextObject.gameObject.SetActive(false);
    }
}
