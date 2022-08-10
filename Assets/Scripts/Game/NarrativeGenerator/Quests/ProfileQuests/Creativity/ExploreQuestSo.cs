using Util;
using System;
using System.Collections.Generic;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class ExploreQuestSo : CreativityQuestSo
    {
        public override string SymbolType => Constants.EXPLORE_QUEST;

        public int NumOfRoomsToExplore { get; set; }
        private List<Coordinates> _exploredRooms;

        public override void Init()
        {
            base.Init();
            _exploredRooms = new List<Coordinates>();
            NumOfRoomsToExplore = 0;
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, int numOfRoomsToExplore)
        {
            base.Init(questName, endsStoryLine, previous);
            _exploredRooms = new List<Coordinates>();
            NumOfRoomsToExplore = numOfRoomsToExplore;
        }

        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            var exploreQuestSo = copiedQuest as ExploreQuestSo;
            if (exploreQuestSo != null)
            {
                NumOfRoomsToExplore = exploreQuestSo.NumOfRoomsToExplore;
            }
            else
            {
                throw new ArgumentException(
                    $"Expected argument of type {typeof(ExploreQuestSo)}, got type {copiedQuest.GetType()}");
            }
        }
        
        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<ExploreQuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            return !IsCompleted
                   && !_exploredRooms.Contains(questElement as Coordinates ?? throw new InvalidOperationException());
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            _exploredRooms.Add(questElement as Coordinates ?? throw new InvalidOperationException());
            if (_exploredRooms.Count == NumOfRoomsToExplore)
            {
                IsCompleted = true;
            }
        }

        public override string ToString()
        {
            return $"Visit a total of {NumOfRoomsToExplore} rooms.\n";
        }
    }
}