/// Property of VRSBUTBI.

using System;
using UnityEngine;

/// <summary>
/// Controls the sun and its properties in the scene.
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
    /// Initialize the sun's position and components.
    /// </summary>
    private void Start()
    {
        GameObject sunObject = new GameObject("Sun");  /// Create a new game object for the sun
        Light lightComponent = sunObject.AddComponent<Light>();  /// Add a Light component to the sun

        /// Set the sun's properties
        lightComponent.type = LightType.Directional;
        lightComponent.color = color;
        lightComponent.intensity = intensity;
        sunObject.transform.rotation = Quaternion.Euler(50.0f, 30.0f, 0.0f);

        /// Set the sun's initial position in the sky
        SetSunInitialPosition();
    }
    
    /// <summary>
    /// Calculate the initial position of the sun based on the time of day
    /// and set its position and orientation.
    /// </summary>
    private void SetSunInitialPosition()
    {
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
    private void Update()
    {
        sunOffset = Quaternion.AngleAxis(orbitSpeed * Time.deltaTime, Vector3.up) * sunOffset;
        sunTransform.position = transform.position + sunOffset;
        sunTransform.LookAt(transform);
    }
}
