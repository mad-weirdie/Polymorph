using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HelpfulText : MonoBehaviour
{
    // Start is called before the first frame update
    public TextAsset text_file;
    public TextMeshProUGUI HelpfulTextObject;
    public float TextShowSeconds = 2.0f;

    public void Show()
    {
        StartCoroutine(_show());
    }

    private IEnumerator _show()
    {
        print("Started");
        HelpfulTextObject.SetText(text_file.text);
        HelpfulTextObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(TextShowSeconds);
        HelpfulTextObject.CrossFadeAlpha(0.0f, 0.5f, false);
        print("Finished");

    }

    public void Remove()
    {
        HelpfulTextObject.gameObject.SetActive(false);
    }
}
