using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Pedestals : MonoBehaviour
{
    //As the other Pedestals, but slightly different!
    private IGenericBehavior logicObject;

    public GameObject to_toggle;

    // Update is called once per frame

    private void Start()
    {
        logicObject = to_toggle.GetComponent<IGenericBehavior>();
        if (logicObject == null) {
            print("NO GENERICBEHAVIOR ON OBJECT< AAAAA");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Puzzle_Crystals")
        {
            logicObject.SetState(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Puzzle_Crystals")
        {
            logicObject.SetState(false);
        }
    }
}
