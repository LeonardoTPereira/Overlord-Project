using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class GiveQuestSo : ImmersionQuestSo
    {
        public override string symbolType {
            get { return Constants.GIVE_QUEST; }
        }

        public NpcSo Npc { get; set; }
        public ItemSo Item {get; set; }

        public override void Init()
        {
            base.Init();
            Npc = null;
            Item = null;
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, NpcSo npc, ItemSo item)
        {
            base.Init(questName, endsStoryLine, previous);
            Npc = npc;
            Item = item;
        }
        
        public void AddNpc( NpcSo npc, ItemSo item )
        {
            Npc = npc;
            Item = item;
        }
        
        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            Npc = (copiedQuest as GiveQuestSo).Npc;
            Item = (copiedQuest as GiveQuestSo).Item;
        }
        
        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<GiveQuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }
    }
}