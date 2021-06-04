using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;

public class AchievementController : MonoBehaviour
{
    public GameObject achievementDisplay;
    public TMP_Text TMPText;
    public Animator anim;
    public PlayerController player;

    public List<Achievement> Achievements = new List<Achievement>();
    public Achievement A1;
    public Achievement A2;
    public Achievement A3;
    public Achievement A4;
    // Start is called before the first frame update
    void Start()
    {
        A1 = new Achievement("Left the house");
        A2 = new Achievement("Fried Chicken");
        A3 = new Achievement("New Heights");
        A4 = new Achievement("Tiny Disaster");

        if (SceneManager.GetActiveScene().name == "Forest")
        {
            Activate(A1);
        }
    }

    // Update is called once per frame
    public void Activate(Achievement obtained)
    {
        if (!obtained.isUnlocked)
            StartCoroutine(WaitThenDisplay(obtained));
        obtained.isUnlocked = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (player.activeScript.animalName == "Crow")
        {
            print("wow");
            if (other.gameObject.name == "THE FLOOR IS LAVA")
            {
                Activate(A2);
            }
        }
        else
            print(player.activeScript.animalName);
    }

    IEnumerator WaitThenDisplay(Achievement obtained)
    {
        yield return new WaitForSecondsRealtime(2);
        displayAchievement(obtained);
        yield return new WaitForSecondsRealtime(3);
        RemoveAchievement();
    }

    public void displayAchievement(Achievement obtained)
    {
        achievementDisplay.SetActive(true);
        anim.SetTrigger("ShowAchievement");
        TMPText.text = obtained.text;
    }

    public void RemoveAchievement()
    {
        anim.SetTrigger("StopAchievement");
    }
}
