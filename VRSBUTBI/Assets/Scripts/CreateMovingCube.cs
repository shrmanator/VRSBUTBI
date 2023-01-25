using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMovingCube : MonoBehaviour
{
    private float movementSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        // Spawn a cube
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // Attach MoveObject script to cube
        MoveObject script = cube.AddComponent<MoveObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
