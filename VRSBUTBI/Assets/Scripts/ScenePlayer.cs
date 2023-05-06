using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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

    public delegate void MoveCommandReceivedEventHandler(string objectName3, string pathName, float duration1, float startPosition);
    public static event MoveCommandReceivedEventHandler MoveCommandReceived;

    public delegate void DestroyCommandReceivedEventHandler(string objectName);
    public static event DestroyCommandReceivedEventHandler DestroyCommandReceived;

    public delegate void DynUpdateCommandReceivedEventHandler(object[] data);
    public static event DynUpdateCommandReceivedEventHandler DynUpdateCommandReceived;

    // The list of commands for playing the scene
    List<object[]> commands;

    // floats for the timing of commands
    float startTime;
    float waitTime = 0;

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
    public void SetScene(List<object[]> newCommands)
    {
        Debug.Log("Setting new scene");
        commands = newCommands;
    }

    /// <summary>
    /// Set up and play the scene if there is a list of commands
    /// <summary>
    public void PlayScene()
    {
        Debug.Log("Playing Scene");
        if (commands == null || commands.Count == 0){
            Debug.Log("No commands recieved");
            return;
        }
        waitTime = 0;
        // Get start time of the scene 
        // Time.time goes from when the user starts the program so it won't be 0
        startTime = Time.time;
        Debug.Log("Start time: " + startTime);
        StartCoroutine(PlaySceneCoroutine());
    }

    /// <summary>
    /// Coroutine for playing the scene
    /// <summary>
    private IEnumerator PlaySceneCoroutine(){
        foreach (var cmd in commands)
        {
            Debug.Log(cmd[0]);
            Debug.Log("Current time: " + Time.time);
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
                    UnityEngine.Debug.Log("DYNUPDATECELL command recieved");
                    DynUpdateCommandReceived?.Invoke(cmd);
                    break;
                default:
                    Debug.Log("Unrecognized command in ScenePlayer");
                    break;  
            }
            Debug.Log("Wait until: " + waitTime);
            // waits until the indicated time to execute next command
            yield return new WaitWhile(() => Time.time < waitTime);
        }
    }
        
}
