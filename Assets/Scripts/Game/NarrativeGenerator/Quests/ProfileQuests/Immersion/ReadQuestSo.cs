using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class ReadQuestSo : ImmersionQuestSo
    {
        public override string symbolType {
            get { return Constants.READ_QUEST; }
        }

        public ItemSo ItemToRead;

        public override void Init()
        {
            base.Init();
            ItemToRead = null;
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, ItemSo itemToRead)
        {
            base.Init(questName, endsStoryLine, previous);
            this.ItemToRead = itemToRead;
        }
        
        public void AddReadableItem(ItemSo itemToRead)
        {
            ItemToRead = itemToRead;
        }
        
        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            ItemToRead = (copiedQuest as ReadQuestSo).ItemToRead;
        }
        
        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<ReadQuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }
    }
}