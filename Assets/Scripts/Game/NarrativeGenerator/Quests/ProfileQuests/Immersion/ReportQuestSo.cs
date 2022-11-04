using Util;
using System;
using System.Collections.Generic;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class ReportQuestSo : ImmersionQuestSo
    {
        public override string SymbolType => Constants.REPORT_QUEST;

        public override Dictionary<string, Func<int,int>> NextSymbolChances
        {
            get => _nextSymbolChances;
            set => _nextSymbolChances = value;
        }
        
        public NpcSo Npc { get; set; }
        public int QuestId { get; set; }

        public override void Init()
        {
            base.Init();
            Npc = null;
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, NpcSo npc)
        {
            base.Init(questName, endsStoryLine, previous);
            Npc = npc;
            QuestId = GetInstanceID();
        }

        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            var reportQuest = copiedQuest as ReportQuestSo;
            if (reportQuest != null)
            {
                Npc = reportQuest.Npc;
            }
            else
            {
                throw new ArgumentException(
                    $"Expected argument of type {typeof(ReportQuestSo)}, got type {copiedQuest.GetType()}");
            }
        }

        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<ReportQuestSo>();
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
            return $"{Npc.NpcName}.\n";
        }
    }
}