using System.Runtime.Serialization;
/*
This script is responsible for handling the saving and loading of a simulation game state in Unity.

It provides methods for opening a file dialog to save and load the game state and uses the
JsonUtility class to serialize and deserialize a list of SerializableGameObject objects,
which are representations of GameObjects in the game world that are marked with the "Serializable" tag.

It also creates a persistent directory to store saved game states in and checks if the directory exists
before attempting to save to it. The script includes error handling for failed save or load attempts and
uses events to notify other objects when a file has been successfully loaded.

NOTE1: SerializableGameObject is a class found in the SerializableGameObject.cs script. 

NOTE2: Importing Models
    After building the game, please find the "imported_models" folder at the following location:

    - Windows: %USERPROFILE%\AppData\LocalLow\<CompanyName>\<ProductName>\imported_models
    - macOS: ~/Library/Application Support/<CompanyName>/<ProductName>/imported_models
    - Linux: ~/.config/unity3d/<CompanyName>/<ProductName>/imported_models

    Drop your OBJ files into this folder, the game will load the 3D models using the LoadExternalModels()
    method.
*/


using Dummiesman;
using System.Text;
using System.Collections;
using System.Linq;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using SimpleFileBrowser;
using System;


public class SimFileHandler : MonoBehaviour
{
    [SerializeField] private ObjectPrefabManager objectPrefabManager;

    public static SimFileHandler Handler { get; private set; }

    private string saveFileName = "saved_sim_state.bin";
    public static string savePath;

    private string importedModelsPath;

    public delegate void OnGameFileLoaded(string filePath);
    public static event OnGameFileLoaded GameFileLoaded;

    public delegate void OnTextFileLoaded(string filePath);
    public static event OnTextFileLoaded TextFileLoaded;

    private void Awake() 
    {
        if (Handler != null && Handler != this)
        {
            Destroy(this);
        }
        else
        {
            Handler = this;
        }
        CreateDirectories();
        LoadExternalModels();
        importedModelsPath = Path.Combine(Application.dataPath, "Imported_Models");
        savePath = Path.Combine(Application.dataPath, "SaveGames");
    }

    /// <summary>
    /// Returns an array of file paths to available models in the Imported_Models directory.
    /// </summary>
    /// <returns>An array of file paths to available models.</returns>
    public string[] GetImportedModelFilePaths()
    {
        if (!Directory.Exists(importedModelsPath))
        {
            Debug.LogError("Imported_Models directory not found. " + "instead points to >> " + importedModelsPath + " <<");
            return new string[0];
        }
        string[] availableModels = Directory.GetFiles(importedModelsPath, "*.obj", SearchOption.AllDirectories);

        print("imported model path are:");
        for (int i = 0; i < availableModels.Length; i++)
        {
            print(availableModels[i]);
        }

        return availableModels;
    }

    /// <summary>
    /// Checks if a model with the given name exists in the Imported_Models directory.
    /// </summary>
    /// <param name="modelName">The name of the model to check for.</param>
    /// <returns>True if the model exists, false otherwise.</returns>
    public bool ModelExists(string modelName)
    {
        string modelPath = Path.Combine(importedModelsPath, modelName + ".obj");
        return File.Exists(modelPath);
    }


    private void CreateDirectories()
    {
        savePath = Path.Combine(Application.persistentDataPath, "SaveGames");
        importedModelsPath = Path.Combine(Application.dataPath, "Imported_Models");

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
            print("SaveGames folder created at --> " + savePath);
        }

        if (!Directory.Exists(importedModelsPath))
        {
            Directory.CreateDirectory(importedModelsPath);
            print("Imported Models folder created at --> " + importedModelsPath);
        }

