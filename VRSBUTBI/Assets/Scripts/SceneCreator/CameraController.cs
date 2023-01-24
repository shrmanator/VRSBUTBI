using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float panSpeed = 20f;
    [SerializeField] float zoomSpeed = 50f;
    [SerializeField] float rotateSpeed = 100f;

    [SerializeField] float zoomMin = 10f;
    [SerializeField] float zoomMax = 80f;

    void Update()
    {
        Vector3 pos = transform.position;


        // ====== PANNING THE CAMERA ======
        //
        if (Input.GetKey("w") || Input.GetKey("up"))
            pos.z += panSpeed * Time.deltaTime;

        if (Input.GetKey("s") || Input.GetKey("down"))
            pos.z -= panSpeed * Time.deltaTime;

        if (Input.GetKey("d") || Input.GetKey("right"))
            pos.x += panSpeed * Time.deltaTime;

        if (Input.GetKey("a") || Input.GetKey("left"))
            pos.x -= panSpeed * Time.deltaTime;


        // ====== ZOOMING IN AND OUT ======
        //
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * zoomSpeed * 100f * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, zoomMin, zoomMax);


        // ====== ROTATING THE CAMERA ======
        //
        if (Input.GetMouseButton(1))
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime);
        }

        transform.position = pos;
    }
}
