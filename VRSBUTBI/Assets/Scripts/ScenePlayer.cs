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
    [SerializeField] public static ScenePlayer Player { get; private set; }

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
    [SerializeField] public List<object[]> commands;
    List<object[]> createCommands;

    // indicates if objects are currently being created
    [SerializeField] bool isCreatingObjects = false;

    // indicates if a scene is playing
    [SerializeField] bool isPlayingScene = false;

    // indicates if a scene is paused
    [SerializeField] public bool isPaused { get; private set; } = false;

    // floats for the timing of commands
    [SerializeField] private float startTime = 0;
    [SerializeField] private float waitTime = 0;

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
    /// <param name="newCreates">The list of CREATE commands to use</param>
    /// <summary>
    public void SetScene(List<object[]> newCommands, List<object[]> newCreates)
    {
        ClearScene();
        commands = newCommands;
        createCommands = newCreates;
        StartCoroutine(SetSceneCoroutine());
        
    }

    public void SetCommands(List<object[]> newCommands)
    {
        commands = newCommands;
        SaveStartScene();
    }

    /// <summary>
    /// The Coroutine for setting a scene. Ensures the initial scene doesn't save until the objects are created
    /// <summary>
    private IEnumerator SetSceneCoroutine()
    {
        isCreatingObjects = true;
        ObjectManager.Manager.CreateObjects(createCommands);
        yield return new WaitWhile(() => isCreatingObjects);
        SaveStartScene();
    }

    /// <summary>
    /// Checks that all objects in a scene exist
    /// <summary>
    public bool CheckObjectsExist()
    {
        foreach (var cmd in commands)
        {
            // ignore TIME commands as they do not have an object
            if (cmd[0].ToString() == "TIME"){continue;}
            if (GameObject.Find(cmd[1].ToString()) == null){
                Debug.LogWarning(cmd[1] + " does not exist in this scene!");
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Checks that all paths in a scene exist
    /// <summary>
    public bool CheckPathsExist()
    {
        foreach (var cmd in commands)
        {
            if (cmd[0].ToString() == "PATH")
            if (GameObject.Find(cmd[2].ToString()) == null)
            {
                Debug.LogWarning(cmd[2] + " does not exist in this scene!");
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Remove all created (has Serializable tag) objects from scene
    /// <summary>
    public void ClearScene()
    {
        Debug.Log("Clearing scene");
        SetDefaultValues();
        ClearObjects();
        PathManager.Manager.ClearPaths();
        PathManager.Manager.ClearWaypoints();
    }

    private void ClearObjects()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Serializable"))
        {
            Destroy(obj);
        }
    }

    /// <summary>
    /// Saves the starting state of the scene
    /// <summary>
    private void SaveStartScene()
    {
        Debug.Log("Saving initial scene");
        if (!isCreatingObjects)
        {
            SimFileHandler.Handler.SaveGame(Path.Combine(SimFileHandler.savePath, "scene_start.json"));
        }
    }

    /// <summary>
    /// Loads the starting state of the scene
    /// <summary>
    private void LoadStartScene()
    {
        Debug.Log("Loading initial scene: " + Path.Combine(SimFileHandler.savePath, "scene_start.json"));
        SimFileHandler.Handler.LoadGame("scene_start.json");
    }

    /// <summary>
    /// Resets the scene to its starting state
    /// <summary>
    public void ResetScene()
    {
        Debug.Log("Resetting scene");
        SetDefaultValues();
        ClearObjects();
        PathManager.Manager.ClearWaypoints();
        LoadStartScene();
    }

    /// <summary>
    /// Pause or unpause if a scene is playing
    /// <summary>
    public void PauseScene()
    {
        // if a scene is playing, pause the scene
        if (!isPaused && isPlayingScene)
        {
            isPaused = true;
            Time.timeScale = 0;
        }
        // unpause the scene
        else
        {
            isPaused = false;
            Time.timeScale = 1;
        }
    }

    /// <summary>
    /// Set up and play the scene if there is a list of commands
    /// <summary>
    public void PlayScene()
    {
        // resume playing scene if paused
        if (isPaused && isPlayingScene)
        {
            PauseScene();
        }
        // play scene if a scene is not playing and objects are not being created
        else if (!isPlayingScene && !isCreatingObjects)
        {
            if (commands == null || commands.Count == 0)
            {
                Debug.LogWarning("No commands recieved");
                return;
            }
            if (!CheckObjectsExist())
            {
                Debug.LogWarning("Cannot play scene with missing objects");
                return;
            }
            if (!CheckPathsExist())
            {
                Debug.LogWarning("Cannot play scene with missing paths");
                return;
            }
            SetDefaultValues();
            // Get start time of the scene 
            // Time.time goes from when the user starts the program so it won't be 0
            startTime = Time.time;
            isPlayingScene = true;

            StartCoroutine(PlaySceneCoroutine());
        }
        // do nothing if a scene is currently playing and not paused
    }


    // Add the following method to ScenePlayer script
    /// <summary>
    /// Checks if a scene is currently playing.
    /// </summary>
    /// <returns>True if a scene is playing, False otherwise.</returns>
    public bool IsScenePlaying()
    {
        return isPlayingScene;
    }

    public bool IsSceneLoaded()
    {
        // Check if there are any commands and createCommands
        return commands != null && commands.Count > 0 && createCommands != null && createCommands.Count > 0;
    }

    /// <summary>
    /// Coroutine for playing the scene
    /// <summary>
    private IEnumerator PlaySceneCoroutine(){
        foreach (var cmd in commands)
        {
            Debug.Log(cmd[0]);
            if(!isPlayingScene)
            {
                //cmd.GetEnumerator().Reset();
                yield break;
            }
            // choose command type
            switch ((string)cmd[0])
            {
                case "CREATE":
                    break;
                case "DESTROY":
                    //invoke ObjectManager.DestroyObject
                    DestroyCommandReceived?.Invoke((string)cmd[1]);
                    break;
                case "MOVE":
                    PathManager.Manager.AssignMovement(cmd);
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
                    PathManager.Manager.AssignPath(cmd);
                    break;
                default:
                    Debug.LogWarning("Unrecognized command in ScenePlayer");
                    break;  
            }
            // waits until the indicated time to execute next command or if the scene is paused
            yield return new WaitWhile(() => Time.time < waitTime || isPaused);
        }
        Debug.Log("Scene complete");
        isPlayingScene = false;
    }

    private void OnObjectsCreated()
    {
        Debug.Log("Objects Created Received");
        isCreatingObjects = false;
        SaveStartScene();
    }

    private void SetDefaultValues()
    {
        isCreatingObjects = false;
        isPaused = false;
        isPlayingScene = false;
        waitTime = 0;
        startTime = 0;
        Time.timeScale = 1;
    }
}

