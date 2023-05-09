using UnityEngine;
using Dummiesman;
using System.IO;
using System.Text;
using System.Collections;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using SimpleFileBrowser;
using System;

/// <summary>
/// creates objects from provided data
/// object data expected to be an object list with the format {Object Type, Object Name, X, Y, Z}
/// <summary>
public sealed class ObjectManager : MonoBehaviour
{
    public delegate void ObjectCreatedReceivedEventHandler();
    public static event ObjectCreatedReceivedEventHandler ObjectCreated;
    public static ObjectManager Manager {get; private set;}
    //private static Dictionary<string, GameObject> _importLibrary = new Dictionary<string, GameObject>();
    private GameObject _loadedObject = null;
    private object[] _objectData = new object[5];
    private bool _isCreatingObject = false;
    private bool _isRetry = false;


    /// <summary>
    /// Subscribe to Create, Destroy, and SetObj events
    /// </summary>
    void Start()
    {
        FileParser.CreateCommandReceived += CreateObject;
        ScenePlayer.DestroyCommandReceived += DestroyObject;
        ScenePlayer.SetObjCommandReceived += ChangeObjectProperty;
        ScenePlayer.DynUpdateCommandReceived += DynamicallyChangeObjectProperty;
    }
    /// <summary>
    /// Ensures that there is only one instance of ObjectCreator
    /// </summary>
    void Awake()
    {
        if (Manager != null && Manager != this)
        {
            Destroy(this);
        }
        else{
            Manager = this;
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
    /// Creates model object from a .obj file.
    /// </summary>
    /// <param name="filePath">The .obj file to load</param>
    public void CreateModelFromFile(string filePath)
    {
        GameObject model = new OBJLoader().Load(filePath);
        if (!ObjectPrefabManager.Manager.HasPrefab(model.name))
        {
            ObjectPrefabManager.Manager.AddObjectToPrefabList(model);
        }
        string name = model.name;
    }

    /// <summary>
    /// Coroutine for creating objects from a list
    /// Note: has some code duplication with CreateObjectCoroutine. This is because the for loop must be inside the coroutine to work.
    /// The coroutine ensures that the object creation process happens in the correct order.
    /// </summary>
    /// <param name="objectsList">The data of the object to create. 
    /// Expected object data: {object name, object type, x, y, z starting coordinates
    /// </param>
    private IEnumerator CreateObjectsCoroutine(object [,] objectsList)
    {
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
            yield return new WaitWhile(() => _isCreatingObject);
        }
    }

    /// <summary>
    /// Coroutine for creating a single object
    /// Note: has some code duplication with CreateObjectCoroutine. This is because the for loop must be in the coroutine to work.
    /// CreateObject has a coroutine to prevent issues with multiple calls to CreateObject
    /// </summary>
    /// <param name="objectsList">The data of the object to create. 
    /// Expected object data: {object name, object type, x, y, z starting coordinates
    /// </param>
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
        yield return new WaitWhile(() => _isCreatingObject);
    }

    /// <summary>
    /// Determines whether the object is created from a pre-existing import or imported from file.
    /// To-Do: add check for assets imported before runtime
    /// </summary>
    private void SelectCreateMethod()
    {
        UnityEngine.Debug.Log("Creating " + _objectData[1] + " " + _objectData[0] + " at " + _objectData[2] + ", " +  _objectData[3] + ", " + _objectData[4]);
        if (ObjectPrefabManager.Manager.HasPrefab((string)_objectData[1]))
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
        ObjectPrefabManager.Manager.AddObjectToPrefabList(_loadedObject);
        SetObjectProperties();
    }

    /// <summary>
    /// Copies the object from _importLibrary
    /// </summary>
    private void CreateObjectFromLibrary()
    {
        if (ObjectPrefabManager.Manager.HasPrefab((string)_objectData[1]))
        {
            _loadedObject = Instantiate(ObjectPrefabManager.Manager.GetPrefabByType((string)_objectData[1]));
            _loadedObject.SetActive(true); 
            SetObjectProperties();
        }
    }

