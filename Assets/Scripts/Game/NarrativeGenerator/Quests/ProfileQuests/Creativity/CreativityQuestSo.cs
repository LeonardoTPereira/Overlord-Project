using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class CreativityQuestSo : QuestSo
    {
        public override string symbolType {
            get { return Constants.CREATIVITY_QUEST; }
        }
        public override Dictionary<string, Func<int,int>> NextSymbolChances
        {
            get {

                if ( nextSymbolChances != null )
                    return nextSymbolChances;

                Dictionary<string, Func<int, int>> creativityQuestWeights = new Dictionary<string, Func<int, int>>();
                creativityQuestWeights.Add( Constants.EXPLORE_QUEST, Constants.TwoOptionQuestLineWeight );
                creativityQuestWeights.Add( Constants.GOTO_QUEST, Constants.TwoOptionQuestLineWeight );
                creativityQuestWeights.Add( Constants.EMPTY_QUEST, Constants.OneOptionQuestEmptyWeight );
                return creativityQuestWeights;

            } 
        }

        public override void DefineQuestSo ( List<QuestSo> questSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSO possibleItems, WeaponTypeRuntimeSetSO enemyTypes)
        {
            switch ( this.symbolType )
            {
                case Constants.EXPLORE_QUEST:
                    CreateAndSaveExploreQuestSo(questSos);
                break;
                case Constants.GOTO_QUEST:
                    CreateAndSaveGotoQuestSo(questSos);
                break;
                default:
                    Debug.LogError("help something went wrong!");
                break;
            }
        }

        public static void CreateAndSaveExploreQuestSo( List<QuestSo> questSos)
        {
            var exploreQuest = ScriptableObject.CreateInstance<ExploreQuestSo>();
            var numOfRoomsToExplore = RandomSingleton.GetInstance().Random.Next(10) + 3;
            exploreQuest.Init($"Explore {numOfRoomsToExplore} rooms", false, questSos.Count > 0 ? questSos[^1] : null, numOfRoomsToExplore);
            
            if (questSos.Count > 0)
            {
                questSos[^1].Next = exploreQuest;
            }

            questSos.Add(exploreQuest);
        }

        public static void CreateAndSaveGotoQuestSo( List<QuestSo> questSos )
        {
            var gotoQuest = ScriptableObject.CreateInstance<GotoQuestSo>();
            //TODO verify if there's a way to mark the room in the minimap/get a rooms name here
            gotoQuest.Init("Go to the marked room", false, questSos.Count > 0 ? questSos[^1] : null);
            if (questSos.Count > 0)
            {
                questSos[^1].Next = gotoQuest;
            }

            questSos.Add(gotoQuest);
        }
    }
}