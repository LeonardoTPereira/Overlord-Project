using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NpcDialogueSO : ScriptableObject
{
    [SerializeField]
    private string npcName;
    [SerializeField, TextArea]
    private string[] dialogues;

    public string[] Dialogues { get => dialogues; set => dialogues = value; }
    public string NpcName { get => npcName; set => npcName = value; }
}
