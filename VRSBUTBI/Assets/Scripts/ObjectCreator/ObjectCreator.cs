using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Dummiesman;
using SimpleFileBrowser;
using static FileBrowserHelper;


// creates objects from provided data
// object data expected to be a string array with the format {Object Type, Object Name, X Y Z}
namespace ObjectCreator {
public sealed class ObjCreator : MonoBehaviour
{
    Dictionary<string, GameObject> importLibrary = new Dictionary<string, GameObject>();
    //OBJLoader objLoader = new OBJLoader();
    private GameObject loadedObject = null;
    private string[] objectData = new string[3];
    private bool objectCreatedFlag = false;

    //creates objects from a list of objects
    public void CreateObjects(string [,] objectsList)
    {
        StartCoroutine(CreateObjectsCoroutine(objectsList));
    }

    private IEnumerator CreateObjectsCoroutine(string [,] objectsList)
    {
        for (int i = 0; i < objectsList.GetLength(0); i++)
        {
            loadedObject = null;
            objectData[0] = objectsList[i, 0];
            objectData[1] = objectsList[i, 1];
            objectData[2] = objectsList[i, 2];
            UnityEngine.Debug.Log("New object " + objectData[1]);
            CreateObject();
            yield return new WaitUntil(() => objectCreatedFlag);
        }
    }

    //creates a single object
    private void CreateObject()
    {
        objectCreatedFlag = false;
        if (HasObjectType(objectData[0]))
        {
            CreateObjectFromLibrary();
        }
        else
        {
            ShowSelectObjFileDialogue();
        }
    }

    private void CreateObjectFromFile(string[] filePath)
    {
        UnityEngine.Debug.Log(filePath[0]);
        loadedObject = new OBJLoader().Load(filePath[0]);
        if (!HasObjectType(objectData[0]))
        {
            importLibrary.Add(objectData[0], loadedObject.transform.GetChild(0).gameObject);
        }
        SetObjectProperties();
    }

    private void CreateObjectFromLibrary()
    {
        UnityEngine.Debug.Log("Creating object from library");
        loadedObject = new GameObject();
        GameObject model = Instantiate(importLibrary[objectData[0]]);
        model.transform.parent = loadedObject.transform;
        SetObjectProperties();
    }

    private void ShowSelectObjFileDialogue(){
        FileBrowserHelper fileBrowser = gameObject.AddComponent<FileBrowserHelper>();
        string title = "Select .obj file to import for " + objectData[0];
        string[] filter = {".obj"};
        fileBrowser.LoadSingleFile(CreateObjectFromFile, CancelledImportHandler, title, "Import", filter);
    }

    // sets object properties
    private void SetObjectProperties()
    {
        UnityEngine.Debug.Log("Setting object properties");
        //set name
        loadedObject.name = objectData[1];
        //set position
        string[] coords = objectData[2].Split(' ');
        loadedObject.transform.position = new Vector3(int.Parse(coords[0]), int.Parse(coords[1]), int.Parse(coords[2]));
        loadedObject.transform.GetChild(0).name = objectData[0];
        objectCreatedFlag = true;
    }

    //placeholder for error handler for cancelled file imports
    private void CancelledImportHandler()
    {
        UnityEngine.Debug.Log("Import cancelled");
    }

    private bool HasObjectType(string objectType)
    {
        UnityEngine.Debug.Log(importLibrary.ContainsKey(objectType));
        return importLibrary.ContainsKey(objectType);
    }
}
}