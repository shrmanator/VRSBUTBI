/*
This script is responsible for handling the saving and loading of a simulation game state in Unity.

It provides methods for opening a file dialog to save and load the game state and uses the
BinaryFormatter class to serialize and deserialize a list of SerializableGameObject objects,
which are representations of GameObjects in the game world that are marked with the "Serializable" tag.

It also creates a persistent directory to store saved game states in and checks if the directory exists
before attempting to save to it. The script includes error handling for failed save or load attempts and
uses events to notify other objects when a file has been successfully loaded.

NOTE: SerializableGameObject is a class found in the SerializableGameObject.cs script. 
*/

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using SimpleFileBrowser;
using System;

public class SimFileHandler : MonoBehaviour
{
    [SerializeField] private ObjectPrefabManager objectPrefabManager;

    private string saveFileName = "saved_sim_state.bin";
    private static string savePath;

    public delegate void OnGameFileLoaded(string filePath);
    public static event OnGameFileLoaded GameFileLoaded;

    public delegate void OnTextFileLoaded(string filePath);
    public static event OnTextFileLoaded TextFileLoaded;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "savegames");
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
    }

    public void OpenGameSaveDialog()
    {
        FileBrowser.SetFilters(false, new FileBrowser.Filter(".bin", ".bin"));
        FileBrowser.ShowSaveDialog(OnGameSaveSuccess, OnSaveGameCancel, FileBrowser.PickMode.Files, false, savePath, "new_file.bin", "Save File", "Save");
    }

    private void OnGameSaveSuccess(string[] filePaths)
    {
        SaveGame(filePaths[0], GetSerializableGameObjects());
    }

    private SerializableGameObject[] GetSerializableGameObjects()
    {
        List<SerializableGameObject> serializableGameObjects = new List<SerializableGameObject>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Serializable"))
        {
            SerializableVector3 position = new SerializableVector3(obj.transform.position);
            SerializableVector3 rotation = new SerializableVector3(obj.transform.rotation.eulerAngles);
            SerializableVector3 scale = new SerializableVector3(obj.transform.localScale);
            SerializableGameObject serializedObject = new SerializableGameObject(obj.name, position, rotation, scale);

            serializableGameObjects.Add(serializedObject);
        }
        return serializableGameObjects.ToArray();
    }

    public static void SaveGame(string fileName, SerializableGameObject[] gameObjects)
    {
        if (gameObjects == null || gameObjects.Length == 0)
        {
            Debug.LogWarning("Cannot save empty game state.");
            return;
        }

        if (Path.HasExtension(fileName))
        {
            fileName = Path.GetFileNameWithoutExtension(fileName);
        }

        string filePath = Path.Combine(savePath, fileName + ".json");

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


    public static SerializableGameObject[] LoadGame(string fileName)
    {
        string filePath = Path.Combine(savePath, fileName);

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
        SerializableGameObject[] gameObjects = SimFileHandler.LoadGame(filePaths[0]);
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

        ObjectPrefabManager prefabManager = FindObjectOfType<ObjectPrefabManager>();
        if (prefabManager == null)
        {
            Debug.LogError("ObjectPrefabManager not found in the scene.");
            return;
        }

        foreach (SerializableGameObject loadedObject in loadedObjects)
        {
            GameObject prefab = prefabManager.GetPrefabByName(loadedObject.objectName);
            if (prefab != null)
            {
                GameObject newGameObject = Instantiate(prefab);
                newGameObject.name = loadedObject.objectName;
                newGameObject.tag = "Serializable";
                newGameObject.transform.position = loadedObject.position.ToVector3();
                newGameObject.transform.rotation = Quaternion.Euler(loadedObject.rotation.ToVector3());
                newGameObject.transform.localScale = loadedObject.scale.ToVector3();
            }
            else
            {
                Debug.LogWarning("SimfileHandler.cs error: Prefab not found for object name: " + loadedObject.objectName);
            }
        }
    }


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
