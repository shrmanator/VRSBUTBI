using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Testing class for path features
public class PathTester : MonoBehaviour
{
    // example data of a PATH command
    // assigns Cube to Path1
    object[] pathCommand = {"PATH", "Cube", "Path1"};
    // example data of a MOVE command
    // moves Cube for 20 seconds
    object[] moveCommand = {"MOVE", "Cube", 20};
    // Start is called before the first frame update
    void Start()
    {
        // create cube
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = "Cube";
        // execute path command
        PathManager.Manager.AssignPath(pathCommand);
        // execute move command
        PathManager.Manager.AssignMovement(moveCommand);
    }
}
