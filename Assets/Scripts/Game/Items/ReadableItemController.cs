using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects;

namespace Game
{
    public class ReadableItemController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer itemSprite;

        [SerializeField] private QuestDialogueInteraction _questDialogue;
        private ReadableItemSo itemSo;
        
        public void SetItemInfo ( ReadableItemSo item, int questId )
        {
            _questDialogue.DialogueObj = item;
            _questDialogue.DialogueLine = item.SetRandomText();
            _questDialogue.QuestId = questId;

            itemSprite.sprite = item.sprite;
        }
    }
}
