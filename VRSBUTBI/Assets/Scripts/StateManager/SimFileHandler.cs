/*
This script is responsible for handling the saving and loading of a simulation game state in Unity.

It provides methods for opening a file dialog to save and load the game state and uses the
BinaryFormatter class to serialize and deserialize a list of SerializableGameObject objects,
which are representations of GameObjects in the game world that are marked with the "Serializable" tag.

It also creates a persistent directory to store saved game states in and checks if the directory exists
before attempting to save to it. The script includes error handling for failed save or load attempts and
uses events to notify other objects when a file has been successfully loaded.
*/

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using SimpleFileBrowser;
using System.Runtime.Serialization;
using System.Reflection;
using System;
using System.Linq;


public class SimFileHandler : MonoBehaviour
{
    private string saveFileName = "save_game.dat";
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
        // FileBrowser is defined in the SimpleFileBrowser library
        FileBrowser.SetFilters(false, new FileBrowser.Filter(".bin", ".bin"));
        FileBrowser.ShowSaveDialog(OnGameSaveSuccess, OnSaveGameCancel, FileBrowser.PickMode.Files, false, null, "new_file.bin", "Save File", "Save");
    }

    /// <summary>
    /// Handles a successful save.
    /// </summary>
    /// <param name="filePaths">The paths of the saved files.</param>
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
            SerializableGameObject serializedObject = new SerializableGameObject(obj.name, position, rotation);

            SerializableGameObject testGameObject = new SerializableGameObject("MyObject", new SerializableVector3(new Vector3(1f, 2f, 3f)), new SerializableVector3(new Vector3(0f, 90f, 0f)));
            serializableGameObjects.Add(testGameObject);
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

        string directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(directory, fileName + ".dat");

        try
        {
            using (FileStream fileStream = File.Create(filePath))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, gameObjects);
            }

            Debug.Log($"Simulation state saved to {filePath}");
        }
        catch (IOException ex)
        {
            Debug.LogError($"Failed to save game to {filePath}: {ex.Message}");
        }
    }




    public static SerializableGameObject[] LoadGame(string fileName)
    {
        string directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(directory, fileName);

        if (!File.Exists(filePath))
        {
            Debug.LogError("Failed to load game from " + filePath + ": File not found");
            return null;
        }

        try
        {
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Binder = new VersionDeserializationBinder(); // Use custom binder
                SerializableGameObject[] gameObjects = (SerializableGameObject[])binaryFormatter.Deserialize(fileStream);
                if (gameObjects != null && gameObjects.Length > 0)
                {
                    foreach (SerializableGameObject gameObject in gameObjects)
                    {
                        Debug.Log("Object name: " + gameObject.name);
                        // Print or display other properties of the game object as desired
                    }
                }
                else
                {
                    Debug.LogWarning("No game objects found in file: " + filePath);
                }
                return gameObjects;
            }
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
        FileBrowser.SetFilters(false, new FileBrowser.Filter(".dat", ".dat"));
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
            print(gameObjects);
        }
        GameFileLoaded?.Invoke(filePaths[0]);
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

