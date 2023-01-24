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
        // TODO:
        // 1. camera should move flat and never on an angle 

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

            // TODO; 
            // 1. fix bug where camera rotates and tilts

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            Quaternion rotation = Quaternion.Euler(transform.localEulerAngles.x - mouseY * mouseSensitivity * Time.deltaTime, transform.localEulerAngles.y + mouseX * mouseSensitivity * Time.deltaTime, 0);
            transform.localRotation = rotation;
        }

        transform.position = cameraPosition;
    }
}
