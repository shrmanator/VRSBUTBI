using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Dummiesman;

// creates objects from provided data
// object data expected to be a string array with the format {Object Type, Object Name, X Y Z}
namespace ObjectCreator {
public class ObjCreator : MonoBehaviour
{
    Dictionary<string, string> importLibrary = new Dictionary<string, string>();
    string objPath = string.Empty;
    string error = string.Empty;
    //file path for testing. Remove when dialogue prompt is implemented
    public string filePath = string.Empty;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //creates objects from a list of objects
    public void CreateObjects(string [,] objectsList)
    {
        for (int i = 0; i < objectsList.GetLength(0); i++)
        {
            string[] obj = new string[] {objectsList[i, 0], objectsList[i, 1], objectsList[i, 2]};
            CreateObject(obj);
        }
    }

    //creates a single object
    public void CreateObject(string [] obj)
    {
        GameObject loadedObject;
        // Check if object type has been imported before
        if (IsNewType(obj[0]) == true)
        {
            Debug.Log(obj[0] + " found!");
            loadedObject = new OBJLoader().Load(importLibrary[obj[0]]);
        }
        //if object type is not found, prompt user to select a file
        //to-do: update to implement file selection
        else{
            Debug.Log(obj[0] + " not found");
            loadedObject = new OBJLoader().Load(filePath);
            importLibrary.Add(obj[0], filePath);
        }
        //set object properties

        //set name
        loadedObject.name = obj[1];
        //set position
        string[] coords = obj[2].Split(' ');
        loadedObject.transform.position = new Vector3(int.Parse(coords[0]), int.Parse(coords[1]), int.Parse(coords[2]));

    }

    //function to check if object type has been seen before
    bool IsNewType(string objType)
    {
        return importLibrary.ContainsKey(objType);
    }

    //to delete once file select dialogue is implemented
    public void SetString(string str)
    {
        filePath += str;
    }
}
}