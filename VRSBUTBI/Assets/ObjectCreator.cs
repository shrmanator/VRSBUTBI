using System.ComponentModel;
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
    Dictionary<string, GameObject> importLibrary = new Dictionary<string, GameObject>();
    string error = string.Empty;
    //file path for testing. Remove when dialogue prompt is implemented
    public string filePath = string.Empty;
    OBJLoader objLoader = new OBJLoader();
    private GameObject loadedObject;

    //creates objects from a list of objects
    public void CreateObjects(string [,] objectsList)
    {
        for (int i = 0; i < objectsList.GetLength(0); i++)
        {
            UnityEngine.Debug.Log("New object");
            string[] objectData = new string[] {objectsList[i, 0], objectsList[i, 1], objectsList[i, 2]};
            CreateObject(objectData);
        }
    }

    //creates a single object
    public void CreateObject(string [] objectData)
    {
        UnityEngine.Debug.Log(objectData);
        if (HasObjectType(objectData[0]))
        {
            CreateObjectFromLibrary(objectData);
        }
        else
        {
            ShowSelectObjFileDialogue(objectData);
        }
    }

    private void CreateObjectFromFile(string filePath, string[] objectData)
    {
        UnityEngine.Debug.Log("Creating object from file");
        loadedObject = objLoader.Load(filePath);
        if (!HasObjectType(objectData[0]))
        {
            importLibrary.Add(objectData[0], loadedObject.transform.GetChild(0).gameObject);
        }
        SetObjectProperties(objectData);
    }

    private void CreateObjectFromLibrary(string[] objectData)
    {
        UnityEngine.Debug.Log("Creating object from library");
        loadedObject = Instantiate(importLibrary[objectData[0]]);
        SetObjectProperties(objectData);
    }

    private void ShowSelectObjFileDialogue(string[] objectData)
    {
        UnityEngine.Debug.Log("Showing dialogue");
        FileBrowser.SetFilters(false, ".obj");
        StartCoroutine(ShowSelectObjFileDialogueCoroutine(objectData));
    }

    IEnumerator ShowSelectObjFileDialogueCoroutine(string[] objectData)
    {
        UnityEngine.Debug.Log("Running coroutine");
        string loadString = "Select model for " + objectData[0];
        yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.Files, false, null, null, loadString, "Import" );
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
        loadedObject.transform.GetChild(0).name = objectData[0];
    }

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

    private bool HasObjectType(string objectType)
    {
        UnityEngine.Debug.Log(importLibrary.ContainsKey(objectType));
        return importLibrary.ContainsKey(objectType);
    }
}
}