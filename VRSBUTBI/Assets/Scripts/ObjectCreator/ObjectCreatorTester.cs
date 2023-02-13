using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectCreator;

public class ObjectCreatorTester : MonoBehaviour
{ 
    // example list of object data
    string[,] objects = new string[,] {{"ObjectType", "ObjectName1", "0 0 0"}, {"ObjectType", "ObjectName2", "10 10 10"}};
    // Start is called before the first frame update
    void Start()
    {
        ObjCreator creator = gameObject.AddComponent<ObjCreator>();
        creator.CreateObjects(objects);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
