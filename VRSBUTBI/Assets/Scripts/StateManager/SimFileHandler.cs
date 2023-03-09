/*
This script is responsible for handling the saving and loading of a simulation game's state in Unity.
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
        FileBrowser.SetFilters(false, new FileBrowser.Filter(".bin", ".bin"));
        FileBrowser.ShowSaveDialog(OnGameSaveSuccess, OnSaveGameCancel, FileBrowser.PickMode.Files, false, null, "new_file", "Save File", "Save");
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
        // Check if gameObjects is null or empty
        if (gameObjects == null || gameObjects.Length == 0)
        {
            Debug.LogWarning("Cannot save empty game state.");
            return;
        }

        // Remove any extra file name from the save path
        if (Path.HasExtension(fileName))
        {
            fileName = Path.GetFileNameWithoutExtension(fileName);
        }

        // Create persistent directory if it does not exist
        string directory = Path.Combine(Application.persistentDataPath, "SimSaves");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Create file path
        string filePath = Path.Combine(directory, fileName + ".dat");

        // Open file stream
        FileStream fileStream = null;

        try
        {
            if (File.Exists(filePath))
            {
                fileStream = File.Open(filePath, FileMode.Open);
                if (fileStream.Length > 0)
                {
                    // Deserialize game objects from file
                    BinaryFormatter binaryFormatter = new BinaryFormatter();

                    foreach (SerializableGameObject gameObject in gameObjects)
                    {
                        gameObject.Deserialize(binaryFormatter.Deserialize(fileStream));
                    }
                }
            }
            else
            {
                fileStream = File.Create(filePath);

                // Serialize game objects and write to file
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                foreach (SerializableGameObject gameObject in gameObjects)
                {
                    binaryFormatter.Serialize(fileStream, gameObject);
                }
            }
            Debug.Log($"Simulation state saved to {filePath}");
        }
        catch (IOException ex)
        {
            Debug.LogError($"Failed to save game to {filePath}: {ex.Message}");
        }
        finally
        {
            // Close file stream
            if (fileStream != null)
            {
                fileStream.Close();
            }
        }
    }




    public static SerializableGameObject[] LoadGame(string fileName)
    {
        // Create file path
        string filePath = Path.Combine(Application.persistentDataPath, "SimSaves", fileName);

        // Check if file exists
        if (!File.Exists(filePath))
        {
            Debug.LogError("Failed to load game from " + filePath + ": File not found");
            return null;
        }

        // Open file stream
        FileStream fileStream = null;
        try
        {
            fileStream = File.OpenRead(filePath);

            // Deserialize game objects and print their contents
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
        catch (IOException ex)
        {
            Debug.LogError("Failed to load game from " + filePath + ": " + ex.Message);
            return null;
        }
        finally
        {
            // Close file stream
            if (fileStream != null)
            {
                fileStream.Close();
            }
        }
    }

    public void OpenTextFileLoadDialog()
    {
        FileBrowser.SetFilters(false, new FileBrowser.Filter(".txt", ".txt"));
        FileBrowser.ShowLoadDialog(OnLoadTextSuccess, OnLoadTextCancel, FileBrowser.PickMode.Files, false, null, "", "Load File", "Load");
    }

    public void OpenSimStateLoadDialog()
    {
        FileBrowser.SetFilters(false, new FileBrowser.Filter(".bin", ".bin"));
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

