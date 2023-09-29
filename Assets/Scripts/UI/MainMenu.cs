using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{

    public string startGameScene = "";
    public TMP_Text txtMusicButtonText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (startGameScene == "")
            {
                Debug.LogWarning("Please set the game scene in MainMenu.");
                return;
            }

            var controller = FindFirstObjectByType<SceneController>();
            if (controller != null) {
                controller.Load(startGameScene);
            } else {
                SceneManager.LoadSceneAsync(startGameScene, LoadSceneMode.Single);
            }
        } 
        else if (Input.GetButtonDown("Cancel") && ApplicationManager.Instance != null)
        {
            ApplicationManager.Instance.ExitGame();
        }
    }

    public void ToggleMusic()
    {
        float value;
        string buttonText;
        if (ApplicationManager.Instance.MusicVolume > 0.001f) {
            value = 0.0f;
            buttonText = "<color=#888>Off</color>";
        } else {
            value = 1.0f;
            buttonText = "On";
        }

        if (ApplicationManager.Instance != null)
            ApplicationManager.Instance.SetMusicVolume(value);

        if (txtMusicButtonText != null)
            txtMusicButtonText.text = "MUSIC\n" + buttonText;
    }
}
