using ScriptableObjects;
using Util;
using System.Collections.Generic;
using System;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class GiveQuestSo : ImmersionQuestSo
    {
        public override string SymbolType => Constants.GiveQuest;

        public override Dictionary<string, Func<int,float>> NextSymbolChances
        {
            get => _nextSymbolChances;
            set => _nextSymbolChances = value;
        }

        public GiveQuestData GiveQuestData { get; set; }

        public bool HasItem { get; private set; }
        public bool HasCreatedDialogue { get; set; }        

        public override void Init()
        {
            base.Init();
            HasItem = false;
            HasCreatedDialogue = false;
            GiveQuestData = new GiveQuestData();
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, NpcSo npc, ItemSo item)
        {
            base.Init(questName, endsStoryLine, previous);
            HasItem = false;
            HasCreatedDialogue = false;
            GiveQuestData = new GiveQuestData(npc, item, Id);
        }

        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            var giveQuest = copiedQuest as GiveQuestSo;
            if (giveQuest != null)
            {
                var npcToReceive = giveQuest.GiveQuestData.NpcToReceive;
                var itemToGive = giveQuest.GiveQuestData.ItemToGive;
                GiveQuestData = new GiveQuestData(npcToReceive, itemToGive, Id);
                HasItem = giveQuest.HasItem;
                HasCreatedDialogue = giveQuest.HasCreatedDialogue;
            }
            else
            {
                throw new ArgumentException(
                    $"Expected argument of type {typeof(GiveQuestSo)}, got type {copiedQuest.GetType()}");
            }
        }
        
        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<GiveQuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            if (questId != Id) return false;
            return questElement switch
            {
                ItemSo itemSo => !IsCompleted && !HasItem && GiveQuestData.ItemToGive.ItemName == itemSo.ItemName,
                NpcSo npcSo => !IsCompleted && HasItem && npcSo == GiveQuestData.NpcToReceive,
                _ => false
            };
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            if (HasItem)
            {
                IsCompleted = true;
                return;
            }
            HasItem = true;
        }

        public override void CreateQuestString()
        {
            var spriteString = GiveQuestData.ItemToGive.GetToolSpriteString();
            QuestText = $"the item {GiveQuestData.ItemToGive.ItemName} {spriteString} to {GiveQuestData.NpcToReceive.NpcName}.\n";
        }
    }
}