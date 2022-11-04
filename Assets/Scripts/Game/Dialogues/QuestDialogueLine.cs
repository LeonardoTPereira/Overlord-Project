using System.Transactions;
using Fog.Dialogue;
using Malee.List;
using UnityEngine;

namespace Game.Dialogues
{
    public class QuestDialogueLine : DialogueLine
    {
        [field: SerializeField] public bool KeepAfterSpoken { get; set; }
        [field: SerializeField] public int DialogueId { get; private set; }
        
        public QuestDialogueLine(DialogueEntity speaker, string text, bool keepAfterSpoken, int id) : base(speaker, text)
        {
            KeepAfterSpoken = keepAfterSpoken;
            DialogueId = id;
        }
    }
}