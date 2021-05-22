using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


// Lava level listener - gets input from player
public class LavaNotifier : Listener
{
    public GameObject deathScreen;
    public LavaNotifier(GameObject d, PlayerController p)
    {
        deathScreen = d;
        playerController = p;
    }

    public override void Notify()
    {
       // print("let me out!");
        deathScreen.SetActive(false);
        playerController.UnpauseGame();
    }
}

public class Lava : MonoBehaviour
{
    public GameObject player;
    private Vector3 startPosition;
    private float startDrag;
    private Quaternion startRotation;
    
    PlayerController playerScript;
    public GameObject deathScreen;

    // Lava go tsssss
    public AudioSource lavaImpact;
    public AudioSource lavaSizzle;

    // Are we in lava right now?
    private bool isSinking;
    // Time before we stop doing antigravity math
    // and just reset the game
    public float sinkingTime = 1.5f;
    // This impacts how "dense" the lava will feel
    // by adding this much drag to the player
    float lavaDensity = 20.0f;

    void Start()
    {
        // Remember player's starting position/rotation/"gravity" for level reset
        // I've tried using DontDestroyOnLoad and scene changes to reset,
        // but it makes a huge mess trying to preserve old variable data.
        startPosition = player.transform.position;
        startRotation = player.transform.rotation;
        startDrag = player.GetComponent<Rigidbody>().drag;

        playerScript = player.GetComponent<PlayerController>();

        // This receives input from our player, mainly clicking
        LavaNotifier lava = new LavaNotifier(deathScreen, playerScript);
 
        player.GetComponent<PlayerController>().AddListener(lava);
        deathScreen.SetActive(false);
        isSinking = false;
    }

    void Update()
    {
        // Reduce gravity on the player to make it look like the lava is dense
        if (isSinking)
            player.GetComponent<Rigidbody>().drag = lavaDensity;
    }

    void OnTriggerEnter(Collider other)
    {
        // Trying to leave room for effects like having other objects sink too!
        if (other.gameObject.CompareTag("Player"))
        {
            lavaImpact.Play();
            lavaSizzle.Play();
            isSinking = true;
            StartCoroutine("FinishSinking");
        }
    }

    // Wait before resetting the level
    IEnumerator FinishSinking()
    {
        yield return new WaitForSeconds(sinkingTime);
        isSinking = false;
        ResetLevel();
    }

    // Reset the level
    void ResetLevel()
    {
        deathScreen.SetActive(true);
        player.transform.position = startPosition;
        player.transform.rotation = startRotation;
        player.GetComponent<Rigidbody>().drag = startDrag;
        playerScript.PauseGame();
    }
}