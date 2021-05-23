using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


// Mine entrance listener - gets input from player
public class SceneSwitchNotifier : Listener
{
    private string nextScene;
    private bool triggerActive;

    public SceneSwitchNotifier(string s, PlayerController p)
    {
        triggerActive = false;
        nextScene = s;
        playerController = p;
        listenerType = "Grab";
    }

    public void setActive(bool state)
    {
        triggerActive = state;
    }

    public override void Notify()
    {
        if (triggerActive)
        {
            SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        }
    }
}

public class MineEntrance : HelpfulText
{
    bool insideTrigger = false;
    public string nextScene;
    SceneSwitchNotifier sceneNotif;

    void Start()
    {
        // This receives input from our player, mainly clicking
        sceneNotif = new SceneSwitchNotifier(nextScene, player);
        player.GetComponent<PlayerController>().AddListener(sceneNotif);
    }

    override public bool conditionsMet()
    {
        return insideTrigger;
    }

    override public void Remove()
    {
        return;
    }

    void OnTriggerEnter()
    {
        insideTrigger = true;
        sceneNotif.setActive(true);
    }

    void OnTriggerExit()
    {
        insideTrigger = false;
        sceneNotif.setActive(false);

    }
}
