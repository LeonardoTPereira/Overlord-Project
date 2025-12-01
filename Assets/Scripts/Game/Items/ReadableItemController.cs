using Fog.Dialogue;
using Overlord.NarrativeGenerator;
using ScriptableObjects;
using System;
using Topdown.Overlord.NarrativeGenerator;
using UnityEngine;
using static Util.Enums;

namespace Game
{
    public class ReadableItemController : MonoBehaviour
    {
        public static event EventHandler ReadableItemInteraction;
        [SerializeField] private SpriteRenderer itemSprite;

        [SerializeField] private QuestDialogueInteraction _questDialogue;
        private ReadableItemSo itemSo;
        private Language _language;

        private void Awake()
        {
            var questGeneratorManager = FindObjectOfType<QuestGeneratorManager>();
            if (questGeneratorManager == null)
                questGeneratorManager = FindObjectOfType<TopdownQuestGeneratorManager>();
            _language = questGeneratorManager.language;
        }

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
            _questDialogue.DialogueLine = item.SetRandomText(_language);
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