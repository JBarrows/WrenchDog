using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/DialogueAsset", order = 1)]
public class DialogueAsset : ScriptableObject, IEnumerable
{

    [SerializeField, Multiline] private List<string> lines = new List<string>();

    public IEnumerator GetEnumerator()
    {
        return ((IEnumerable)lines).GetEnumerator();
    }
}
