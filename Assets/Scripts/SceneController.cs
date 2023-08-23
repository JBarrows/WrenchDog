using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    static SceneController instance;
    private bool loading = false;
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
        if (loading)
            return;
        
        StartCoroutine(LoadAsync(scene));
    }

    IEnumerator LoadAsync(string scene)
    {
        loading = true;
        var activeScene = SceneManager.GetActiveScene();

        // Load the transition scene
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(transitionSceneName, LoadSceneMode.Single);
        while (!asyncOp.isDone) {
            yield return null;
        }

        // Load the new scene
        asyncOp = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        while (!asyncOp.isDone) {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));

        // Unload the transition
        asyncOp = SceneManager.UnloadSceneAsync(transitionSceneName);
        while (!asyncOp.isDone) {
            yield return null;
        }

        loading = false;
    }

    public void reload()
    {
        if (loading)
            return;

        StartCoroutine(ReloadAsync());
    }

    private IEnumerator ReloadAsync()
    {
        loading = true;
        var activeScene = SceneManager.GetActiveScene();
        var sceneIndex = activeScene.buildIndex;

        // Load the transition scene
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(transitionSceneName, LoadSceneMode.Single);
        while (!asyncOp.isDone) {
            yield return null;
        }

        // Load the new scene
        asyncOp = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        while (!asyncOp.isDone) {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));

        // Unload the transition
        SceneManager.UnloadSceneAsync(transitionSceneName);
        while (!asyncOp.isDone) {
            yield return null;
        }

        loading = false;
    }
}
