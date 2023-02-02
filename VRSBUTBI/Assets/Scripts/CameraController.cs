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


    void Update()
    {
        Vector3 cameraPosition = transform.position;

        // ====== HORIZONTAL CAMERA MOVEMENT ======
        //
        if (Input.GetKey("w") || Input.GetKey("up"))
        {
            // move forward in respect to x (increase x)
            cameraPosition += transform.forward * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.GetKey("down"))
        {
            // move backward in respect to x (decrease x)
            cameraPosition -= transform.forward * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            // move right in respect to z (increase z)
            cameraPosition += transform.right * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            // move left in respect to z (decrease z)
            cameraPosition -= transform.right * panSpeed * Time.deltaTime;
        }

        // ====== VERTICAL CAMERA MOVEMENT ======
        //
        if (Input.GetKey("e") || Input.GetKey("/"))
        {
            // move up in respect to y (increase y)
            cameraPosition += transform.up * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("f") || Input.GetKey(KeyCode.RightAlt))
        {
            // move down in respect to y (decrease y)
            cameraPosition -= transform.up * panSpeed * Time.deltaTime;
        }

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

            float xRotation = transform.localEulerAngles.x + mouseY * mouseSensitivity * Time.deltaTime;
            float yRotation = transform.localEulerAngles.y + mouseX * mouseSensitivity * Time.deltaTime;

            Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);

            // set the camera's rotation equal to the updated camera rotation
            transform.localRotation = rotation;
        }

        // set the camera's transform position equal to the updated camera position
        transform.position = cameraPosition;
    }
}
