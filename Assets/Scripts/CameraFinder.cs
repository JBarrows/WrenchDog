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
            canvas.enabled = true;
            canvas.worldCamera = Camera.main;
        }
    }

    private void Update() {
        if (!canvas) {
            canvas = GetComponent<Canvas>();
            canvas.enabled = true;
        }

        if (canvas && canvas.worldCamera == null) {
            canvas.worldCamera = Camera.main;
        }
    }
}
