using Util;
using System.Collections.Generic;
using System;
using System.Linq;
using Game.LevelGenerator.LevelSOs;
using Game.LevelManager.DungeonLoader;
using MyBox;
using UnityEngine;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class GotoQuestSo : CreativityQuestSo
    {
        public override string SymbolType => Constants.GotoQuest;
        public Coordinates SelectedRoomCoordinates { get; set; }

        public override Dictionary<string, Func<int,float>> NextSymbolChances
        {
            get => _nextSymbolChances;
            set => _nextSymbolChances = value;
        }

        public override void Init()
        {
            base.Init();
            SelectedRoomCoordinates = null;
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, Coordinates selectedRoomCoordinates)
        {
            base.Init(questName, endsStoryLine, previous);
            SelectedRoomCoordinates = selectedRoomCoordinates;
        }

        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            var goToQuestSo = copiedQuest as GotoQuestSo;
            if (goToQuestSo != null)
            {
                SelectedRoomCoordinates = goToQuestSo.SelectedRoomCoordinates;
            }
            else
            {
                throw new ArgumentException(
                    $"Expected argument of type {typeof(GotoQuestSo)}, got type {copiedQuest.GetType()}");
            }
        }
        
        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<GotoQuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            return !IsCompleted 
                   && (questElement as Coordinates 
                       ?? throw new InvalidOperationException()).Equals(SelectedRoomCoordinates);
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            IsCompleted = true;
        }

        public override void CreateQuestString()
        {
            QuestText = $"$Go to the room highlighted in the map! <goto={SelectedRoomCoordinates.X},{SelectedRoomCoordinates.Y}>";
        }

        public void SelectRoomCoordinates(List<DungeonRoomData> partsList)
        {
            var leafList = partsList.Where(room => room.IsLeaf).ToList();
            if (!leafList.Any())
            {
                leafList = partsList.Where(room => room.Type != Constants.RoomTypeString.Corridor).ToList();
            }
            SelectedRoomCoordinates = leafList.GetRandom().Coordinates;
        }
    }
}