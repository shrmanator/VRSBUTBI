using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectCreator;

public class ObjectCreatorTester : MonoBehaviour
{ 
    // example list of object data
    string[,] objects = new string[,] {{"Cat", "Cat1", "0 0 0"}, {"Cat", "Cat2", "10 10 10"},
    {"Tree", "Tree1", "30 5 30"}};
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
