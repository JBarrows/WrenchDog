using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueSequencer : MonoBehaviour
{
    public UnityEvent<string> OnLineStart;
    public UnityEvent OnSequenceFinished;

    private IEnumerator<string> IDialogue;



    public void PlayDialogue(DialogueAsset dialogue)
    {
        IDialogue = (IEnumerator<string>)dialogue.GetEnumerator();
        StartNextLine();
    }


    public void StartNextLine()
    {
        bool hasNext = IDialogue.MoveNext();

        if (hasNext)
        {
            string line = IDialogue.Current;
            print("Starting line: " + line);
            OnLineStart.Invoke(line);
        }
        else
        {
            OnSequenceFinished.Invoke();
        }
    }

}
