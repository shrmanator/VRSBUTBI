using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummiesman;

/// <summary>
/// creates objects from provided data
/// object data expected to be a string array with the format {Object Type, Object Name, X Y Z}
/// <summary>
public sealed class ObjectCreator : MonoBehaviour
{
    public static ObjectCreator Creator {get; private set;}
    private static Dictionary<string, GameObject> _importLibrary = new Dictionary<string, GameObject>();
    private GameObject _loadedObject = null;
    private string[] _objectData = new string[3];
    private bool _isCreatingObject = false;

    /// <summary>
    /// Ensures that there is only one instance of ObjectCreator
    /// </summary>
    void Awake()
    {
        if (Creator != null && Creator != this)
        {
            Destroy(this);
        }
        else{
            Creator = this;
        }
    }

    /// <summary>
    /// Creates objects from a list.
    /// </summary>
    /// <param name="objectsList">The list of objects to create</param>
    public void CreateObjects(string [,] objectsList)
    {
        StartCoroutine(CreateObjectsCoroutine(objectsList));
    }

    /// <summary>
    /// Creates objects from a list.
    /// </summary>
    /// <param name="objectData">The data of the object to create</param>
    public void CreateObject(string [] objectData)
    {
        StartCoroutine(CreateObjectCoroutine(objectData));
    }

    /// <summary>
    /// Coroutine for creating objects from a list
    /// Note: has some code duplication with CreateObjectCoroutine. This is because the for loop must be in the coroutine to work.
    /// The coroutine ensures that the object creation process happens in the correct order.
    /// </summary>
    /// <param name="objectsList">The data of the object to create</param>
    private IEnumerator CreateObjectsCoroutine(string [,] objectsList)
    {
        for (int i = 0; i < objectsList.GetLength(0); i++)
        {
            _loadedObject = null;
            _objectData[0] = objectsList[i, 0];
            _objectData[1] = objectsList[i, 1];
            _objectData[2] = objectsList[i, 2];
            _isCreatingObject = true;
            SelectCreateMethod();
            yield return new WaitUntil(() => _isCreatingObject == false);
        }
    }

    /// <summary>
    /// Coroutine for creating a single object
    /// Note: has some code duplication with CreateObjectCoroutine. This is because the for loop must be in the coroutine to work.
    /// CreateObject has a coroutine to prevent issues with multiple calls to CreateObject
    /// </summary>
    /// <param name="objectsList">The data of the object to create</param>
    private IEnumerator CreateObjectCoroutine(string [] objectData)
    {
        _loadedObject = null;
        _objectData[0] = objectData[0];
        _objectData[1] = objectData[1];
        _objectData[2] = objectData[2];
        _isCreatingObject = true;
        SelectCreateMethod();
        yield return new WaitUntil(() => _isCreatingObject == false);
    }

    /// <summary>
    /// Determines whether the object is created from a pre-existing import or imported from file.
    /// To-Do: add check for assets imported before runtime
    /// </summary>
    private void SelectCreateMethod()
    {
        if (IsInImportLibrary(_objectData[0]))
        {
            CreateObjectFromLibrary();
        }
        else
        {
            ShowSelectObjFileDialogue();
        }
    }

    /// <summary>
    /// Creates objects from a list.
    /// </summary>
    /// <param name="filePath">The path of the file to load</param>
    private void CreateObjectFromFile(string[] filePath)
    {
        _loadedObject = new OBJLoader().Load(filePath[0]);
        _importLibrary.Add(_objectData[0], _loadedObject);
        SetObjectProperties();
    }

    /// <summary>
    /// Copies the object from _importLibrary
    /// </summary>
    private void CreateObjectFromLibrary()
    {
        _loadedObject = Instantiate(_importLibrary[_objectData[0]]);
        SetObjectProperties();
    }

    /// <summary>
    /// Creates the appropriate file select prompt
    /// </summary>
    private void ShowSelectObjFileDialogue(){
        FileBrowserHelper fileBrowser = gameObject.AddComponent<FileBrowserHelper>();
        string title = "Select .obj file to import for " + _objectData[0];
        string[] filter = {".obj"};
        fileBrowser.LoadSingleFile(CreateObjectFromFile, CancelledImportHandler, title, "Import", filter);
    }

    /// <summary>
    /// Sets object properties
    /// </summary>
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

    /// <summary>
    /// Placeholder for cancelled import
    /// To-Do: reprompt user once and continue if cancelled again
    /// </summary>
    private void CancelledImportHandler()
    {
        UnityEngine.Debug.Log("Import cancelled");
    }

    /// <summary>
    /// Checks if the object type matches an object in _importLibrary
    /// <param name="objectType">The object type to look up</param>
    /// </summary>
    private bool IsInImportLibrary(string objectType)
    {
        return _importLibrary.ContainsKey(objectType);
    }
}
