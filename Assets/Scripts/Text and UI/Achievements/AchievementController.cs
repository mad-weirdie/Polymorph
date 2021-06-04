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
    Achievement A1;
    Achievement A2;
    Achievement A3;
    Achievement A4;
    // Start is called before the first frame update
    void Start()
    {
        A1 = new Achievement("Left the house");
        A2 = new Achievement("Fried Chicken");
        A3 = new Achievement("New Heights");

        if (SceneManager.GetActiveScene().name == "Forest")
        {
            print("YEA");
            StartCoroutine(WaitThenDisplay(A1));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (player.activeScript.animalName == "Crow")
        {
            print("wow");
            if (other.gameObject.name == "THE FLOOR IS LAVA")
            {
                StartCoroutine(WaitThenDisplay(A2));
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

    public void achievementUnlocked()
    {

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
