using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueSystem : MonoBehaviour
{
    //Event Callbacks
    public UnityEvent onDialogueEnd;
    public static UnityEvent OnDialogueEnd { get { return Instance.onDialogueEnd; } }

    const string PREFAB_PATH = "Assets/Prefabs/DialogueSystem/DialogueCanvas.prefab";

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

    public void PlayDialogue(DialogueAsset dialogue)
    {
        if (!Instance)
        {
            GameObject obj = (Resources.Load(PREFAB_PATH) as GameObject);
            Instantiate(obj);
        }
        Instance.gameObject.SetActive(true);
        StartCoroutine(WaitForTransition(dialogue));
    }

    public IEnumerator WaitForTransition(DialogueAsset dialogue)
    {
        yield return new WaitForSeconds(0.5f);

        
        Instance.sequencer.PlayDialogue(dialogue);
    }



    private static void OnFinish()
    {
        Instance.onDialogueEnd.Invoke();
    }
}
