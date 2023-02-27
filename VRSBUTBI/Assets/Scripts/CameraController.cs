/// Property of VRSBUTBI. 

using UnityEngine;


/// <summary>
/// This script implements the keyboard camera controls.
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] 
    /// <summary>
    /// Speed at which camera pans horizontally or vertically.
    /// </summary>
    float panSpeed = 50f;
    [SerializeField] 
    /// <summary>
    /// Speed at which camera zooms in or out.
    /// </summary>
    float zoomSpeed = 50f;
    [SerializeField] 
    /// <summary>
    /// Sensitivity of the mouse movement for rotation.
    /// </summary>
    float mouseSensitivity = 800.0f;
    [SerializeField] 
    /// <summary>
    /// Minimum limit of vertical movement.
    /// </summary>
    float verticalMin = 2f;
    [SerializeField] 
    /// <summary>
    /// Maximum limit of vertical movement.
    /// </summary>
    float verticalMax = 100f;
    [SerializeField]
    /// <summary>
    /// Moves the camera forwards.
    /// </summary>
    KeyCode fwdCameraKey;
    [SerializeField]
     /// <summary>
    ///  Moves the camera backwards.
    /// </summary>
    KeyCode backwardCameraKey;
    [SerializeField]
    /// <summary>
    /// Strafes the camera to the left
    /// </summary>
    KeyCode leftCameraStrafeKey;
    [SerializeField]
    /// <summary>
    ///  Strafes the camera to the right
    /// </summary>
    KeyCode rightCameraStrafeKey;
    [SerializeField]
    /// <summary>
    /// Moves the camera up vertically.
    /// </summary>
    KeyCode verticalUpCameraKey;
    [SerializeField]
    /// <summary>
    ///  Moves the camera down vertically.
    /// </summary>
    KeyCode verticalDownCameraKey;
    //TODO: add inverse mouse rotate control
    [SerializeField]
    /// <summary>
    ///  Moves the camera down vertically.
    /// </summary>
    bool invertCameraRotation;

    /// <summary>
    /// The camera's current X rotation.
    /// </summary>
    float currentXRotation;
    /// <summary>
    ///  The camera's current Y rotation.
    /// </summary>
    float currentYRotation;


    void Update()
    {
        Vector3 cameraPosition = transform.position;

        // ====== HORIZONTAL CAMERA MOVEMENT ======
        //
        if (Input.GetKey(fwdCameraKey))
        {
            // move forward in respect to x (increase x)
            cameraPosition += transform.forward * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(backwardCameraKey))
        {
            // move backward in respect to x (decrease x)
            cameraPosition -= transform.forward * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(rightCameraStrafeKey))
        {
            // move right in respect to z (increase z)
            cameraPosition += transform.right * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(leftCameraStrafeKey))
        {
            // move left in respect to z (decrease z)
            cameraPosition -= transform.right * panSpeed * Time.deltaTime;
        }

        // ====== VERTICAL CAMERA MOVEMENT ======
        //
        if (Input.GetKey(verticalUpCameraKey))
        {
            // move up in respect to y (increase y)
            cameraPosition += Vector3.up * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(verticalDownCameraKey))
        {
            // move down in respect to y (decrease y)
            cameraPosition -= Vector3.up * panSpeed * Time.deltaTime;
        }

        // ====== ZOOMING IN AND OUT ======
        //
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cameraPosition.y -= scroll * zoomSpeed * 100f * Time.deltaTime;
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, verticalMin, verticalMax);

        // ====== ROTATING THE CAMERA ======
        //
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");  // horizontal mouse movement axis value 
            float mouseY = Input.GetAxis("Mouse Y");  // vertical mouse movement axis value

            if (invertCameraRotation)
            {
                // calculate the inverted (x,y)-axis rotation
                currentXRotation = transform.localEulerAngles.x + mouseY * mouseSensitivity * Time.deltaTime;
                currentYRotation = transform.localEulerAngles.y - mouseX * mouseSensitivity * Time.deltaTime;
            }
            else
            {        
                // calculate the normal (x,y)-axis rotation        
                currentXRotation = transform.localEulerAngles.x - mouseY * mouseSensitivity * Time.deltaTime;
                currentYRotation = transform.localEulerAngles.y + mouseX * mouseSensitivity * Time.deltaTime;
            }
            // set the camera's rotation equal to the current camera rotation
            transform.localRotation = Quaternion.Euler(currentXRotation, currentYRotation, 0);
        }
        // set the camera's transform position equal to the updated camera position
        transform.position = cameraPosition;
    }
}
