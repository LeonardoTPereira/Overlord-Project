using ScriptableObjects;
using System.Collections.Generic;
using Util;
using System;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class ReadQuestSo : ImmersionQuestSo
    {
        public override string SymbolType => Constants.READ_QUEST;

        public override Dictionary<string, Func<int,int>> NextSymbolChances
        {
            get => _nextSymbolChances;
            set => _nextSymbolChances = value;
        }

        public ItemSo ItemToRead {get; set; }
        public int QuestId { get; set; }

        public override void Init()
        {
            base.Init();
            ItemToRead = null;
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, ItemSo itemToRead)
        {
            base.Init(questName, endsStoryLine, previous);
            ItemToRead = itemToRead;
            QuestId = GetInstanceID();
        }

        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            var readQuest = copiedQuest as ReadQuestSo;
            if (readQuest != null)
            {
                ItemToRead = readQuest.ItemToRead;
            }
            else
            {
                throw new ArgumentException(
                    $"Expected argument of type {typeof(ReadQuestSo)}, got type {copiedQuest.GetType()}");
            }
        }

        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<ReadQuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            return !IsCompleted && QuestId == questId;
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            IsCompleted = true;
        }
        
        public override string ToString()
        {
            return $"{ItemToRead.ItemName}.\n";
        }
    }
}