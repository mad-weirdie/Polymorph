using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barricade : MonoBehaviour
{
    private int ActivatedCrystals;
    public int RequiredCrystals;
    private bool isMoved;
    private Vector3 vec_position;

    private void Start()
    {
        vec_position = new Vector3();
        vec_position = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ActivatedCrystals == RequiredCrystals && !isMoved)
            MoveWall();
    }

    public void AddCrystal()
    {
        ActivatedCrystals++;
    }
    

    private void MoveWall()
    {
        vec_position.y += 0.01f;
        transform.position = vec_position;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Stop")
            isMoved = true;
    }
}
