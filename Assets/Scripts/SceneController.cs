using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    static SceneController instance;
    public string transitionSceneName = "Transition";

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this);
        }
    }

    public void Load(string scene)
    {
        StartCoroutine(LoadAsync(scene));
    }

    IEnumerator LoadAsync(string scene)
    {
        var activeScene = SceneManager.GetActiveScene();

        // Load the transition scene
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(transitionSceneName, LoadSceneMode.Single);
        while (!asyncOp.isDone) {
            yield return null;
        }

        yield return new WaitForSeconds(0.4f);

        // Load the new scene
        var loadNew = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        while (!loadNew.isDone) {
            yield return null;
        }
        Debug.Log("New Scene Loaded");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));

        // Unload the transition
        SceneManager.UnloadSceneAsync(transitionSceneName);
    }
}
