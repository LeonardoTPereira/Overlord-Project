using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fog.Dialogue;
using Game.Dialogues;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.Quests;
using UnityEngine;

#if UNITY_EDITOR
using MyBox;
using UnityEditor;
using Util;
#endif

namespace Game.NPCs
{

    public class NpcController : QuestDialogueInteraction
    {    
        [field: SerializeField] public NpcSo Npc { get; set; }

        protected override void OnEnable()
        {
            base.OnEnable();
            QuestLine.QuestCompletedEventHandler += CreateQuestCompletedDialogue;
        }

        protected override void OnDisable()
        {
            QuestLine.QuestCompletedEventHandler -= CreateQuestCompletedDialogue;
            base.OnDisable();
        }

        protected override void OpenQuest(object sender, NewQuestEventArgs eventArgs)
        {
            base.OpenQuest(sender, eventArgs);
            var quest = eventArgs.Quest;
            var npcInCharge = eventArgs.NpcInCharge;
            CreateQuestOpenedDialogue(quest, npcInCharge);
        }

        protected override bool IsTarget(QuestSo quest)
        {
            var questNpc = GetQuestNpc(quest);
            return questNpc.NpcName == Npc.NpcName;
        }

        private NpcSo GetQuestNpc (QuestSo quest)
        {
            var questNpc = quest switch
            {
                ImmersionQuestSo immersionQuestSo => CheckIfNpcIsTargetOfImmersionQuest(immersionQuestSo),
                AchievementQuestSo achievementQuestSo => CheckIfNpcIsTargetOfAchievementQuest(achievementQuestSo),
                _ => null
            };
            return questNpc;
        }

        private static NpcSo CheckIfNpcIsTargetOfAchievementQuest(AchievementQuestSo achievementQuestSo)
        {
            var questNpc = achievementQuestSo switch
            {
                ExchangeQuestSo exchangeQuestSo => exchangeQuestSo.Npc,
                _ => null
            };

            return questNpc;
        }

        private static NpcSo CheckIfNpcIsTargetOfImmersionQuest(ImmersionQuestSo immersionQuestSo)
        {
            var questNpc = immersionQuestSo switch
            {
                ListenQuestSo listenQuestSo => listenQuestSo.Npc,
                GiveQuestSo giveQuestSo => giveQuestSo.GiveQuestData.NpcToReceive,
                ReportQuestSo reportQuestSo => reportQuestSo.Npc,
                _ => null
            };
            return questNpc;
        }

        private void CreateQuestCompletedDialogue(object sender, NewQuestEventArgs eventArgs)
        {
            if (eventArgs.NpcInCharge != Npc) return;
            var questId = eventArgs.Quest.Id;
            dialogue.StopDialogueFromQuest(questId);
            var closerLine = NpcDialogueGenerator.CreateQuestCloser(eventArgs.Quest, Npc);
            dialogue.AddDialogue(Npc.DialogueData, closerLine, false, questId, true);
        }
        
        private void CreateQuestOpenedDialogue(QuestSo quest, NpcSo npcInCharge)
        {
            if (npcInCharge != Npc) return;
            var openerLine = NpcDialogueGenerator.CreateQuestOpener(quest, Npc);
            var questId = quest.Id;
            dialogue.AddDialogue(Npc.DialogueData, openerLine, true, questId);
        }
        
#if UNITY_EDITOR
        [ButtonMethod]
        public void CreateDialogueAsset()
        {
            var target = "Assets";
            target += Constants.SeparatorCharacter + "Resources";
            target += Constants.SeparatorCharacter + "Dialogues";
            var newFolder = Npc.NpcName;
            if (!AssetDatabase.IsValidFolder(target + Constants.SeparatorCharacter + newFolder))
            {
                AssetDatabase.CreateFolder(target, newFolder);
            }
            target += Constants.SeparatorCharacter + newFolder;
            var fileName = target + Constants.SeparatorCharacter + "Dialogue.asset";
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(fileName);
            Debug.Log(uniquePath);
            AssetDatabase.CreateAsset(dialogue, uniquePath);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
            AssetDatabase.Refresh();
        }
#endif
        
        protected override void CreateIntroDialogue()
        {
            dialogueLine = NpcDialogueGenerator.CreateGreeting(Npc);
            dialogueData = Npc.DialogueData;
            base.CreateIntroDialogue();
        }

        public override void OnInteractAttempt()
        {
            if (_isDialogueNull) return;
            var incompleteQuestQueue = new Queue<QuestSo>();
            while (_assignedQuestsQueue.Count > 0)
            {
                var quest = _assignedQuestsQueue.Dequeue();
                switch (quest)
                {
                    case ExchangeQuestSo exchangeQuest when !quest.IsCompleted:
                        incompleteQuestQueue.Enqueue(quest);
                        continue;
                    case ExchangeQuestSo exchangeQuest:
                        exchangeQuest.TradeItems();
                        break;
                    case GiveQuestSo giveQuest when !quest.IsCompleted:
                        incompleteQuestQueue.Enqueue(quest);
                        continue;
                    case GiveQuestSo giveQuest:
                        giveQuest.GiveItems();
                        break;
                }
                ((IQuestElement)this).OnQuestTaskResolved(this, new QuestTalkEventArgs(Npc, quest.Id));
            }
            _assignedQuestsQueue = incompleteQuestQueue;

            var questsInDialogue = dialogue.GetQuestCloserDialogueIds();
            foreach (var questIds in questsInDialogue)
            {
                ((IQuestElement)this).OnQuestCompleted(this, new QuestElementEventArgs(questIds));
            }
            DialogueHandler.instance.StartDialogue(dialogue);
        }
    }
}