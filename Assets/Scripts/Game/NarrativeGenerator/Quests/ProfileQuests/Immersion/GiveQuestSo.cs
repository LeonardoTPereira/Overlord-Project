using ScriptableObjects;
using Util;
using System;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class GiveQuestSo : ImmersionQuestSo
    {
        public override string SymbolType => Constants.GIVE_QUEST;

        public GiveQuestData GiveQuestData { get; set; }
        private bool _hasItemToCollect;

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
            return !IsCompleted && Id == questId;
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            IsCompleted = true;
        }

        //TODO Check the usage of these methods
        public bool HasItemToCollect(ItemSo itemSo)
        {
            return !_hasItemToCollect && GiveQuestData.ItemToGive.ItemName == itemSo.ItemName;
        }

        public void CollectItem ()
        {
            _hasItemToCollect = true;
        }

        public bool CheckIfCanComplete ()
        {
            return _hasItemToCollect;
        }

        public override string ToString()
        {
            return $"the item {GiveQuestData.ItemToGive.ItemName} to {GiveQuestData.NpcToReceive.NpcName}.\n";
        }
    }
}