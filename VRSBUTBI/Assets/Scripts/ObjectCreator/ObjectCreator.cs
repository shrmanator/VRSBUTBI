using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummiesman;

/// <summary>
/// creates objects from provided data
/// object data expected to be an object list with the format {Object Type, Object Name, X, Y, Z}
/// <summary>
public sealed class ObjectCreator : MonoBehaviour
{
    public static ObjectCreator Creator {get; private set;}
    private static Dictionary<string, GameObject> _importLibrary = new Dictionary<string, GameObject>();
    private GameObject _loadedObject = null;
    private object[] _objectData = new object[5];
    private bool _isCreatingObject = false;
    private bool _isRetry = false;

    /// <summary>
    /// Ensures that there is only one instance of ObjectCreator
    /// </summary>

    void Start()
    {
        
    }
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
    public void CreateObjects(object [,] objectsList)
    {
        StartCoroutine(CreateObjectsCoroutine(objectsList));
    }

    /// <summary>
    /// Creates objects from a list.
    /// </summary>
    /// <param name="objectData">The data of the object to create</param>
    public void CreateObject(object [] objectData)
    {
        StartCoroutine(CreateObjectCoroutine(objectData));
    }

    /// <summary>
    /// Coroutine for creating objects from a list
    /// Note: has some code duplication with CreateObjectCoroutine. This is because the for loop must be inside the coroutine to work.
    /// The coroutine ensures that the object creation process happens in the correct order.
    /// </summary>
    /// <param name="objectsList">The data of the object to create</param>
    private IEnumerator CreateObjectsCoroutine(object [,] objectsList)
    {
        while (_isCreatingObject)
        {
            yield return null;
        }
        for (int i = 0; i < objectsList.GetLength(0); i++)
        {
            _loadedObject = null;
            _objectData[0] = objectsList[i, 0];
            _objectData[1] = objectsList[i, 1];
            _objectData[2] = objectsList[i, 2];
            _objectData[3] = objectsList[i, 3];
            _objectData[4] = objectsList[i, 4];
            _isCreatingObject = true;
            _isRetry = false;
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
    private IEnumerator CreateObjectCoroutine(object [] objectData)
    {
        while (_isCreatingObject){
            yield return null;
        }
        _loadedObject = null;
        _objectData = objectData;
        _isCreatingObject = true;
        _isRetry = false;
        SelectCreateMethod();
        yield return new WaitUntil(() => _isCreatingObject == false);
    }

    /// <summary>
    /// Determines whether the object is created from a pre-existing import or imported from file.
    /// To-Do: add check for assets imported before runtime
    /// </summary>
    private void SelectCreateMethod()
    {
        UnityEngine.Debug.Log("Creating " + _objectData[0] + " " + _objectData[1] + " at " + _objectData[2] + ", " +  _objectData[3] + ", " + _objectData[4]);
        if (IsInImportLibrary((string)_objectData[0]))
        {
            CreateObjectFromLibrary();
        }
        // Resources has no "check if file exists function" so we try to import 
        // and if it returns null, no file exists
        else if (ImportFromResources())
        {
            SetObjectProperties();
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
        AddObjectToImportLibrary();
        SetObjectProperties();
    }

    /// <summary>
    /// Copies the object from _importLibrary
    /// </summary>
    private void CreateObjectFromLibrary()
    {
        if (IsInImportLibrary((string)_objectData[0]))
        {
            _loadedObject = Instantiate(_importLibrary[(string)_objectData[0]]);
            SetObjectProperties();
        }
    }

    private void AddObjectToImportLibrary()
    {
        if (!IsInImportLibrary((string)_objectData[0]))
        {
            _importLibrary.Add((string)_objectData[0], _loadedObject);
        }
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
        _loadedObject.name = (string)_objectData[1];
        //set position
        _loadedObject.transform.position = new Vector3(float.Parse(_objectData[2].ToString()), float.Parse(_objectData[3].ToString()), float.Parse(_objectData[4].ToString()));
        _loadedObject.transform.GetChild(0).name = (string)_objectData[0];
        _isCreatingObject = false;
    }

    /// <summary>
    /// Attempts to create object again or stop object creation if this is the second attempt
    /// </summary>
    private void CancelledImportHandler()
    {
        if(_isRetry)
        {
            _isCreatingObject = false;
            UnityEngine.Debug.Log(
                "Canceled import on " + _objectData[0] + " " + _objectData[1]);
            _isRetry = false;
        }
        else{
            _isRetry = true;
            SelectCreateMethod();
        }
    }

    /// <summary>
    /// Checks if the object type matches an object in _importLibrary
    /// <param name="objectType">The object type to look up</param>
    /// </summary>
    private bool IsInImportLibrary(string objectType)
    {
        return _importLibrary.ContainsKey(objectType);
    }

    /// <summary>
    /// Imports asset from Resources folder or returns false if not found
    /// File must be in 'Assets/Resources' and the file name must match the object type
    /// </summary>
    private bool ImportFromResources()
    {
        var loadedObjectPrefab = Resources.Load((string)_objectData[0]) as GameObject;
        // Instantiates object if a file was found
        if (loadedObjectPrefab != null)
        {
            _loadedObject = GameObject.Instantiate(loadedObjectPrefab);
            AddObjectToImportLibrary();
            return true;
        }
        return false;
    }
}
