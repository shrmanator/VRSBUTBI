using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

/// <summary>
/// Singleton class that manages paths
/// <summary>
public class PathManager : MonoBehaviour
{
    // Singleton instance
    public static PathManager Manager { get; private set; }

    // Delete any other instances if they exist
    void Awake()
    {
        if (Manager != null && Manager != this)
        {
            Destroy(this);
        }
        else
        {
            Manager = this;
        }
    }
    
    // Subscribes to PATH and MOVE commands
    void Start()
    {
        ScenePlayer.PathCommandReceived += AssignPath;
        ScenePlayer.MoveCommandReceived += AssignMovement;
    }

    /// <summary>
    /// Assigns an object to a path
    /// </summary>
    /// <param name="data">Expected to be ["PATH", object name, path name]</param>
    public void AssignPath(object[] data)
    {
        // Get object by name
        GameObject obj = GameObject.Find((string)data[1]);
        if (obj == null)
        {
            UnityEngine.Debug.Log(data[1] + " not found!");
            return;
        }

        // Get path by name
        GameObject pathObj = GameObject.Find((string)data[2]);
        if (pathObj == null)
        {
            UnityEngine.Debug.Log(data[2] + " not found!");
            return;
        }

        // Get PathCreator componant from path
        PathCreator path = pathObj.GetComponent<PathCreator>();
        if (path == null)
        {
            UnityEngine.Debug.Log(data[2] + " is not a valid path!");
            return;
        }
        // Get existing PathFollower script on object or add one
        PathFollower script = obj.GetComponent<PathFollower>();
        if (script == null)
        {
            script = obj.AddComponent(typeof(PathFollower)) as PathFollower;
        }
   
        script.SetPath(path);
    }

    /// <summary>
    /// Assigns movement along a path to an object
    /// </summary>
    /// <param name="data">Expected to be ["MOVE", object name, duration, (optional) starting distance on path]</param>
    public void AssignMovement(object[] data)
    {
        // Find object by name
        GameObject obj = GameObject.Find((string)data[1]);
        if (obj == null)
        {
            UnityEngine.Debug.Log(data[1] + " not found!");
            return;
        }

        // Get PathFollower componant
        PathFollower script = obj.GetComponent<PathFollower>();
        if (script == null)
        {
            UnityEngine.Debug.Log(data[1] + " is not on a path!");
        }

        // Set movement without optional parameter
        if (data.Length < 4){
            script.SetMovement(float.Parse(data[2].ToString()));
        }
        // Set movement with optional parameter
        else {
            script.SetMovement(float.Parse(data[2].ToString()), float.Parse(data[3].ToString()));
        }
    }
}