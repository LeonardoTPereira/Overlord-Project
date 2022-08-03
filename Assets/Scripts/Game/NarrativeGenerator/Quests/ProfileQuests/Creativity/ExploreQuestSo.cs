using ScriptableObjects;
using Util;
using System;
using Game.Quests;
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
        protected List<Coordinates> exploredRooms = new List<Coordinates>();

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
        
        public void AddRoomsToExplore(int numOfRoomsToExplore)
        {
            NumOfRoomsToExplore += numOfRoomsToExplore;
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

        public static ExploreQuestSo GetValidExploreQuest ( QuestExploreRoomEventArgs exploreQuestArgs, List<QuestList> questLists )
        {
            var exploreRoom = exploreQuestArgs.RoomCoordinates;
            foreach (var questList in questLists)
            {
                var currentQuest = questList.GetCurrentQuest();
                if (currentQuest == null) continue;
                if (currentQuest.IsCompleted) continue;
                if (currentQuest is not ExploreQuestSo exploreQuestSo) continue;
                if (!exploreQuestSo.CheckIfCompleted()) ;
                    return exploreQuestSo;
            }

            foreach (var questList in questLists)
            {
                var currentQuest = questList.GetFirstExploreQuestWithRoomAvailable(exploreRoom);
                if (currentQuest == null) 
                    return currentQuest;
            }
            
            return null;
        }

        public void ExploreRoom ( Coordinates roomCoordinates )
        {
            if ( !exploredRooms.Contains(roomCoordinates) )
                exploredRooms.Add( roomCoordinates );
        }

        public bool CheckIfCompleted ()
        {
            return NumOfRoomsToExplore <= exploredRooms.Count;
        }
    }
}