using UnityEngine;
using System.Collections;
using PathCreation;

namespace PathCreation
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class ExtendedPathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction = EndOfPathInstruction.Stop;
        public float speed = 10;
        float distanceTravelled;

        Rigidbody rb;

       /* void Start() {
            if (pathCreator != null)
            {
                // AddComponent of PathFollower
                //PathCreator pc = gameObject.AddComponent<PathCreator>() as PathCreator;

                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }

           // rb = GetComponent<Rigidbody>();
        } */

        void FixedUpdate()
        {
            if (pathCreator != null)
            {
                distanceTravelled += speed * Time.deltaTime;
                Vector3 nextPosition = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                if (rb != null)
                {
                    rb.MovePosition(nextPosition);
                }
                else
                {
                    transform.position = nextPosition;
                }

                // Make the object face the direction it's moving
               /* Vector3 direction = pathCreator.path.GetDirectionAtDistance(distanceTravelled, endOfPathInstruction);
                Vector3 normal = pathCreator.path.GetNormalAtDistance(distanceTravelled, endOfPathInstruction);
                if (direction != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(direction, normal);
                    transform.rotation = rotation;
                }*/
            }
        }

        /*public void StartMoving()
        {
            StartCoroutine(MoveToFirstWaypoint());
        }

        IEnumerator MoveToFirstWaypoint() 
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = pathCreator.path.GetPointAtDistance(0);
            float duration = 1.0f; // Change this to control the speed of the movement
            float elapsedTime = 0;

            while (elapsedTime < duration) {
                transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = endPosition;
        }

        void OnPathChanged() {
            StartCoroutine(MoveToFirstWaypoint());
        }*/

        public void SetPath(PathCreator newPath)
        {
            pathCreator = newPath;
        }

        public void SetMovement(float duration, float startPoint = 0)
        {
            transform.position = pathCreator.path.GetPointAtDistance(startPoint);
            speed = (pathCreator.path.length - startPoint) / duration;
        }

        public float getDistanceTraveled()
        {
            return distanceTravelled;
        }
    }
}
