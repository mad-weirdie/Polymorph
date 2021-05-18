using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public Material offMaterial;
    public Material onMaterial;

    bool isActivated;
    public List<GameObject> crystals;
    private int crystalsActivated;
    private int crystalsRequired;

    void Start ()
    {
        isActivated = false;
        crystalsActivated = 0;
        crystalsRequired = 5;
        GetComponent<Renderer>().material = offMaterial;
    }

    // This is called by the objects that hold the crystals
    public void AddCrystal()
    {
        crystalsActivated++;
        if (crystalsActivated == crystalsRequired)
        {
            ActivatePortal();
        }
    }

    public void RemoveCrystal()
    {
        crystalsActivated--;
    }

    private void ActivatePortal()
    {
        print("TURN IT ON!");
        isActivated = true;
        GetComponent<Renderer>().material = onMaterial;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isActivated)
        {
            SceneManager.LoadScene("Forest", LoadSceneMode.Single);
        }
    }
}
