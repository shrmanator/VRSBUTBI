using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Dummiesman;
using SimpleFileBrowser;

// creates objects from provided data
// object data expected to be a string array with the format {Object Type, Object Name, X Y Z}
namespace ObjectCreator {
public sealed class ObjCreator : MonoBehaviour
{
   // Dictionary<string, string> importLibrary = new Dictionary<string, string>();
    string objPath = string.Empty;
    string error = string.Empty;
    //file path for testing. Remove when dialogue prompt is implemented
    public string filePath = string.Empty;

    private GameObject loadedObject;
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
            string[] objectData = new string[] {objectsList[i, 0], objectsList[i, 1], objectsList[i, 2]};
            CreateObject(objectData);
        }
    }

    //creates a single object
    public void CreateObject(string [] objectData)
    {
        ShowSelectObjFileDialogue(objectData);
    }

    private void CreateObjectFromFile(string filePath, string[] objectData)
    {
        UnityEngine.Debug.Log("Creating object from file");
        loadedObject = new OBJLoader().Load(filePath);
        //importLibrary.Add(filePath, this.filePath);
        SetObjectProperties(objectData);
    }

    private void ShowSelectObjFileDialogue(string[] objectData)
    {
        UnityEngine.Debug.Log("Showing dialogue");
        FileBrowser.SetFilters(false, ".obj");
        StartCoroutine( ShowSelectObjFileDialogueCoroutine(objectData) );
        
    }

    IEnumerator ShowSelectObjFileDialogueCoroutine(string[] objectData)
    {
        UnityEngine.Debug.Log("Running coroutine");
        yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.Files, false, null, null, "Load", "Select" );
        if (FileBrowser.Success)
        {
            CreateObjectFromFile(FileBrowser.Result[0], objectData);
        }
        else
        {
            CancelledImportHandler();
        }
    }

    // sets object properties
    private void SetObjectProperties(string[] objectData)
    {
        UnityEngine.Debug.Log("Setting object properties");
        //set name
        loadedObject.name = objectData[1];
        //set position
        string[] coords = objectData[2].Split(' ');
        loadedObject.transform.position = new Vector3(int.Parse(coords[0]), int.Parse(coords[1]), int.Parse(coords[2]));
    }

    //function to check if object type has been seen before
    /*private bool IsNewType(string objType)
    {
        return importLibrary.ContainsKey(objType);
    }
    */

    //to delete once file select dialogue is implemented
    public void SetString(string str)
    {
        filePath += str;
    }

    //placeholder for error handler for cancelled file imports
    private void CancelledImportHandler()
    {
        UnityEngine.Debug.Log("Import cancelled");
    }
}
}