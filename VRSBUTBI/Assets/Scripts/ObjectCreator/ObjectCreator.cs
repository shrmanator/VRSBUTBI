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
public sealed class ObjectCreator : MonoBehaviour
{
    public static ObjectCreator creator {get; private set;}
    private static Dictionary<string, GameObject> importLibrary = new Dictionary<string, GameObject>();
    private GameObject _loadedObject = null;
    private string[] _objectData = new string[3];
    private bool _isCreatingObject = false;


    private void Awake()
    {
        if (creator != null && creator != this)
        {
            Destroy(this);
        }
        else{
            creator = this;
        }
    }

    //creates objects from a list of objects
    public void CreateObjects(string [,] objectsList)
    {
        StartCoroutine(CreateObjectsCoroutine(objectsList));
    }

    public void CreateObject(string [] objectData)
    {
        StartCoroutine(CreateObjectCoroutine(objectData));
    }

    private IEnumerator CreateObjectsCoroutine(string [,] objectsList)
    {
        for (int i = 0; i < objectsList.GetLength(0); i++)
        {
            _loadedObject = null;
            _objectData[0] = objectsList[i, 0];
            _objectData[1] = objectsList[i, 1];
            _objectData[2] = objectsList[i, 2];
            UnityEngine.Debug.Log("New object " + _objectData[1]);
            SelectCreateMethod();
            yield return new WaitUntil(() => _isCreatingObject == false);
        }
    }

    private IEnumerator CreateObjectCoroutine(string [] objectData)
    {
        _loadedObject = null;
        SelectCreateMethod();
        yield return new WaitUntil(() => _isCreatingObject);
    }

    //creates a single object
    private void SelectCreateMethod()
    {
        _isCreatingObject = true;
        if (HasObjectType(_objectData[0]))
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
        _loadedObject = new OBJLoader().Load(filePath[0]);
        if (!HasObjectType(_objectData[0]))
        {
            importLibrary.Add(_objectData[0], _loadedObject.transform.GetChild(0).gameObject);
        }
        SetObjectProperties();
    }

    private void CreateObjectFromLibrary()
    {
        UnityEngine.Debug.Log("Creating object from library");
        _loadedObject = new GameObject();
        GameObject model = Instantiate(importLibrary[_objectData[0]]);
        model.transform.parent = _loadedObject.transform;
        SetObjectProperties();
    }

    private void ShowSelectObjFileDialogue(){
        FileBrowserHelper fileBrowser = gameObject.AddComponent<FileBrowserHelper>();
        string title = "Select .obj file to import for " + _objectData[0];
        string[] filter = {".obj"};
        fileBrowser.LoadSingleFile(CreateObjectFromFile, CancelledImportHandler, title, "Import", filter);
    }

    // sets object properties
    private void SetObjectProperties()
    {
        UnityEngine.Debug.Log("Setting object properties");
        //set name
        _loadedObject.name = _objectData[1];
        //set position
        string[] coords = _objectData[2].Split(' ');
        _loadedObject.transform.position = new Vector3(int.Parse(coords[0]), int.Parse(coords[1]), int.Parse(coords[2]));
        _loadedObject.transform.GetChild(0).name = _objectData[0];
        _isCreatingObject = false;
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
