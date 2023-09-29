using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ApplicationManager : MonoBehaviour
{    
    [SerializeField] private AudioMixer musicMixer;
    float musicValue = 1.0f;

    public static ApplicationManager Instance {
        get;
        private set;
    }

    public float MusicVolume { get => musicValue; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SetMusicVolume(float value)
    {
        musicValue = value;

        if (!musicMixer)
            return;
        
        var dbValue = Mathf.Log(Mathf.Max(0.001f, musicValue)) * 20;
        musicMixer.SetFloat("masterVol", dbValue);
    }
}
