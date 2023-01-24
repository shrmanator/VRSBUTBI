/*
Property of VRSBUTBI

This script implements the
keyboard camera controls
*/


using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float panSpeed = 20f;
    [SerializeField] float zoomSpeed = 50f;
    [SerializeField] float mouseSensitivity = 100.0f;

    [SerializeField] float zoomMin = 10f;
    [SerializeField] float zoomMax = 80f;

    void Update()
    {
        Vector3 cameraPosition = transform.position;

        // ====== PANNING THE CAMERA ======
        //
        if (Input.GetKey("w") || Input.GetKey("up"))
            cameraPosition += transform.forward * panSpeed * Time.deltaTime;

        if (Input.GetKey("s") || Input.GetKey("down"))
            cameraPosition -= transform.forward * panSpeed * Time.deltaTime;

        if (Input.GetKey("d") || Input.GetKey("right"))
            cameraPosition += transform.right * panSpeed * Time.deltaTime;

        if (Input.GetKey("a") || Input.GetKey("left"))
            cameraPosition -= transform.right * panSpeed * Time.deltaTime;


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
            transform.Rotate(-mouseY * mouseSensitivity * Time.deltaTime, mouseX * mouseSensitivity * Time.deltaTime, 0);

            // stop camera from rotating too far around its axis
            // float x = transform.localEulerAngles.x;
            // x = Mathf.Clamp(x, minX, maxX);
            // transform.localEulerAngles = new Vector3(x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }

        transform.position = cameraPosition;
    }
}
