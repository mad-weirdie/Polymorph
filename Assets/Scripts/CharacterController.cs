using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private GameObject activePlayer;
    public List<GameObject> characters;

    // Start is called before the first frame update
    void Start()
    {
        // Set all other game objects to not active.
        foreach (GameObject character in characters)
        {
            character.SetActive(false);
        }

        // Start as tortoise
        characters[0].SetActive(true);
        activePlayer = characters[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnShapeShift()
    {
        // Swap between characters.
        Transform pos = activePlayer.transform;
        activePlayer.SetActive(false);

        if (activePlayer.name == characters[0].name)
        {
            activePlayer = characters[1];
        }
        else
        {
            activePlayer = characters[0];
        }

        activePlayer.SetActive(true);
    }

    private void OnMove()
    {
        
    }
}
