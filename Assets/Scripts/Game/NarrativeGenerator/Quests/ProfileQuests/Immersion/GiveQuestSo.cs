using ScriptableObjects;
using Util;
using System.Collections.Generic;
using System;
using Game.Events;
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
        private bool _hasItemToCollect;
        public static event TreasureCollectEvent TreasureLostEventHandler;

        public override void Init()
        {
            base.Init();
            GiveQuestData = new GiveQuestData();
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, NpcSo npc, ItemSo item)
        {
            base.Init(questName, endsStoryLine, previous);
            GiveQuestData = new GiveQuestData(npc, item);
        }

        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            var giveQuest = copiedQuest as GiveQuestSo;
            if (giveQuest != null)
            {
                var npcToReceive = giveQuest.GiveQuestData.NpcToReceive;
                var itemToGive = giveQuest.GiveQuestData.ItemToGive;
                GiveQuestData = new GiveQuestData(npcToReceive, itemToGive);
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
            return questElement switch
            {
                ItemSo itemSo => !IsCompleted && !_hasItemToCollect && GiveQuestData.ItemToGive.ItemName == itemSo.ItemName,
                NpcSo npcSo => !IsCompleted && _hasItemToCollect && npcSo == Npc,
                _ => false
            };
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            IsCompleted = true;
            _hasItemToCollect = true;
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            IsCompleted = true;
        }

        public override void CreateQuestString()
        {
            var spriteString = GiveQuestData.ItemToGive.GetToolSpriteString();
            QuestText = $"the item {GiveQuestData.ItemToGive.ItemName} {spriteString} to {GiveQuestData.NpcToReceive.NpcName}.\n";
        }

        public void GiveItems()
        {
            TreasureLostEventHandler?.Invoke(this, new TreasureCollectEventArgs(GiveQuestData.ItemToGive, Id));
        }
    }
}