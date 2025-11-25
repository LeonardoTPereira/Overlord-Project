using Util;
using System;
using System.Collections.Generic;
using static Util.Enums;

namespace Overlord.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class ExploreQuestSo : CreativityQuestSo
    {
        public override string SymbolType => Constants.ExploreQuest;

        public override Dictionary<string, Func<int,float>> NextSymbolChances
        {
            get => _nextSymbolChances;
            set => _nextSymbolChances = value;
        }

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
                _exploredRooms = new List<Coordinates>();
                foreach (var roomCoordinate in _exploredRooms)
                {
                    _exploredRooms.Add(roomCoordinate);
                }
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

        public override string GetRoomAmount()
        {
            return NumOfRoomsToExplore.ToString();
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

        public override void CreateQuestString(Language language)
        {
            if (language == Language.Portuguese)
                QuestText = $"Visite {NumOfRoomsToExplore} ou mais salas.\n";
            else if (language == Language.English)
                QuestText = $"Visit a total of {NumOfRoomsToExplore} rooms.\n";
        }

        public void ChangeRoomsPercentageToValue(int roomsCount)
        {
            NumOfRoomsToExplore = NumOfRoomsToExplore * roomsCount / 100 - 1;        
        }
    }
}