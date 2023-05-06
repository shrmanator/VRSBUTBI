using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObjectTester : MonoBehaviour
{
    object[] transformCommand = {"DYNUPDATECELL", "Cube", "TRANSFORM", 20, 10, 10, 10};
    object[] transformCommand1 = {"DYNUPDATECELL", "Cube", "TRANSFORM", 30, 5, 30, 10 };
    object[] rotateCommand = { "DYNUPDATECELL", "Cube", "ROTATE", 20, 180, 0, 0 };
    object[] rotateCommand1 = { "DYNUPDATECELL", "Cube", "ROTATE", 30, 180, 180, 0 };
    // Start is called before the first frame update
    void Start()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(10, 10, 10);
        cube.transform.localScale = new Vector3(1, 1, 1);
        cube.name = "Cube";
        ObjectManager.Manager.DynamicallyChangeObjectProperty(transformCommand);
        ObjectManager.Manager.DynamicallyChangeObjectProperty(transformCommand1);
        ObjectManager.Manager.DynamicallyChangeObjectProperty(rotateCommand);
        ObjectManager.Manager.DynamicallyChangeObjectProperty(rotateCommand1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
