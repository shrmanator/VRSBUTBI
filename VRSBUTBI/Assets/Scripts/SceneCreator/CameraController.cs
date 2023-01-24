using UnityEngine;

public class RTSCamera : MonoBehaviour
{
    [SerializeField] float panSpeed = 20f;
    [SerializeField] float zoomSpeed = 50f;
    [SerializeField] float rotateSpeed = 100f;

    [SerializeField] float zoomMin = 10f;
    [SerializeField] float zoomMax = 80f;

    [SerializeField] float panBorderThickness = 10f;

    void Update()
    {
        Vector3 pos = transform.position;

        // Panning
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        // Zooming
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * zoomSpeed * 100f * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, zoomMin, zoomMax);

        // Rotation
        if (Input.GetMouseButton(1))
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime);
        }

        transform.position = pos;
    }
}
