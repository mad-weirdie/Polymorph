using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [SerializeField]
    public List<resettableObject> resettables;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            PlayerController pc = other.gameObject.GetComponentInParent<PlayerController>();
            if (pc == null) {
                print("WHAT");
                return;
            }
            pc.current_puzzle = this;
        
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") {
            PlayerController pc = other.gameObject.GetComponentInParent<PlayerController>();
            if (pc == null)
            {
                print("WHAT");
                return;
            }
            pc.current_puzzle = null;

        }
    }

    public void ResetPuzzle() {
        print("puzzles resetting...");
        for (int i = 0; i < resettables.Count; i++) {
            resettables[i].reset();
        
        }
    }

}
