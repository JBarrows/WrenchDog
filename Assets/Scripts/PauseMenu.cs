using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
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
        Application.Quit();
    }

    public void setVolume(float value)
    {
        var audio = FindAnyObjectByType<AudioSource>();
        if (audio)
            audio.volume = value;
    }
}
