using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject prefabAppController;
    [SerializeField] GameObject prefabSceneController;

    // Start is called before the first frame update
    void Start()
    {
        if (ApplicationManager.Instance == null) {
            // ApplicationManager is expected to assign itself as Instance on creation, so no need to assign it
            Instantiate(prefabAppController);
        }

        var sceneController = FindAnyObjectByType<SceneController>();
        if (!sceneController) {
            Instantiate(prefabSceneController);
        }
    }
}
