using System;
using UnityEngine;

public class SkyComponents : MonoBehaviour
{
    void Start()
    {
    }
}

public class Sun
{
    [SerializeField] Transform sun;
    [SerializeField] float orbitRadius = 50f;
    [SerializeField] float orbitSpeed = 1f;
    [SerializeField] float inclination = 45f;
    [SerializeField] float intensity = 1.0f;
    [SerializeField] Color color = Color.white;

    private Vector3 sunOffset;

    void Start()
    {
        createSun();
        // Get the current time
        DateTime now = DateTime.Now;

        // Calculate the position of the sun based on the time of day
        float hourAngle = (float)(now.Hour * 15 - 90) * Mathf.Deg2Rad;
        float declination = (float)(23.45 * Mathf.Sin(Mathf.Deg2Rad * (360f * (284f + now.DayOfYear) / 365f)));

        // Convert polar coordinates to Cartesian coordinates
        float x = orbitRadius * Mathf.Cos(hourAngle);
        float y = orbitRadius * Mathf.Sin(hourAngle) * Mathf.Sin(inclination * Mathf.Deg2Rad);
        float z = orbitRadius * Mathf.Sin(hourAngle) * Mathf.Cos(inclination * Mathf.Deg2Rad);

        // Set the position of the sun
        sunOffset = new Vector3(x, y, z);
        sun.position = transform.position + sunOffset;

        // Orient the sun towards the center of the scene
        sun.LookAt(transform);
    }
    void Update()
    {
        // Rotate the sun around the center of the scene
        sunOffset = Quaternion.AngleAxis(orbitSpeed * Time.deltaTime, Vector3.up) * sunOffset;
        sun.position = transform.position + sunOffset;
        sun.LookAt(transform);
    }

    public void createSun()
    {
        // Create a new GameObject for the sun
        GameObject sun = new GameObject("Sun");
    
        // Add a light component to the sun
        Light lightComp = sun.AddComponent<Light>();

        // Set the light type to directional
        lightComp.type = LightType.Directional;

        // Set the light color and intensity
        lightComp.color = color;
        lightComp.intensity = intensity;

        // Set the position of the sun in the sky
        sun.transform.rotation = Quaternion.Euler(50.0f, 30.0f, 0.0f);
    }

}
