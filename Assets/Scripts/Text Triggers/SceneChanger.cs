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
    private Vector3 newScenePosition;
    private Quaternion newSceneRotation;

    public SceneSwitchNotifier(string s, PlayerController p, Vector3 pos, Quaternion rot)
    {
        triggerActive = false;
        nextScene = s;
        playerController = p;
        listenerType = "Grab";
        newScenePosition = pos;
        newSceneRotation = rot;
    }

    public void setActive(bool state)
    {
        triggerActive = state;
    }

    public override void Notify()
    {
        if (triggerActive)
        {
            PersistentData.spawnPoint = newScenePosition;
            PersistentData.spawnRotation = newSceneRotation;
            SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        }
    }
}

public class SceneChanger : HelpfulText
{
    public Vector3 newSpawnPosition;
    public Quaternion newSpawnRotation;
    bool insideTrigger = false;
    public string nextScene;
    SceneSwitchNotifier sceneNotif;

    void Start()
    {
        // This receives input from our player, mainly clicking
        sceneNotif = new SceneSwitchNotifier(nextScene, player, newSpawnPosition, newSpawnRotation);
        player.GetComponent<PlayerController>().AddListener(sceneNotif);
    }

    override public bool conditionsMet()
    {
        print("inside trigger: " + insideTrigger);
        return insideTrigger;
    }

    override public void Remove()
    {
        return;
    }

    void OnTriggerEnter(Collider sceneTrigger)
    {
        if (sceneTrigger.gameObject.CompareTag("Player") || sceneTrigger.gameObject.CompareTag("Animal"))
        {
            print("wowow");
            insideTrigger = true;
            sceneNotif.setActive(true);
        }   
    }

    void OnTriggerExit(Collider sceneTrigger)
    {
        if (sceneTrigger.gameObject.CompareTag("Player") || sceneTrigger.gameObject.CompareTag("Animal"))
        {
            insideTrigger = false;
            sceneNotif.setActive(false);
        }
    }
}
