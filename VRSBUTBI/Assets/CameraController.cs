using UnityEngine;

/// <summary>
/// Keyboard camera controls implementation.
/// Property of VRSBUTBI.
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] float panSpeed = 20f; // speed of horizontal camera movement
    [SerializeField] float zoomSpeed = 50f; // speed of zooming in and out
    [SerializeField] float mouseSensitivity = 100.0f; // sensitivity of mouse movement for rotation
    [SerializeField] float zoomMin = 2f; // minimum distance from the ground for zooming
    [SerializeField] float zoomMax = 10f; // maximum distance from the ground for zooming

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
            // rotation on the x-axis based on mouse movement in the vertical direction
            float xRotation = transform.localEulerAngles.x - mouseY * mouseSensitivity
        }
    }
}
