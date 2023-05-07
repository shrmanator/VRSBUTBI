using UnityEngine;
using PathCreation;

/// <summary>
/// Script to attach to an object to move it along a path
/// <summary>
public class PathFollower : MonoBehaviour
{
    // path to move along
    public PathCreator pathCreator;
    // instruction on what to do when the end of the path is reached
    public EndOfPathInstruction end = EndOfPathInstruction.Stop;

    // speed to move
    public float speed = 0;

    // the distance travelled
    float dstTravelled;

    /// <summary>
    /// Called once per frame and updates the object's position along the path
    /// </summary>
    void Update()
    {
        dstTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end);
    }

    /// <summary>
    /// Sets the path for the object to follow
    /// </summary>
    /// <param name="newPath">The path to set</param>
    public void SetPath(PathCreator newPath)
    {
        pathCreator = newPath;
    }

    /// <summary>
    /// Sets the movement of the object along the path
    /// </summary>
    /// <param name="duration">The length of time to move on the path</param>
    /// <param name="startPoint">Optional distance along the path to start from. Defaults to the start of the path</param>
    public void SetMovement(float duration, float startPoint = 0)
    {
        transform.position = pathCreator.path.GetPointAtDistance(startPoint);
        speed = (pathCreator.path.length - startPoint) / duration;
    }
}