using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using Game.ExperimentControllers;
using UnityEngine;
using Game.NPCs;
using MyBox;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class CreativityQuestSo : QuestSo
    {
        public override string SymbolType => Constants.CreativityQuest;

        public override Dictionary<string, Func<int,float>> NextSymbolChances
        {
            get
            {
                var creativityQuestWeights = new Dictionary<string, Func<int, float>>
                {
                    {Constants.ExploreQuest, Constants.TwoOptionQuestLineWeight},
                    {Constants.GotoQuest, Constants.TwoOptionQuestLineWeight},
                    {Constants.EmptyQuest, Constants.OneOptionQuestEmptyWeight}
                };
                return creativityQuestWeights;
            } 
        }

        public override QuestSo DefineQuestSo (List<QuestSo> questSos, in GeneratorSettings generatorSettings)
        {
            switch ( SymbolType )
            {
                case Constants.ExploreQuest:
                    return CreateAndSaveExploreQuestSo(questSos, generatorSettings.RoomsToExplore);
                case Constants.GotoQuest:
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

        public override void CreateQuestString()
        {
            throw new NotImplementedException();
        }


        private static ExploreQuestSo CreateAndSaveExploreQuestSo(List<QuestSo> questSos, RangedInt roomsToExplore)
        {
            var exploreQuest = CreateInstance<ExploreQuestSo>();
            var numOfRoomsToExplore = RandomSingleton.GetInstance().Random.Next(roomsToExplore.Max - roomsToExplore.Min) + roomsToExplore.Min;
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
            var gotoQuest = CreateInstance<GotoQuestSo>();
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