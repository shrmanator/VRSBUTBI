using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectCreator;

public class ObjectCreatorTester : MonoBehaviour
{
    ObjCreator creator;
    //hard coded file path for testing. Remove when dialogue prompt is implemented
    //remember backslashes need to be escaped
    string filePath = string.Empty; 
    // example list of object data
    string[,] objects = new string[,] {{"ObjectType", "ObjectName1", "0 0 0"}, {"ObjectType", "ObjectName2", "5 5 5"}};
    // Start is called before the first frame update
    void Start()
    {
        if (string.IsNullOrEmpty(filePath) == false)
        {
            creator.SetString(filePath);
            creator.CreateObjects(objects);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
