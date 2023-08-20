using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public string nextLevel = "Level1";

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            this.enabled = false;
            var controller = FindFirstObjectByType<SceneController>();
            if (controller != null) {
                controller.Load(nextLevel);
            } else {
                SceneManager.LoadSceneAsync(nextLevel, LoadSceneMode.Single);
            }
        }
    }
}
