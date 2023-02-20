/// Property of VRSBUTBI.

using System;
using UnityEngine;

public class RealTimeLighting : MonoBehaviour
{
    /// <summary>
    /// Creates a new directional light and sets its position and type
    /// </summary>
    void Start()
    {
        // Make a game object for the light
        GameObject mainLight = new GameObject("MainLight");
        // Add the light component
        Light lightComp = mainLight.AddComponent<Light>();
        
        // Set the position (or any transform property)
        mainLight.transform.position = new Vector3(0, 1000, 0);
        // Set the light type to directional
        lightComp.type = LightType.Directional;
    }

    /// <summary>
    /// Updates the color and intensity of the main light based on the current time of day
    /// </summary>
    void Update()
    {
        GameObject mainLight = GameObject.Find("MainLight");
        Light lightComp = mainLight.GetComponent<Light>();
    
        System.DateTime currentTime = System.DateTime.Now;

        // Calculate time of day as a value between 0 and 1
        float time = (float)currentTime.Hour / 24.0f;

        // Use the time value to set the light color and intensity
        lightComp.color = Color.Lerp(Color.blue, Color.red, time);
        lightComp.intensity = Mathf.Lerp(0.5f, 1.0f, time);

    }
}
