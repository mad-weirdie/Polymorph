using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    public CinemachineConfiner cam;
    public GameObject playerCamera;
    private CinemachineFreeLook cameraController;
    public Collider newConfiner;
    public float newClip;

    void Start()
    {
        cameraController = playerCamera.GetComponent<CinemachineFreeLook>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cam.m_BoundingVolume = newConfiner;
            cameraController.m_Lens.NearClipPlane = newClip;
        }
    }
}
