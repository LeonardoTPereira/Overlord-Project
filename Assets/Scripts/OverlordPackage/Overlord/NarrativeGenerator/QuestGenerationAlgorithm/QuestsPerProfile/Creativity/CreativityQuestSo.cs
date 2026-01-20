using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using static Util.Enums;
using Overlord.NarrativeGenerator.NPCs;

namespace Overlord.NarrativeGenerator.Quests.QuestGrammarTerminals
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

        public override QuestSo DefineQuestSo(List<QuestSo> questSos, NpcSo npcInCharge, in NarrativeSettings narrativeSettings, Language language)
        {
            switch ( SymbolType )
            {
                case Constants.ExploreQuest:
                    return CreateAndSaveExploreQuestSo(questSos, npcInCharge, narrativeSettings.RoomsToExplore, language);
                case Constants.GotoQuest:
                    return CreateAndSaveGotoQuestSo(questSos, npcInCharge, language);
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

        public override void CreateQuestString(Enums.Language l)
        {
            throw new NotImplementedException();
        }


        private static ExploreQuestSo CreateAndSaveExploreQuestSo(List<QuestSo> questSos, NpcSo npcInCharge, RangedInt roomsToExplore, Language language)
        {
            var exploreQuest = CreateInstance<ExploreQuestSo>();
            var numOfRoomsToExplore = RandomSingleton.GetInstance().Random.Next(roomsToExplore.Max - roomsToExplore.Min) + roomsToExplore.Min;

            if (language == Language.Portuguese)
                exploreQuest.Init($"Explore {numOfRoomsToExplore} salas", false, questSos.Count > 0 ? questSos[^1] : null, numOfRoomsToExplore);
            else if (language == Language.English)
                exploreQuest.Init($"Explore {numOfRoomsToExplore} rooms", false, questSos.Count > 0 ? questSos[^1] : null, numOfRoomsToExplore);

            if (questSos.Count > 0)
            {
                questSos[^1].Next = exploreQuest;
            }
            exploreQuest.NpcInCharge = npcInCharge;

            questSos.Add(exploreQuest);

            return exploreQuest;
        }

        private static GotoQuestSo CreateAndSaveGotoQuestSo( List<QuestSo> questSos, NpcSo npcInCharge, Language language)
        {
            var gotoQuest = CreateInstance<GotoQuestSo>();

            if (language == Language.Portuguese)
                gotoQuest.Init("Vá para a sala marcada", false, questSos.Count > 0 ? questSos[^1] : null);
            else if (language == Language.English)
                gotoQuest.Init("Go to the marked room", false, questSos.Count > 0 ? questSos[^1] : null);

            if (questSos.Count > 0)
            {
                questSos[^1].Next = gotoQuest;
            }
            gotoQuest.NpcInCharge = npcInCharge;

            questSos.Add(gotoQuest);
            return gotoQuest;
        }
    }
}