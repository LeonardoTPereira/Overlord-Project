using System.Collections.Generic;
using System.Linq;
using Fog.Dialogue;
using Game.Dialogues;
using Game.Events;
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
        public List<ExchangeQuestData> ExchangeDataList { get; set; }
        public static event ItemTradeEvent ItemTradeEventHandler;

        protected override void Awake()
        {
            base.Awake();
            ExchangeDataList = new List<ExchangeQuestData>();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            QuestLine.QuestCompletedEventHandler += CreateQuestCompletedDialogue;
            QuestLine.AllowExchangeEventHandler += CreateExchangeDialogue;
            TaggedDialogueHandler.StartExchangeEventHandler += TradeItems;
        }
        protected override void OnDisable()
        {
            QuestLine.QuestCompletedEventHandler -= CreateQuestCompletedDialogue;
            QuestLine.AllowExchangeEventHandler -= CreateExchangeDialogue;
            TaggedDialogueHandler.StartExchangeEventHandler -= TradeItems;
            base.OnDisable();
        }

        protected override void OpenQuest(object sender, NewQuestEventArgs eventArgs)
        {
            base.OpenQuest(sender, eventArgs);
            var quest = eventArgs.Quest;
            var npcInCharge = eventArgs.NpcInCharge;
            CreateQuestOpenedDialogue(quest, npcInCharge);
        }

        protected override bool IsTarget(QuestSo questSo)
        {
            NpcSo questNpc = GetQuestNpc(questSo);
            return questNpc != null && questNpc.NpcName == Npc.NpcName;
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

        private void CreateExchangeDialogue(object sender, QuestElementEventArgs eventArgs)
        {
            if (eventArgs is not QuestExchangeEventArgs exchangeEventArgs) return;
            var targetNpc = exchangeEventArgs.ExchangeQuestData.Npc;
            if (targetNpc != Npc) return;
            var openerLine = NpcDialogueGenerator.CreateExchangeDialogue(exchangeEventArgs.ExchangeQuestData, Npc);
            ExchangeDataList.Add(new ExchangeQuestData(exchangeEventArgs.ExchangeQuestData.ExchangeData));
            var questId = exchangeEventArgs.ExchangeQuestData.Id;
            dialogue.AddDialogue(Npc.DialogueData, openerLine, false, questId);
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
            DialogueLine = NpcDialogueGenerator.CreateGreeting(Npc);
            DialogueObj = Npc;
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
                    case ExchangeQuestSo exchangeQuest:
                        if (!exchangeQuest.HasItems)
                        {
                            incompleteQuestQueue.Enqueue(quest);
                            continue;
                        }
                        break;
                    case GiveQuestSo giveQuest:
                        if (!giveQuest.HasItemToCollect)
                        {
                            incompleteQuestQueue.Enqueue(quest);
                            continue;
                        }
                        giveQuest.GiveItems();
                        break;
                }
                ((IQuestElement)this).OnQuestTaskResolved(this, new QuestTalkEventArgs(Npc, quest.Id));
            }
            _assignedQuestsQueue = incompleteQuestQueue;

            DialogueHandler.instance.StartDialogue(dialogue);
        }
        
        private void TradeItems(object sender, StartExchangeEventArgs eventArgs)
        {
            foreach (var exchangeQuestData in ExchangeDataList.Where(exchangeQuestData 
                         => eventArgs.ExchangeQuestId == exchangeQuestData.QuestId))
            {
                ItemTradeEventHandler?.Invoke(this, new ItemTradeEventArgs(exchangeQuestData.CopyOfItemsToTrade, 
                    exchangeQuestData.ReceivedItem, exchangeQuestData.QuestId));
            }
        }
    }
}