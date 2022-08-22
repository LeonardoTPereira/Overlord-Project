using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class CreativityQuestSo : QuestSo
    {
        public override string SymbolType => Constants.CreativityQuest;

        public override Dictionary<string, Func<int,int>> NextSymbolChances
        {
            get
            {
                var creativityQuestWeights = new Dictionary<string, Func<int, int>>
                {
                    {Constants.EXPLORE_QUEST, Constants.TwoOptionQuestLineWeight},
                    {Constants.GOTO_QUEST, Constants.TwoOptionQuestLineWeight},
                    {Constants.EMPTY_QUEST, Constants.OneOptionQuestEmptyWeight}
                };
                return creativityQuestWeights;
            } 
        }

        public override QuestSo DefineQuestSo ( List<QuestSo> questSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSO possibleItems, WeaponTypeRuntimeSetSO enemyTypes)
        {
            switch ( SymbolType )
            {
                case Constants.EXPLORE_QUEST:
                    return CreateAndSaveExploreQuestSo(questSos);
                case Constants.GOTO_QUEST:
                    return CreateAndSaveGotoQuestSo(questSos);
                default:
                    Debug.LogError("help something went wrong! - Creativity doesn't contain symbol: "+SymbolType);
                break;
            }

            return null;
        }

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            throw new NotImplementedException();
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            throw new NotImplementedException();
        }


        private static ExploreQuestSo CreateAndSaveExploreQuestSo( List<QuestSo> questSos)
        {
            var exploreQuest = CreateInstance<ExploreQuestSo>();
            var numOfRoomsToExplore = RandomSingleton.GetInstance().Random.Next(10) + 3;
            exploreQuest.Init($"Explore {numOfRoomsToExplore} rooms", false, questSos.Count > 0 ? questSos[^1] : null, numOfRoomsToExplore);
            
            if (questSos.Count > 0)
            {
                questSos[^1].Next = exploreQuest;
            }

            questSos.Add(exploreQuest);

            return exploreQuest;
        }

        private static GotoQuestSo CreateAndSaveGotoQuestSo( List<QuestSo> questSos )
        {
            var gotoQuest = ScriptableObject.CreateInstance<GotoQuestSo>();
            //TODO verify if there's a way to mark the room in the minimap/get a rooms name here
            gotoQuest.Init("Go to the marked room", false, questSos.Count > 0 ? questSos[^1] : null);
            if (questSos.Count > 0)
            {
                questSos[^1].Next = gotoQuest;
            }

            questSos.Add(gotoQuest);
            return gotoQuest;
        }
    }
}