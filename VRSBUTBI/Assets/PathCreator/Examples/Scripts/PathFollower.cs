using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 10;
        float distanceTravelled;

        void Start() {
            if (pathCreator != null)
            {
                // AddComponent of PathFollower
                PathCreator pc = gameObject.AddComponent<PathCreator>() as PathCreator;

                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        void FixedUpdate()
        {
            if (pathCreator != null)
            {
                print("in fixedupdate");
                distanceTravelled += speed * Time.deltaTime;
                Vector3 nextPosition = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.MovePosition(nextPosition);
                }
                else
                {
                    transform.position = nextPosition;
                }
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }
        }


        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}