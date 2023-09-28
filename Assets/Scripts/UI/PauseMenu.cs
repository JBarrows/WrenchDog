using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenu : MonoBehaviour
{
    public Animator animator;
    public AudioMixer masterMixer;
    public Slider musicSlider;

    static float musicValue = 1.0f;

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
        if (musicSlider != null)
            musicSlider.value = musicValue;
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
    #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    public void setMusicVolume(float value)
    {
        if (!masterMixer)
            return;
        
        musicValue = value;
        var dbValue = Mathf.Log(Mathf.Max(0.001f, musicValue)) * 20;
        masterMixer.SetFloat("masterVol", dbValue);
    }
}