   /* private void AddObjectToImportLibrary()
    {
        if (!IsInImportLibrary((string)_objectData[1]))
        {
            ObjectPrefabManager.Manager.AddObjectToPrefabList(_loadedObject);
        }
    }*/

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
        //set name
        _loadedObject.name = (string)_objectData[0];
        //set position
        _loadedObject.transform.position = new Vector3(float.Parse(_objectData[2].ToString()), float.Parse(_objectData[3].ToString()), float.Parse(_objectData[4].ToString()));
        _loadedObject.transform.GetChild(0).name = (string)_objectData[1];
        _loadedObject.tag = "Serializable";
        _isCreatingObject = false;
        ObjectCreated?.Invoke();
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
            ObjectCreated?.Invoke();
        }
        else{
            _isRetry = true;
            SelectCreateMethod();
        }
    }

    /*
    /// <summary>
    /// Checks if the object type matches an object in _importLibrary
    /// </summary>
    /// <param name="objectType">The object type to look up</param>
    private bool IsInImportLibrary(string objectType)
    {
        return _importLibrary.ContainsKey(objectType);
    }*/

    /// <summary>
    /// Imports asset from Resources folder or returns false if not found
    /// File must be in 'Assets/Resources' and the file name must match the object type
    /// </summary>
    private bool ImportFromResources()
    {
        var loadedObjectPrefab = Resources.Load((string)_objectData[1]) as GameObject;
        // Instantiates object if a file was found
        if (loadedObjectPrefab != null)
        {
            _loadedObject = GameObject.Instantiate(loadedObjectPrefab);
            ObjectPrefabManager.Manager.AddObjectToPrefabList(_loadedObject);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Changes an object's properties. Called on SetObjCommandReceived event. 
    /// </summary>
    /// <param name="data"> Expected {"SETOBJCELL", object name, property name, property value(s)
    /// Currently supported properties and expected values:
    /// "TRANSFORM" float x , float y , float x scale factors 
    /// "ROTATE" float x, float y, float z, degrees 
    /// </param>
    public void ChangeObjectProperty (object[] data){
        GameObject obj = GameObject.Find((string)data[1]);
        if (obj == null){
            UnityEngine.Debug.Log(data[1] + " not found!");
            return;
        }
        switch ((string)data[2]){
            case "TRANSFORM":
                if (data.Length != 6){
                    UnityEngine.Debug.Log("Invalid syntax for SETOBJCELL TRANSFORM");
                    return;
                }
                float xScale = float.Parse(data[3].ToString());
                float yScale = float.Parse(data[4].ToString());
                float zScale = float.Parse(data[5].ToString());
                ChangeObjectSize(obj, xScale, yScale, zScale);
                break;
            case "ROTATE":
                if (data.Length != 6){
                    UnityEngine.Debug.Log("Invalid syntax for SETOBJCELL ROTATE");
                    return;
                }
                float xDegrees = float.Parse(data[3].ToString());
                float yDegrees = float.Parse(data[4].ToString());
                float zDegrees = float.Parse(data[5].ToString());
                RotateObject(obj, xDegrees, yDegrees, zDegrees);
                break;
            default:
                UnityEngine.Debug.Log("Unidentified property");
                break;
        }
    }

    /// <summary>
    /// Changes an object's size. A value of 0 keeps that dimension at its current scale
    /// </summary>
    /// <param name="obj"> The object to change</param>
    /// <param name="x"> x axis scale factor</param>
    /// <param name="y"> y axis scale factor</param>
    /// <param name="z"> z axis scale factor</param>
    private void ChangeObjectSize (GameObject obj, float x, float y, float z){
        if (x == 0){
            x = obj.transform.localScale.x;
        }
        if (y == 0)
        {
            y = obj.transform.localScale.y;
        }
        if (z == 0)
        {
            z = obj.transform.localScale.z;
        }
        obj.transform.localScale = new Vector3(x, y, z);
    }


    /// <summary>
    /// Changes an object's angle.
    /// </summary>
    /// <param name="obj"> The object to change </param>
    /// <param name="x"> Degrees to rotate on x axis </param>
    /// <param name="y"> Degrees to rotate on y axis </param>
    /// <param name="z"> Degrees to rotate on z axis </param>
    private void RotateObject(GameObject obj, float x, float y, float z){
        obj.transform.Rotate(x, y, z);
    }

    /// <summary>
    /// Destroys a selected object. Called on DestroyCommandReceived event.
    /// </summary>
    /// <param name="name">Name of the object to destory</param>
    public void DestroyObject(string name){
        UnityEngine.Debug.Log(name);
        var obj = GameObject.Find(name);
        UnityEngine.Debug.Log(obj);
        if (obj == null)
        {
            UnityEngine.Debug.Log(name + " not found!");
            return;
        }
        Destroy(obj);
    }

    /// <summary>
    /// Dynamically changed the select object property. Called on DynUpdateCommandReceived event
    /// </summary>
    /// <param name="data">Expected [DYNUPDATECELL, objectName, PROPERTYNAME, duration, x, y, z] </param>
    public void DynamicallyChangeObjectProperty(object[] data){
        UnityEngine.Debug.Log("DYNUPDATECELL command started");
        GameObject obj = GameObject.Find((string)data[1]);
        if (obj == null)
        {
            UnityEngine.Debug.Log(data[2] + " not found!");
            return;
        }
        switch ((string)data[2])
        {
            case "TRANSFORM":
                if (data.Length != 7)
                {
                    UnityEngine.Debug.Log("Invalid syntax for SETOBJCELL TRANSFORM");
                    return;
                }
                float xScale = float.Parse(data[4].ToString());
                float yScale = float.Parse(data[5].ToString());
                float zScale = float.Parse(data[6].ToString());
                float timeTransform = float.Parse(data[3].ToString());
                DynamicallyChangeObjectSize(obj, xScale, yScale, zScale, timeTransform);
                break;
            case "ROTATE":
                if (data.Length != 7)
                {
                    UnityEngine.Debug.Log("Invalid syntax for SETOBJCELL ROTATE");
                    return;
                }
                float xDegrees = float.Parse(data[4].ToString());
                float yDegrees = float.Parse(data[5].ToString());
                float zDegrees = float.Parse(data[6].ToString());
                float timeRotate = float.Parse(data[3].ToString());
                DynamicallyRotateObject(obj, xDegrees, yDegrees, zDegrees, timeRotate);
                break;
            default:
                UnityEngine.Debug.Log("Unidentified property");
                break;
        }
    }

    /// <summary>
    /// Dynamically changed the object's size.
    /// </summary>
    /// <param name="obj"> The object to change</param>
    /// <param name="x"> x axis scale factor</param>
    /// <param name="y"> y axis scale factor</param>
    /// <param name="z"> z axis scale factor</param>
    private void DynamicallyChangeObjectSize(GameObject obj, float x, float y, float z, float time)
    {
        if (x == 0)
        {
            x = obj.transform.localScale.x;
        }
        if (y == 0)
        {
            y = obj.transform.localScale.y;
        }
        if (z == 0)
        {
            z = obj.transform.localScale.z;
        }
        Vector3 scale = new Vector3(x, y, z);
        DynamicObjectTransformer script = obj.AddComponent(typeof(DynamicObjectTransformer)) as DynamicObjectTransformer;
        script.SetTransform(scale, time);
    }

    /// <summary>
    /// Dynamically changed the object's rotation.
    /// </summary>
    /// <param name="obj"> The object to change</param>
    /// <param name="x"> x axis angle</param>
    /// <param name="y"> y axis angle</param>
    /// <param name="z"> z axis angle</param>
    private void DynamicallyRotateObject(GameObject obj, float x, float y, float z, float time)
    {
        Vector3 angle = new Vector3(x, y, z);
        DynamicObjectRotator script = obj.AddComponent(typeof(DynamicObjectRotator)) as DynamicObjectRotator;
        script.SetTransform(angle, time);
    }

}
