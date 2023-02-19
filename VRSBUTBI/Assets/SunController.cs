using System;
using UnityEngine;

/// <summary>
/// Controls the sun and sky components of the scene.
/// </summary>
public class SunController : MonoBehaviour
{
    [SerializeField] Transform sunTransform;
    [SerializeField] float orbitRadius = 50f;
    [SerializeField] float orbitSpeed = 1f;
    [SerializeField] float inclination = 45f;
    [SerializeField] float intensity = 1.0f;
    [SerializeField] Color color = Color.white;

    private Vector3 sunOffset;

    /// <summary>
    /// Initialize the sun and sky components.
    /// </summary>
    void Start()
    {
        /// Create a new game object for the sun, adds a light component, and sets its properties.
        GameObject sunObject = new GameObject("Sun");
        Light lightComponent = sunObject.AddComponent<Light>();
        lightComponent.type = LightType.Directional;
        lightComponent.color = color;
        lightComponent.intensity = intensity;
        sunObject.transform.rotation = Quaternion.Euler(50.0f, 30.0f, 0.0f);
        
        /// Calculate the position of the sun based on the time of day and sets its position and orientation.
        DateTime now = DateTime.Now;
        float hourAngle = (float)(now.Hour * 15 - 90) * Mathf.Deg2Rad;
        float declination = (float)(23.45 * Mathf.Sin(Mathf.Deg2Rad * (360f * (284f + now.DayOfYear) / 365f)));
        float x = orbitRadius * Mathf.Cos(hourAngle);
        float y = orbitRadius * Mathf.Sin(hourAngle) * Mathf.Sin(inclination * Mathf.Deg2Rad);
        float z = orbitRadius * Mathf.Sin(hourAngle) * Mathf.Cos(inclination * Mathf.Deg2Rad);
        sunOffset = new Vector3(x, y, z);
        sunTransform.position = transform.position + sunOffset;
        sunTransform.LookAt(transform);
    }
    
    /// <summary>
    /// Rotate the sun around the center of the scene.
    /// </summary>
    void Update()
    {
        sunOffset = Quaternion.AngleAxis(orbitSpeed * Time.deltaTime, Vector3.up) * sunOffset;
        sunTransform.position = transform.position + sunOffset;
        sunTransform.LookAt(transform);
    }
}
