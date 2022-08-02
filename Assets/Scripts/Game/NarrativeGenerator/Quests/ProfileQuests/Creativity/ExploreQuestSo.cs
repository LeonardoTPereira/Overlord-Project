using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class ExploreQuestSo : CreativityQuestSo
    {
        public override string symbolType {
            get { return Constants.EXPLORE_QUEST; }
        }

        public int NumOfRoomsToExplore { get; set; }

        public override void Init()
        {
            base.Init();
            NumOfRoomsToExplore = 0;
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, int numOfRoomsToExplore)
        {
            base.Init(questName, endsStoryLine, previous);
            NumOfRoomsToExplore = numOfRoomsToExplore;
        }
        
        public void AddRoom(int numOfRoomsToExplore)
        {
            NumOfRoomsToExplore = numOfRoomsToExplore;
        }
        
        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            NumOfRoomsToExplore = (copiedQuest as ExploreQuestSo).NumOfRoomsToExplore;
        }
        
        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<ExploreQuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }

        public void ExploreRoom ()
        {
            NumOfRoomsToExplore--;
        }

        public bool CheckIfCompleted ()
        {
            return NumOfRoomsToExplore > 0;
        }
    }
}