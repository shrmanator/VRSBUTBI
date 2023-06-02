/// Property of VRSBUTBI. 

using UnityEngine;


/// <summary>
/// This script implements the keyboard camera controls for the regular non-VR camera.
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] 
    /// <summary>
    /// Speed at which camera pans horizontally or vertically.
    /// </summary>
    float panSpeed = 450f;
    [SerializeField] 
    /// <summary>
    /// Speed at which camera zooms in or out.
    /// </summary>
    float zoomSpeed = 750f;
    [SerializeField] 
    /// <summary>
    /// Sensitivity of the mouse movement for rotation.
    /// </summary>
    float mouseSensitivity = 4000f;
    [SerializeField] 
    /// <summary>
    /// Minimum limit of vertical movement.
    /// </summary>
    float verticalMin = 13f;
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

    bool topDownView = false;

    void Update()
    {
        Vector3 cameraPosition = transform.position;

        // ====== HORIZONTAL CAMERA MOVEMENT ======
        //
        // Move the camera in the direction it's facing on the xz-plane.
        if (Input.GetKey(fwdCameraKey))
        {
            Vector3 forward = transform.forward;
            forward.y = 0;  // ignore vertical direction
            forward.Normalize();  // make sure the speed is consistent
            cameraPosition += forward * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(backwardCameraKey))
        {
            Vector3 backward = -transform.forward;
            backward.y = 0;  // ignore vertical direction
            backward.Normalize();  // make sure the speed is consistent
            cameraPosition += backward * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(rightCameraStrafeKey))
        {
            Vector3 right = transform.right;
            right.y = 0;  // ignore vertical direction
            right.Normalize();  // make sure the speed is consistent
            cameraPosition += right * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(leftCameraStrafeKey))
        {
            Vector3 left = -transform.right;
            left.y = 0;  // ignore vertical direction
            left.Normalize();  // make sure the speed is consistent
            cameraPosition += left * panSpeed * Time.deltaTime;
        }

        // ====== VERTICAL CAMERA MOVEMENT ======
        //
        if (Input.GetKey(verticalUpCameraKey))
        {
            cameraPosition += Vector3.up * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(verticalDownCameraKey))
        {
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

        
    /*
    This method will be used alongside the path creator scripts.
    When the user wants to create a path for an object, switch to top-down view.
    */
    // public void SwitchToTopDownView()
    // {
    //     // Get the center of your scene. This depends on your scene layout.
    //     Vector3 sceneCenter = new Vector3(0, 0, 0);

    //     // Position the camera above the center of the scene.
    //     // You may need to adjust the Y value based on the size of your scene.
    //     transform.position = sceneCenter + new Vector3(194, 453, 250);

    //     // Point the camera straight down.
    //     transform.rotation = Quaternion.Euler(90, 0, 0);

    //     // Set topDownView to true
    //     topDownView = true;
    // }
}
