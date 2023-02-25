using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectCreator;

public class ObjectCreatorTester : MonoBehaviour
{ 
    // example list of object data
    string[,] objects = new string[,] {{"Cat", "Cat1", "0 0 0"}, {"Cat", "Cat2", "10 10 10"},
    {"Tree", "Tree1", "30 5 30"}, {"dead_trees", "Dead Trees", "20, 0, 20"}};
    //string[] anObject = new string[] {"dead_trees", "Dead Trees", "0 0 0"};
    //string[] anObject = new string[] {"Cat", "Cat1", "0 0 0"};
    // Start is called before the first frame update
    void Start()
    {
        ObjectCreator.Creator.CreateObjects(objects);
        //ObjectCreator.Creator.CreateObject(anObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
