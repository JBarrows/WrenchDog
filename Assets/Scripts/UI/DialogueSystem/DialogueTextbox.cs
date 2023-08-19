using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTextbox : MonoBehaviour
{

    private Animator boxAnimator;

    // Start is called before the first frame update
    void Start()
    {
        boxAnimator = GetComponent<Animator>();
    }


    public void OnLineStart(string _line)
    {
        boxAnimator.SetBool("isOpen", true);
    }

    public void OnSequenceFinished()
    {
        boxAnimator.SetBool("isOpen", false);
    }
}
