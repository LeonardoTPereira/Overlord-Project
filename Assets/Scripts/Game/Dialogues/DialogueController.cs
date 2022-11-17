using System;
using System.Collections.Generic;
using System.Linq;
using Fog.Dialogue;
using Malee.List;
using ScriptableObjects;
using UnityEngine;

namespace Game.Dialogues
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/DialogueController")]
    public class DialogueController : Dialogue
    {
        public static event EventHandler DialogueOpenEventHandler;
        public static event EventHandler DialogueCloseEventHandler;

        private void Awake()
        {
            QuestDialogues = new ReorderableQuestDialogueList();
            NewQuestDialogues = new ReorderableQuestDialogueList();
            lines = new ReorderableDialogueList();
        }

        [field:Reorderable, SerializeField] public ReorderableQuestDialogueList QuestDialogues { get; set; }
        [field:Reorderable, SerializeField] public ReorderableQuestDialogueList NewQuestDialogues { get; set; }
        public override void BeforeDialogue(){
            DialogueOpenEventHandler?.Invoke(null, EventArgs.Empty);
            base.BeforeDialogue();
            DialogueHandler.instance.OnDialogueStart -= BeforeDialogue;
            RecreateDialogueLines();
        }

        public override void AfterDialogue(){
            DialogueCloseEventHandler?.Invoke(null, EventArgs.Empty);
            base.AfterDialogue();
            DialogueHandler.instance.OnDialogueStart -= AfterDialogue;
            RemoveUnrepeatedLines();
        }

        public void AddDialogue(NpcDialogueData dialogueData, string dialogueLine, bool keepDialogueAfterSpoken, int id, 
            bool isQuestCloser = false)
        {
            NewQuestDialogues.Add(new QuestDialogueLine(dialogueData, dialogueLine,keepDialogueAfterSpoken, 
                id, isQuestCloser));
        }

        private void RecreateDialogueLines()
        {
            lines.Clear();
            if (NewQuestDialogues.Count > 0)
            {
                foreach (var dialogue in NewQuestDialogues)
                {
                    QuestDialogues.Add(dialogue);
                }
                NewQuestDialogues.Clear();
            }
            foreach (var dialogues in QuestDialogues)
            {
                lines.Add(dialogues);
            }
        }

        public void StopDialogueFromQuest(int id)
        {
            foreach (var line in QuestDialogues)
            {
                if (line.QuestId == id)
                {
                    line.KeepAfterSpoken = false;
                }
            }
            foreach (var line in NewQuestDialogues)
            {
                if (line.QuestId == id)
                {
                    line.KeepAfterSpoken = false;
                }
            }
        }
        
        private void RemoveUnrepeatedLines()
        {
            for (var i = QuestDialogues.Count-1; i > -1; i--)
            {
                if (!QuestDialogues[i].KeepAfterSpoken)
                {
                    QuestDialogues.RemoveAt(i);
                }
            }
        }
        
        [Serializable]
        public class ReorderableQuestDialogueList : ReorderableArray<QuestDialogueLine> { }

        public IEnumerable<int> GetQuestCloserDialogueIds()
        {
            var closerInDialogues = QuestDialogues.Where(questDialogue => questDialogue.IsQuestCloser).Select(questDialogue => questDialogue.QuestId).ToList();
            var closerInNewDialogues = NewQuestDialogues.Where(questDialogue => questDialogue.IsQuestCloser).Select(questDialogue => questDialogue.QuestId).ToList();
            return closerInDialogues.Concat(closerInNewDialogues);
        }
    }
}