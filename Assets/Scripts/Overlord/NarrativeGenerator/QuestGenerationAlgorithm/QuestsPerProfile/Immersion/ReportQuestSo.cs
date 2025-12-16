using Util;
using System;
using System.Collections.Generic;
using static Util.Enums;
using Overlord.NarrativeGenerator.NPCs;

namespace Overlord.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class ReportQuestSo : ImmersionQuestSo
    {
        public override string SymbolType => Constants.ReportQuest;

        public override Dictionary<string, Func<int,float>> NextSymbolChances
        {
            get => _nextSymbolChances;
            set => _nextSymbolChances = value;
        }
        
        public NpcSo Npc { get; set; }
        public bool HasCreatedDialogue { get; set; }

        public override void Init()
        {
            base.Init();
            Npc = null;
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, NpcSo npc)
        {
            base.Init(questName, endsStoryLine, previous);
            Npc = npc;
            HasCreatedDialogue = false;
        }

        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            var reportQuest = copiedQuest as ReportQuestSo;
            if (reportQuest != null)
            {
                Npc = reportQuest.Npc;
                HasCreatedDialogue = reportQuest.HasCreatedDialogue;
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
        
        public override string GetTargetNpc()
        {
            return Npc.NpcName;
        }

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            return !IsCompleted && Id == questId;
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            IsCompleted = true;
        }

        public override void CreateQuestString(Language language)
        {
            if (language == Language.Portuguese)
            {
                QuestText = $"Reporte para {Npc.NpcName}.\n";
                return;
            }
            QuestText = $"Report to {Npc.NpcName}.\n";
        }
    }
}