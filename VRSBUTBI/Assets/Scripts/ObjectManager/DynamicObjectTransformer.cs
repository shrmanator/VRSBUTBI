using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObjectTransformer : MonoBehaviour
{
    Vector3 endScale;
    float duration = 0;
    float counter = 0;

    Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if (duration > 0){
            if (counter < duration){
                counter += Time.deltaTime;
                transform.localScale = Vector3.SmoothDamp(transform.localScale, endScale, ref velocity, duration-counter);
            }
            else{
                Destroy(this);
            }
        }
    }

    public void SetTransform(Vector3 newScale, float newDuration){
        endScale = newScale;
        duration = newDuration;
        counter = 0;
        Debug.Log(Time.time);
    }
}
