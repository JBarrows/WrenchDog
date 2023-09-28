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

    // Start is called before the first frame update
    void Start()
    {
        if (musicSlider != null)
            musicSlider.value = musicValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void close()
    {
        animator.SetBool("IsOpen", false);
    }

    public void open()
    {
        animator.SetBool("IsOpen", true);
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