        print(GetImportedModelFilePaths());
    }

    public void OpenGameSaveDialog()
    {
        FileBrowser.SetFilters(false, new FileBrowser.Filter(".json ", ".json"));
        FileBrowser.ShowSaveDialog(OnGameSaveSuccess, OnSaveGameCancel, FileBrowser.PickMode.Files, false, savePath, "new_file.json", "Save File", "Save");
    }

    private void OnGameSaveSuccess(string[] filePaths)
    {
        Debug.Log("Saving: " + filePaths[0]);
        SaveGame(filePaths[0]);
    }

    private static SerializableGameObject[] GetSerializableGameObjects()
    {
        List<SerializableGameObject> serializableGameObjects = new List<SerializableGameObject>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Serializable"))
        {
            SerializableVector3 position = new SerializableVector3(obj.transform.position);
            SerializableVector3 rotation = new SerializableVector3(obj.transform.rotation.eulerAngles);
            SerializableVector3 scale = new SerializableVector3(obj.transform.localScale);
            SerializableGameObject serializedObject = new SerializableGameObject(obj.name, obj.transform.GetChild(0).name, position, rotation, scale);

            serializableGameObjects.Add(serializedObject);
        }
        return serializableGameObjects.ToArray();
    }

    public void SaveGame(string filePath)
    {
        var gameObjects = GetSerializableGameObjects();
        if (gameObjects == null || gameObjects.Length == 0)
        {
            Debug.LogWarning("Cannot save empty game state.");
            return;
        }

        String fileName = Path.GetFileNameWithoutExtension(filePath);

        // filePath = Path.Combine(savePath, fileName + ".json");

        try
        {
            string json = JsonUtility.ToJson(new SerializableGameObjectWrapper { gameObjects = gameObjects }, true);
            File.WriteAllText(filePath, json);
            Debug.Log($"Simulation state saved to {filePath}");
        }
        catch (IOException ex)
        {
            Debug.LogError($"Failed to save game to {filePath}: {ex.Message}");
        }
    }

    public void LoadGame(string filePath)
    {
        SerializableGameObject[] gameObjects = SimFileHandler.GetSerializableGameObjectFromFile(filePath);
        if (gameObjects != null)
        {
            InstantiateLoadedObjects(gameObjects);
        }
        GameFileLoaded?.Invoke(filePath);
    }

    private static SerializableGameObject[] GetSerializableGameObjectFromFile(string fileName)
    {
        string filePath = Path.Combine(savePath, fileName);
        Debug.Log("Loading from: " + filePath);

        if (!File.Exists(filePath))
        {
            Debug.LogError("Failed to load game from " + filePath + ": File not found");
            return null;
        }

        try
        {
            string json = File.ReadAllText(filePath);
            SerializableGameObjectWrapper wrapper = JsonUtility.FromJson<SerializableGameObjectWrapper>(json);
            return wrapper.gameObjects;
        }
        catch (IOException ex)
        {
            Debug.LogError("Failed to load game from " + filePath + ": " + ex.Message);
            return null;
        }
    }

    public void OpenTextFileLoadDialog()
    {
        FileBrowser.SetFilters(false, new FileBrowser.Filter(".txt", ".txt"));
        FileBrowser.ShowLoadDialog(OnLoadTextSuccess, OnLoadTextCancel, FileBrowser.PickMode.Files, false, null, "", "Load File", "Load");
    }

    public void OpenSimStateLoadDialog()
    {
        FileBrowser.SetFilters(false, new FileBrowser.Filter(".json", ".json"));
        FileBrowser.ShowLoadDialog(OnLoadGameSuccess, OnLoadGameCancel, FileBrowser.PickMode.Files, false, null, "", "Load File", "Load");
    }

    /// <summary>
    /// Handles a successful load.
    /// </summary>
    /// <param name="filePaths">The paths of the saved files.</param>
    private void OnLoadGameSuccess(string[] filePaths)
    {
        SerializableGameObject[] gameObjects = SimFileHandler.GetSerializableGameObjectFromFile(filePaths[0]);
        if (gameObjects != null)
        {
            InstantiateLoadedObjects(gameObjects);
        }
        GameFileLoaded?.Invoke(filePaths[0]);
    }

    private void InstantiateLoadedObjects(SerializableGameObject[] loadedObjects)
    {
        if (loadedObjects == null || loadedObjects.Length == 0)
        {
            Debug.LogWarning("No game objects to instantiate.");
            return;
        }

        if (ObjectPrefabManager.Manager == null)
        {
            Debug.LogError("ObjectPrefabManager not found in the scene.");
            return;
        }

        foreach (SerializableGameObject loadedObject in loadedObjects)
        {
            /*GameObject prefab = ObjectPrefabManager.Manager.GetPrefabByType(loadedObject.objectType);
            if (prefab != null)
            {
                GameObject newGameObject = Instantiate(prefab);
                newGameObject.name = loadedObject.objectName;
                newGameObject.tag = "Serializable";
                newGameObject.transform.position = loadedObject.position.ToVector3();
                newGameObject.transform.rotation = Quaternion.Euler(loadedObject.rotation.ToVector3());
                newGameObject.transform.localScale = loadedObject.scale.ToVector3();
                newGameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("SimFileHandler.cs error: Prefab not found for object type: " + loadedObject.objectType);
            }*/
            ObjectManager.Manager.CreateObject(loadedObject.ToObjectData());
        }
    }

    // Loads models from the imported_models directory.
    public void LoadExternalModels()
    {
        // Specify the folder where users can drop their 3D model files
        string importFolder = Path.Combine(Application.persistentDataPath, "imported_models");

        // Get all the files in the import folder
        string[] modelFiles = Directory.GetFiles(importFolder, "*.obj");

        // Load each model file
        foreach (string modelFile in modelFiles)
        {
            //StartCoroutine(LoadModel(modelFile));
            ObjectManager.Manager.CreateModelFromFile(modelFile);
        }
    }

   /* private IEnumerator LoadModel(string filePath)
    {
        WWW www = new WWW("file://" + filePath);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError("Error loading OBJ file: " + www.error);
            yield break;
        }

        // Instantiate the loaded model using OBJImporter
        Mesh holderMesh = new Mesh();
        GameObject obj = new OBJLoader().Load(new MemoryStream(Encoding.UTF8.GetBytes(www.text)));
        MeshFilter[] meshFilters = obj.GetComponentsInChildren<MeshFilter>();
        List<Mesh> meshes = new List<Mesh>();

        foreach (MeshFilter mf in meshFilters)
        {
            meshes.Add(mf.sharedMesh);
        }


        // Use the list of meshes (meshes) for further processing
        GameObject modelGameObject = new GameObject(Path.GetFileNameWithoutExtension(filePath));
        modelGameObject.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = modelGameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = holderMesh;

        // Add the "Serializable" tag to the model GameObject
        modelGameObject.tag = "Serializable";

        // Create and assign a material to the GameObject (optional)
        Material modelMaterial = new Material(Shader.Find("Standard"));
        modelGameObject.GetComponent<Renderer>().material = modelMaterial;

        // Add the GameObject to the ObjectPrefabManager's list (optional)
        objectPrefabManager.AddObjectToPrefabList(modelGameObject);
    }*/

    private void OnLoadTextSuccess(string[] filePaths)
    {
        TextFileLoaded?.Invoke(filePaths[0]);
    }

    private void OnLoadTextCancel()
    {
        Debug.Log("Text file load cancelled");
    }

    private void OnSaveGameCancel()
    {
        Debug.Log("Save game canceled.");
    }

    private void OnLoadGameCancel()
    {
        Debug.Log("Load game canceled.");
    }
}


[Serializable]
public class SerializableGameObjectWrapper
{
    public SerializableGameObject[] gameObjects;
}
