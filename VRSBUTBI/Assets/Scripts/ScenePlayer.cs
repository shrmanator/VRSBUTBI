using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;

/// <summary>
/// Managers the execution of a scene
/// <summary>
public class ScenePlayer : MonoBehaviour
{
    // The instance to call. 
    // ScenePlayer needs to be a singleton to ensure that there's only 1 active list of commands
    public static ScenePlayer Player { get; private set; }

    // Event handlers for commands
    //public delegate void CreateCommandReceivedEventHandler(object[] newObject);
    //public static event CreateCommandReceivedEventHandler CreateCommandReceived;

    public delegate void SetObjCommandReceivedEventHandler(object[] data);
    public static event SetObjCommandReceivedEventHandler SetObjCommandReceived;

    public delegate void DestroyCommandReceivedEventHandler(string objectName);
    public static event DestroyCommandReceivedEventHandler DestroyCommandReceived;

    public delegate void DynUpdateCommandReceivedEventHandler(object[] data);
    public static event DynUpdateCommandReceivedEventHandler DynUpdateCommandReceived;

    public delegate void PathCommandReceivedEventHandler(object[] data);
    public static event PathCommandReceivedEventHandler PathCommandReceived;

    public delegate void MoveCommandReceivedEventHandler(object[] data);
    public static event MoveCommandReceivedEventHandler MoveCommandReceived;

    // The list of commands for playing the scene
    List<object[]> commands;
    List<object[]> createCommands;

    bool isCreatingObjects;
    bool isPlayingScene = false;

    // floats for the timing of commands
    float startTime;
    float waitTime = 0;

    private void Start()
    {
        ObjectManager.AllObjectsCreated += OnObjectsCreated;
    }
    /// <summary>
    /// Ensures that only one instance of ScenePlayer exists at a time
    /// <summary>
    void Awake()
    {
        if (Player != null && Player != this)
        {
            Destroy(this);
        }
        else
        {
            Player = this;
        }
    }

    /// <summary>
    /// Set a new list of commands to use
    /// <param name="newCommands">The list of commands to use</param>
    /// <summary>
    public void SetScene(List<object[]> newCommands, List<object[]> newCreates)
    {
        commands = newCommands;
        createCommands = newCreates;
        StartCoroutine(SetSceneCoroutine());
        
    }

    private IEnumerator SetSceneCoroutine()
    {
        ClearScene();
        isCreatingObjects = true;
        ObjectManager.Manager.CreateObjects(createCommands);
        yield return new WaitWhile(() => isCreatingObjects);
        SaveStartScene();
    }

    public bool CheckObjectsExist()
    {
        foreach (object [] create in createCommands)
        {
            if (GameObject.Find(create[0].ToString()) == null){
                Debug.LogWarning(create[0] + " does not exist in this scene!");
                return false;
            }
        }
        return true;
    }

    public void ClearScene()
    {
        isPlayingScene = false;
        Debug.Log("Clearing scene");
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Serializable"))
        {
            Destroy(obj);
        }
    }

    private void SaveStartScene()
    {
        Debug.Log("Saving initial scene");
        SimFileHandler.Handler.SaveGame(Path.Combine(SimFileHandler.savePath, "scene_start.json"));
    }

    private void LoadStartScene()
    {
        Debug.Log("Loading initial scene: " + Path.Combine(SimFileHandler.savePath, "scene_start.json"));
        SimFileHandler.Handler.LoadGame("scene_start.json");
    }

    public void ResetScene()
    {
        isPlayingScene = false;
        Debug.Log("Resetting scene");
        ClearScene();
        LoadStartScene();
    }

    /// <summary>
    /// Set up and play the scene if there is a list of commands
    /// <summary>
    public void PlayScene()
    {
        if (commands == null || commands.Count == 0){
            Debug.LogWarning("No commands recieved");
            return;
        }
        if (!CheckObjectsExist())
        {
            Debug.LogWarning("Cannot play scene with missing objects");
            return;
        }
        waitTime = 0;
        // Get start time of the scene 
        // Time.time goes from when the user starts the program so it won't be 0
        startTime = Time.time;
        isPlayingScene = true;
        StartCoroutine(PlaySceneCoroutine());
    }

    /// <summary>
    /// Coroutine for playing the scene
    /// <summary>
    private IEnumerator PlaySceneCoroutine(){
        foreach (var cmd in commands)
        {
            Debug.Log(cmd[0]);
            if(!isPlayingScene){break;}
            // choose command type
            switch ((string)cmd[0])
            {
                case "CREATE":
                // objects are currently created during file parsing 
                // this is just a placeholder for potentially allowing objects to start existing during the scene
                    break;
                case "DESTROY":
                    //invoke ObjectManager.DestroyObject
                    DestroyCommandReceived?.Invoke((string)cmd[1]);
                    break;
                case "MOVE":
                    break;
                case "SETOBJCELL":
                    //invoke ObjectManager.ChangeObjectProperties
                    SetObjCommandReceived?.Invoke(cmd);
                    break;
                case "TIME":
                    // set time to wait until
                    waitTime = startTime + float.Parse(cmd[1].ToString());
                    break;
                case "DYNUPDATECELL":
                    DynUpdateCommandReceived?.Invoke(cmd);
                    break;
                case "PATH":
                    break;
                default:
                    Debug.LogWarning("Unrecognized command in ScenePlayer");
                    break;  
            }
            // waits until the indicated time to execute next command
            yield return new WaitWhile(() => Time.time < waitTime);
        }
        Debug.Log("Scene complete");
        isPlayingScene = false;
    }


    private void OnObjectsCreated()
    {
        Debug.Log("Objects Created Received");
        isCreatingObjects = false;
    }      
}


