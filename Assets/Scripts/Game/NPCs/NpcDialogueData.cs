using Fog.Dialogue;
using UnityEngine;

namespace Game.Dialogues
{
    [CreateAssetMenu(fileName = "NewDialogueEntity", menuName = "DialogueModule/DialogueData")]
    public class NpcDialogueData : DialogueEntity
    {
        [SerializeField] private Color dialogueColor = Color.white;
        public override Color DialogueColor => dialogueColor;

        [SerializeField] private string dialogueName = "";
        public override string DialogueName => dialogueName;

        [SerializeField] private Sprite dialoguePortrait = null;
        public override Sprite DialoguePortrait => dialoguePortrait;
    }
}