using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;
using Game.Quests;
using Game.NarrativeGenerator.Quests;

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

        public static ListenQuestSo GetValidListenQuest ( QuestTalkEventArgs talkQuestArgs, List<QuestList> questLists )
        {
            var npc = talkQuestArgs.Npc;
            foreach (var questList in questLists)
            {
                var currentQuest = questList.GetCurrentQuest();
                if (currentQuest == null) continue;
                if (currentQuest.IsCompleted) continue;
                if (currentQuest is not ListenQuestSo listenQuestSo) continue;
                if (listenQuestSo.Npc == npc) return listenQuestSo;
            }

            foreach (var questList in questLists)
            {
                var listenQuestSo = questList.GetFirstListenQuestWithNpc(npc);
                if (listenQuestSo == null) return listenQuestSo;
            }

            return null;
        }
    }
}
