/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameEndingScript : MonoBehaviour
{
    public PlayerController player;
    public GameObject text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player.activePlayer)
        {
            text.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player.activePlayer)
        {
            text.SetActive(false);
        }
    }


}*/
