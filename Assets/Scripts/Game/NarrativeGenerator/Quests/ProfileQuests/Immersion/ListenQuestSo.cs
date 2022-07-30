using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class ListenQuestSo : ImmersionQuestSo
    {
        public override string symbolType { 
            get { return Constants.LISTEN_QUEST;} 
        }
        
        //No NPCSo directly. It must be only the job/race, defined using some method based on the next quest
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
            Npc = (copiedQuest as ListenQuestSo).Npc;
        }
        
        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<ListenQuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }
    }
}
