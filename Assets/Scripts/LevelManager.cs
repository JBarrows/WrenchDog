using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject prefabSceneController;

    // Start is called before the first frame update
    void Start()
    {
        var sceneController = FindAnyObjectByType<SceneController>();
        if (!sceneController) {
            Instantiate(prefabSceneController);
        }
    }
}
