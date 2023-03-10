using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectManager;


//class for testing the Object Creator
public class ObjectCreatorTester : MonoBehaviour
{ 
    // test object data
    // object[,] objects = new object[,] {{"Cat", "Cat1", 0, 0, 0}, {"Cat", "Cat2", 10, 10, 10},
    // {"Tree", "Tree1", 30, 5, 30}, {"dead_trees", "Dead Trees", 20, 0, 20}};
    object[] anObject1 = new object[] {"Cat2", "Dead Trees", 0.0, 0.0, 0.0};
    object[] anObject2 = new object[] {"Cat", "Cat1", 20.0, 20.0, 20.0};
    // Start is called before the first frame update
    void Start()
    {
        //ObjectCreator.Creator.CreateObjects(objects);
        ObjectManager.Manager.CreateObject(anObject1);
        ObjectManager.Manager.CreateObject(anObject2);
    }
}
