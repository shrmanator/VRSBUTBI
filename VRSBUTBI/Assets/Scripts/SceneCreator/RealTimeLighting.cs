using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: add ability to override the real time lighting

public class RealTimeLighting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Make a game object
        GameObject mainLight = new GameObject("MainLight");
        // Add the light component
        Light lightComp = mainLight.AddComponent<Light>();

        // Set the position (or any transform property)
        mainLight.transform.position = new Vector3(0, 5, 0);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject mainLight = GameObject.Find("MainLight");
        Light lightComp = mainLight.GetComponent<Light>();
        lightComp.intensity = 1.0f;
        
        System.DateTime currentTime = System.DateTime.Now;

        if (currentTime.Hour >= 6 && currentTime.Hour < 18) 
        {
            lightComp.intensity = 1.0f;
        } else 
        {
            lightComp.intensity = 0.5f;
        }

    }
}
