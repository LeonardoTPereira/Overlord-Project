using System;
using System.Linq;
using Fog.Dialogue;
using Game.NPCs;
using Malee.List;
using UnityEngine;

namespace Game.Dialogues
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/DialogueController")]
    public class DialogueController : Dialogue
    {
        public static event EventHandler DialogueOpenEventHandler;
        public static event EventHandler DialogueCloseEventHandler;
        
        [field:Reorderable, SerializeField] public ReorderableQuestDialogueList QuestDialogues { get; set; }
        public override void BeforeDialogue(){
            DialogueOpenEventHandler?.Invoke(null, EventArgs.Empty);
            base.BeforeDialogue();
            DialogueHandler.instance.OnDialogueStart -= BeforeDialogue;
        }

        public override void AfterDialogue(){
            DialogueCloseEventHandler?.Invoke(null, EventArgs.Empty);
            base.AfterDialogue();
            DialogueHandler.instance.OnDialogueStart -= AfterDialogue;
            RemoveUnrepeatedLines();
        }

        public void AddDialogue(NpcSo speaker, string dialogueLine, bool keepDialogueAfterSpoken, int id)
        {
            QuestDialogues ??= new ReorderableQuestDialogueList();
            QuestDialogues.Add(new QuestDialogueLine(speaker.DialogueData, dialogueLine,keepDialogueAfterSpoken, id));
            RecreateDialogueLines();
        }

        private void RecreateDialogueLines()
        {
            lines ??= new ReorderableDialogueList();
            lines.Clear();
            foreach (var dialogues in QuestDialogues)
            {
                lines.Add(dialogues);
            }        
        }

        public void StopDialogueFromQuest(int id)
        {
            Debug.Log("Stopped quest: " + id);
            foreach (var line in QuestDialogues)
            {
                if (line.DialogueId == id)
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
            RecreateDialogueLines();
        }
        
        [Serializable]
        public class ReorderableQuestDialogueList : ReorderableArray<QuestDialogueLine> { }
    }
}