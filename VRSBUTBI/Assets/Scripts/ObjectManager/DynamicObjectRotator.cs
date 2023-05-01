using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObjectRotator : MonoBehaviour
{
    Quaternion endAngle;
    float duration = 0;
    float counter = 0;

    Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if (duration > 0)
        {
            if (counter < duration)
            {
                counter += Time.deltaTime;
                transform.rotation = SmoothDampQuaternion(transform.rotation, endAngle, ref velocity, duration - counter);
            }
            else
            {
                Destroy(this);
            }
        }
    }

    public void SetTransform(Vector3 newAngle, float newDuration)
    {
        endAngle = Quaternion.Euler(newAngle);
        duration = newDuration;
        counter = 0;
        var script = gameObject.GetComponent<DynamicObjectRotator>();
        if (script != null){
            velocity = script.velocity;
        }

        Debug.Log(Time.time);
    }

    public static Quaternion SmoothDampQuaternion(Quaternion current, Quaternion target, ref Vector3 currentVelocity, float smoothTime)
    {
        Vector3 c = current.eulerAngles;
        Vector3 t = target.eulerAngles;
        return Quaternion.Euler(
          Mathf.SmoothDampAngle(c.x, t.x, ref currentVelocity.x, smoothTime),
          Mathf.SmoothDampAngle(c.y, t.y, ref currentVelocity.y, smoothTime),
          Mathf.SmoothDampAngle(c.z, t.z, ref currentVelocity.z, smoothTime)
        );
    }


}
