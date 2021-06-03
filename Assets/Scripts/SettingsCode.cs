using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SettingsCode : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerController player;

    private void Start()
    {
        player = GetComponent<PlayerController>();
    }

    public void CameraSensitivityChanged(float S) {

        print(S);
        player.cam.m_XAxis.m_MaxSpeed = Mathf.Clamp(100f * S, 50, 400);
    }


}
