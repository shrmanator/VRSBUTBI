/*
This script is responsible for setting up the Oculus VR tracking system.

It instantiates the OVRCameraRig prefab and OVRPlayerController prefab.

It sets the tracking space to floor level, and sets the camera to the center eye anchor.

If no HMD is detected, it disables the OVRCameraRig and enables a regular camera.
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

/// <summary>
/// This class sets up the Oculus VR tracking system in the Unity scene.
/// </summary>
public class OVRTrackingSetup : MonoBehaviour
{
    /// <summary>
    /// The OVRCameraRig prefab to be instantiated.
    /// </summary>
    public GameObject ovrCameraRigPrefab;

    /// <summary>
    /// The OVRPlayerController prefab to be instantiated.
    /// </summary>
    public GameObject ovrPlayerControllerPrefab;

    /// <summary>
    /// The regular Camera to be used if an HMD is not detected.
    /// </summary>
    public Camera regularCamera;

    /// <summary>
    /// Sets up the Oculus VR tracking system in the Unity scene.
    /// </summary>
    void Start()
    {
        // Check if HMD is connected
        if (OVRPlugin.GetSystemHeadsetType() == OVRPlugin.SystemHeadset.None)
        {
            // Disable OVRCameraRig and enable regular Camera
            ovrCameraRigPrefab.SetActive(false);
            regularCamera.gameObject.SetActive(true);
            return;
        }

        // Instantiate OVRCameraRig prefab
        GameObject ovrCameraRig = Instantiate(ovrCameraRigPrefab);

        // Set tracking space to floor level using OVRPlugin API
        OVRPlugin.SetTrackingOriginType(OVRPlugin.TrackingOrigin.FloorLevel);

        // Set camera to center eye anchor
        Transform centerEyeAnchor = ovrCameraRig.transform.Find("TrackingSpace/CenterEyeAnchor");
        OVRCameraRig ovrCameraRigComponent = ovrCameraRig.GetComponent<OVRCameraRig>();
        PropertyInfo centerEyeAnchorProperty = typeof(OVRCameraRig).GetProperty("centerEyeAnchor", BindingFlags.Instance | BindingFlags.NonPublic);
        centerEyeAnchorProperty.SetValue(ovrCameraRigComponent, centerEyeAnchor);

        // Instantiate OVRPlayerController prefab
        Instantiate(ovrPlayerControllerPrefab, Vector3.zero, Quaternion.identity);
    }
}
