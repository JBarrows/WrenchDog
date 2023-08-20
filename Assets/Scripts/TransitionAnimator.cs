using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionAnimator : MonoBehaviour
{
    float startTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > 0.1) {
            transform.Rotate(0f, 0f, 90f);
            startTime = Time.time;
        }
    }
}
