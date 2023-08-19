using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueSystem : MonoBehaviour
{
    //Event Callbacks
    public UnityEvent onDialogueEnd;
    public static UnityEvent OnDialogueEnd { get { return Instance.onDialogueEnd; } }

    //Singleton pattern
    private static DialogueSystem _instance;
    public static DialogueSystem Instance { get { return _instance; } }

    private DialogueSequencer sequencer;


    public DialogueAsset testDialogue;


    public DialogueSystem()
    {
        _instance = this;
    }

    public void Start()
    {
        sequencer = GetComponent<DialogueSequencer>();
        gameObject.SetActive(false);
        if (testDialogue)
        {
            PlayDialogue(testDialogue);
        }
    }

    public static void PlayDialogue(DialogueAsset dialogue)
    {
        Instance.gameObject.SetActive(true);
        Instance.sequencer.PlayDialogue(dialogue);
    }



    private static void OnFinish()
    {
        Instance.onDialogueEnd.Invoke();
    }
}
