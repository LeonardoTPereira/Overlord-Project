using Fog.Dialogue;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewDialogueEntity", menuName = "DialogueModule/DialogueData")]
    public class NpcDialogueData : DialogueEntity
    {
        [SerializeField] private Color dialogueColor = Color.white;
        public override Color DialogueColor => NpcDialogueColor;

        [SerializeField] private string dialogueName = "";
        public override string DialogueName => NpcDialogueName;

        [SerializeField] private Sprite dialoguePortrait;
        public override Sprite DialoguePortrait => NpcDialoguePortrait;
        public Color NpcDialogueColor
        {
            get => dialogueColor;
            set => dialogueColor = value;
        }
        public string NpcDialogueName
        {
            get => dialogueName;
            set => dialogueName = value;
        }
        public Sprite NpcDialoguePortrait
        {
            get => dialoguePortrait;
            set => dialoguePortrait = value;
        }
    }
}