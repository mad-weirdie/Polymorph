using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;
    public CinemachineConfiner cam;
    public GameObject playerCamera;
    private CinemachineFreeLook cameraController;
    public Collider newConfiner;
    public float newClip;

    void Start()
    {
        cameraController = playerCamera.GetComponent<CinemachineFreeLook>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cam.m_BoundingVolume = newConfiner;
            cameraController.m_Lens.NearClipPlane = newClip;
            cameraController.m_XAxis.m_MaxSpeed = 0.0f;
            cameraController.m_YAxis.m_MaxSpeed = 0.0f;
            //camera1.enabled = false;
            camera2.enabled = true;
}
    }
}
