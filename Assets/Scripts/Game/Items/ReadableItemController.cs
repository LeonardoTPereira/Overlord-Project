using ScriptableObjects;
using System;
using Fog.Dialogue;
using Game.GameManager;
using UnityEngine;

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

        public void SetItemInfo(ReadableItemSo item, int questId)
        {
            _questDialogue.DialogueObj = item;
            _questDialogue.DialogueLine = item.SetRandomText(GameManagerSingleton.Instance.IsInPortuguese);
            _questDialogue.QuestId = questId;

            itemSprite.sprite = item.sprite;
        }

        public void InvokeReadableItemInteraction(object sender, EventArgs eventArgs)
        {
            _questDialogue.OnQuestDialogueInteractionEventHandler -= InvokeReadableItemInteraction;   
            ReadableItemInteraction?.Invoke(this, eventArgs);
            DialogueHandler.instance.OnDialogueEnd += DestroyOnDialogueEnd;
        }

        public void DestroyOnDialogueEnd()
        {
            DialogueHandler.instance.OnDialogueEnd -= DestroyOnDialogueEnd;
            Destroy(this.gameObject);
        }
    }
}