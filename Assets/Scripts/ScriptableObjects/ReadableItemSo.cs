using System;
using System.Text;
using UnityEngine;
using Fog.Dialogue;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ReadableItemSO", menuName = "Items/Readable Item"), Serializable]
    public class ReadableItemSo : ItemSo, IDialogueObjSo
    {
        [SerializeField] private NpcDialogueData dialogueData;

        public NpcDialogueData DialogueData
        {
            get => dialogueData;
            set => dialogueData = value;
        }

        public string GetScrollSpriteString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"<sprite=\"Scrolls\" name=\"{ItemName}\">");
            return stringBuilder.ToString();
        }
    }
}
