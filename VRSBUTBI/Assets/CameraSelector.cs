using UnityEngine;
using UnityEngine.XR;

public class CameraSelector : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject OVRCameraRig;
    public GameObject OculusDetectedPrompt; // A GameObject containing the prompt UI

    void Start()
    {
        if (XRSettings.enabled && XRSettings.isDeviceActive && XRSettings.loadedDeviceName.Contains("Oculus"))
        {
            // Oculus VR device is active
            mainCamera.enabled = false;
            OVRCameraRig.SetActive(true);
            OculusDetectedPrompt.SetActive(true); // Show the prompt
        }
        else
        {
            // No Oculus VR device is active
            mainCamera.enabled = true;
            OVRCameraRig.SetActive(false);
            OculusDetectedPrompt.SetActive(false); // Hide the prompt
        }
    }
}
