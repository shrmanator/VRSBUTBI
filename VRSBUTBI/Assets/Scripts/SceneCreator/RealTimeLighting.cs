using System;
using UnityEngine;

public class RealTimeLighting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Make a game object for the light
        GameObject mainLight = new GameObject("MainLight");
        // Add the light component
        Light lightComp = mainLight.AddComponent<Light>();
        
        // Set the position (or any transform property)
        mainLight.transform.position = new Vector3(0, 1000, 0);
        
        // Make a game object for the sun
        GameObject sun = new GameObject("Sun");
        // Add the sprite renderer component
        SpriteRenderer spriteRenderer = sun.AddComponent<SpriteRenderer>();
        // Set the sprite
        spriteRenderer.sprite = Resources.Load<Sprite>("Sun");
        // Set the scale
        sun.transform.localScale = new Vector3(10, 10, 10);
        // Set the position
        sun.transform.position = mainLight.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject mainLight = GameObject.Find("MainLight");
        Light lightComp = mainLight.GetComponent<Light>();
        GameObject sun = GameObject.Find("Sun");
        SpriteRenderer spriteRenderer = sun.GetComponent<SpriteRenderer>();
        
        System.DateTime currentTime = System.DateTime.Now;

        // Calculate time of day as a value between 0 and 1
        float time = (float)currentTime.Hour / 24.0f;

        // Use the time value to set the light color and intensity
        lightComp.color = Color.Lerp(Color.blue, Color.red, time);
        lightComp.intensity = Mathf.Lerp(0.5f, 1.0f, time);

        // Calculate the angle of the sun based on the time of day
        float sunAngle = time * 360.0f - 90.0f;
        mainLight.transform.rotation = Quaternion.Euler(new Vector3(sunAngle, 0, 0));
        sun.transform.rotation = Quaternion.Euler(new Vector3(sunAngle, 0, 0));
    }
}
