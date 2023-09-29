using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Animator animator;
    public Slider musicSlider;

    bool isOpen = false;

    AligatorController character = null;

    AligatorController Character {
        get {
            if (character == null) {
                character = FindAnyObjectByType<AligatorController>();
            }

            return character;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (musicSlider != null && ApplicationManager.Instance != null)
            musicSlider.value = ApplicationManager.Instance.MusicVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            if (isOpen) {
                close();
            } else {
                open();
            }
        }
    }

    public void close()
    {
        if (!isOpen)
            return;

        isOpen = false;
        animator.SetBool("IsOpen", false);
        if (Character) {
            Character.enabled = true;
        }
        Time.timeScale = 1.0f;
    }

    public void open()
    {
        if (isOpen)
            return;

        isOpen = true;
        animator.SetBool("IsOpen", true);
        if (Character) {
            Character.enabled = false;
        }
        Time.timeScale = 0.0f;
    }

    public void exitGame()
    {
        if (ApplicationManager.Instance != null) {
            ApplicationManager.Instance.ExitGame();
        }
    }

    public void setMusicVolume(float value)
    {
        if (ApplicationManager.Instance != null) {
            ApplicationManager.Instance.SetMusicVolume(value);
        }
    }
}
