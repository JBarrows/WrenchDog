using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFinder : MonoBehaviour
{
    public Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        if (!canvas) {
            canvas = GetComponent<Canvas>();
        }

        if (canvas != null) {
            canvas.worldCamera = Camera.main;
        }
    }
}
