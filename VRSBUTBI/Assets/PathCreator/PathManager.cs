using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PathCreation;
/// <summary>
/// Singleton class that manages paths
/// <summary>
public class PathManager : MonoBehaviour
{
    // Singleton instance
    public static PathManager Manager { get; private set; }

    public GameObject waypoint;

    private bool isCreatingPath = false;

    private int numPaths = 0;

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

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isCreatingPath)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Define an offset to raise the waypoint above the terrain
                float yOffset = 5f; // Change this value to whatever offset you want

                // Add the offset to the hit point's y-coordinate
                Vector3 waypointPosition = hit.point + new Vector3(0, yOffset, 0);

                // Instantiate a waypoint at the adjusted position
                Instantiate(waypoint, waypointPosition, Quaternion.identity);

                //Tag so we can find the waypoints
                waypoint.tag = "New Waypoint";
            }
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
        ExtendedPathFollower script = obj.GetComponent<ExtendedPathFollower>();
        if (script == null)
        {
            script = obj.AddComponent(typeof(ExtendedPathFollower)) as ExtendedPathFollower;
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

        // Get PathFollower component
        ExtendedPathFollower script = obj.GetComponent<ExtendedPathFollower>();
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

        // make the object face the direction it's moving
        obj.transform.forward = script.pathCreator.path.GetDirectionAtDistance(script.getDistanceTraveled());
    }


    public void StartCreatingPath(){
        isCreatingPath = true;
    }

    public void SavePath()
    {
        isCreatingPath = false;
        GeneratePath(); 

    }

    public bool IsCreatingPath(){
        return isCreatingPath;
    }

    private void GeneratePath()
    {
        // Get a list of points
        var waypoints = GameObject.FindGameObjectsWithTag("New Waypoint");
        List<Vector3> coordinates = new List<Vector3>(); 
        foreach (var point in waypoints)
        {
            coordinates.Add(point.transform.position);
        }
        GeneratePathFromVertices(coordinates);
        
        //Remove waypoints as they're no longer needed and create clutter
        ClearWaypoints();
    }

    // Clears all waypoints
    public void ClearWaypoints()
    {
        var waypoints = GameObject.FindGameObjectsWithTag("New Waypoint");
        foreach(var point in waypoints)
        {
            Destroy(point);
        }
    }

    public void CancelPath()
    {
        isCreatingPath = false;
        ClearWaypoints();
    }

    public GameObject[] GetAllPaths()
    {
        return GameObject.FindGameObjectsWithTag("Path");
    }

    public void ClearPaths()
    {
        var paths = GetAllPaths();
        foreach (var path in paths)
        {
            Destroy(path);
        }
        numPaths = 0;
    }

    public void GeneratePathFromVertices(List<Vector3> vertices)
    {
        numPaths++;
        //Create our path
        GameObject pathObject = new GameObject("Path" + numPaths);
        //Attach PathCreator to give it Path functionallity
        var pathCreator = pathObject.AddComponent<PathCreator>();
        //Define the path from the list of points
        pathCreator.bezierPath = new BezierPath(vertices);
        pathCreator.enabled = true;
        //Add line to path
        var line = pathObject.AddComponent<LineRenderer>();
        //LineRender.SetPositions only sets positions up to positionCount
        line.positionCount = vertices.Count;
        line.SetPositions(vertices.ToArray());
        pathObject.tag = "Path";
    }
}
