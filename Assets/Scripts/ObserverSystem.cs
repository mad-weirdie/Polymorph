using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void StateChanged(bool state);
public class ObserverSystem : MonoBehaviour
{
    public List<ObserverSystem> listeners;
    public event StateChanged statechanged;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < listeners.Count; i++) {

            listeners[i].statechanged += SetState;

        }
    }

    void SetState(bool state) {

        if (state == true) { print("PING"); }
        else { print("PONG"); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        statechanged(true);
    }

    private void OnTriggerExit(Collider other)
    {
        statechanged(false);
    }


}
