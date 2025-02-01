using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects;
using System;

namespace Game
{
    public class ReadableItemController : MonoBehaviour
    {
        public static event EventHandler ReadableItemInteraction;
        [SerializeField] private SpriteRenderer itemSprite;

        [SerializeField] private QuestDialogueInteraction _questDialogue;
        private ReadableItemSo itemSo;

        private void OnEnable()
        {
            _questDialogue.OnQuestDialogueInteractionEventHandler += InvokeReadableItemInteraction;    
        }

        private void OnDisable()
        {
            _questDialogue.OnQuestDialogueInteractionEventHandler -= InvokeReadableItemInteraction;    
        }
        
        public void SetItemInfo ( ReadableItemSo item, int questId )
        {
            _questDialogue.DialogueObj = item;
            _questDialogue.DialogueLine = item.SetRandomText();
            _questDialogue.QuestId = questId;

            itemSprite.sprite = item.sprite;
        }

        public void InvokeReadableItemInteraction(object sender, EventArgs eventArgs)
        {
            Destroy(this.gameObject);
            ReadableItemInteraction?.Invoke( this, eventArgs);
        }
    }
}