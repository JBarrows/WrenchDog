using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueText : MonoBehaviour
{
    public UnityEvent onAdvance;

    private TextMeshProUGUI textRenderer;

    public float charactersPerSecond = 45.0f;
    private float currentCharacter = 0f;
    private int textLength = 0;

    [Range(0.1f, 10.0f)] public float autoAdvanceDelay = 1.0f;
    private float autoAdvanceT = 0.0f;

    private bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        textRenderer = GetComponent<TextMeshProUGUI>();
        textRenderer.SetText("");
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            UpdateText();
        }
    }

    private void UpdateText()
    {
        if(currentCharacter < textLength)
        {
            //Type characters
            currentCharacter += Time.deltaTime * charactersPerSecond;
            textRenderer.maxVisibleCharacters = Mathf.FloorToInt(currentCharacter);
            if (currentCharacter >= textLength) {
                autoAdvanceT = 0.0f;
            }
        }
        else
        {
            if(autoAdvanceT < autoAdvanceDelay)
            {
                autoAdvanceT += Time.deltaTime;
                if(autoAdvanceT >= autoAdvanceDelay)
                {
                    onAdvance.Invoke();
                }
            }
        }
    }

    public void OnLineStart(string line)
    {
        isActive = true;
        autoAdvanceT = 0.0f;
        textRenderer.SetText(line);
        textRenderer.maxVisibleCharacters = 0;
        currentCharacter = 0f;
        textLength = line.Length;
    }

    public void OnSequenceFinished()
    {
        isActive = false;
        textRenderer.SetText("");
    }
}
