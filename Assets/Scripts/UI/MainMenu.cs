using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string startGameScene = "";

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

            SceneManager.LoadSceneAsync(startGameScene, LoadSceneMode.Single);
        }
    }
}
