using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class ReportQuestSo : ImmersionQuestSo
    {
        public override string symbolType {
            get { return Constants.REPORT_QUEST; }
        }
        public NpcSo Npc { get; set; }

        public override void Init()
        {
            base.Init();
            Npc = null;
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, NpcSo npc)
        {
            base.Init(questName, endsStoryLine, previous);
            Npc = npc;
        }
        
        public void AddNpc(NpcSo npc)
        {
            Npc = npc;
        }
        
        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            Npc = (copiedQuest as ReportQuestSo).Npc;
        }
        
        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<ReportQuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }
    }
}