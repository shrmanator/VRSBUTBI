using UnityEngine;

/// <summary>
/// Property of VRSBUTBI. 
/// This script implements the keyboard camera controls.
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] 
    /// <summary>
    /// Speed at which camera pans horizontally or vertically.
    /// </summary>
    float panSpeed = 20f;
    [SerializeField] 
    /// <summary>
    /// Speed at which camera zooms in or out.
    /// </summary>
    float zoomSpeed = 50f;
    [SerializeField] 
    /// <summary>
    /// Sensitivity of the mouse movement for rotation.
    /// </summary>
    float mouseSensitivity = 100.0f;
    [SerializeField] 
    /// <summary>
    /// Minimum limit of the zoom level.
    /// </summary>
    float zoomMin = 2f;
    [SerializeField] 
    /// <summary>
    /// Maximum limit of the zoom level.
    /// </summary>
    float zoomMax = 10f;

    private void Update()
    {
        Vector3 cameraPosition = transform.position;

        // ====== HORIZONTAL CAMERA MOVEMENT ======
        //
        if (Input.GetKey("w") || Input.GetKey("up"))
            cameraPosition += transform.forward * panSpeed * Time.deltaTime;
        if (Input.GetKey("s") || Input.GetKey("down"))
            cameraPosition -= transform.forward * panSpeed * Time.deltaTime;
        if (Input.GetKey("d") || Input.GetKey("right"))
            cameraPosition += transform.right * panSpeed * Time.deltaTime;
        if (Input.GetKey("a") || Input.GetKey("left"))
            cameraPosition -= transform.right * panSpeed * Time.deltaTime;

        // ====== VERTICAL CAMERA MOVEMENT ======
        //
        if (Input.GetKey("e") || Input.GetKey("/"))
            // vertical up movement
            cameraPosition += transform.up * panSpeed * Time.deltaTime;
        if (Input.GetKey("f") || Input.GetKey(KeyCode.RightAlt))
            // vertical down movement
            cameraPosition -= transform.up * panSpeed * Time.deltaTime;

        // ====== ZOOMING IN AND OUT ======
        //
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cameraPosition.y -= scroll * zoomSpeed * 100f * Time.deltaTime;
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, zoomMin, zoomMax);

        // ====== ROTATING THE CAMERA ======
        //
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            // Rotation on the x-axis based on mouse movement in the vertical direction
            float xRotation = transform.localEulerAngles.x - mouseY * mouseSensitivity * Time.deltaTime;
            // Rotation on the y-axis based on mouse movement in the horizontal direction
            float yRotation = transform.localEulerAngles.y
         }
      }
  }
