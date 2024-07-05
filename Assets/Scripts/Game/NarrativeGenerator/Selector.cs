using System;
using System.Collections.Generic;
using System.Linq;
using Game.DataCollection;
using Game.Events;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarNonterminals;
using Game.NPCs;
using ScriptableObjects;
using UnityEngine;
using Util;
using Enums = Util.Enums;

namespace Game.NarrativeGenerator
{
    public class Selector
    {
        //
        private Dictionary<string,List<int>> _sameQuestDistance = new Dictionary<string, List<int>>();
        private Dictionary<string,int> _averageSameQuestDistance = new Dictionary<string, int>();
        private Dictionary<string,List<int>> _sameTypeQuestDistance = new Dictionary<string, List<int>>();
        private Dictionary<string,int> _averageSameTypeQuestDistance = new Dictionary<string, int>();
        //
        public void CreateMissions(QuestGeneratorManager m)
        {
            m.Quests.graph = DrawMissions(m.PlaceholderNpcs, m.PlaceholderItems, m.PossibleWeapons);
        }

        private List<QuestSO> DrawMissions(List<NpcSo> possibleNpcs, TreasureRuntimeSetSO possibleTreasures, WeaponTypeRuntimeSetSO possibleEnemyTypes)
        {
            for ( int j = 0; j < 200; j++ )
            {
                bool containsKill = false, containsTalk = false, containsGet = false, containsExplore = false;
                var questSos = new List<QuestSO>();
                PopulateDictionary();
                int i = 0;
                do
                {
                    MarkovChain questChain = new MarkovChain();
                    questChain.GetLastSymbol().SetDictionary( ProfileCalculator.StartSymbolWeights );
                    while ( questChain.GetLastSymbol().canDrawNext )
                    {
                        questChain.GetLastSymbol().SetNextSymbol( questChain );
                        if (  questChain.GetLastSymbol().symbolType != Constants.KILL_QUEST && questChain.GetLastSymbol().symbolType != Constants.TALK_QUEST && questChain.GetLastSymbol().symbolType != Constants.GET_QUEST && questChain.GetLastSymbol().symbolType != Constants.EXPLORE_QUEST )
                            SaveCurrentQuest( questChain, questSos, possibleNpcs, possibleTreasures, possibleEnemyTypes );
                        UpdateListContents( questChain.GetLastSymbol(), ref containsKill ,ref containsTalk ,ref containsGet ,ref containsExplore );
                    }
                    i += 1;
                } while ( (!containsKill || !containsTalk || !containsGet || !containsExplore) );
                GetAverageQuesDistances();
                QuestsToJson.AddGeneratedQuests( j, questSos, _averageSameQuestDistance, _averageSameTypeQuestDistance );
            }
            QuestsToJson.CreateJson();
            
            return null;//questSos;
        }

        private void UpdateListContents ( Symbol lastQuest, ref bool containsKill ,ref bool containsTalk ,ref bool containsGet ,ref bool containsExplore )
        {
            string questType = "";
            switch ( lastQuest.symbolType )
            {
                case Constants.TALK_QUEST:
                    containsTalk = true;
                    questType = Constants.TALK_QUEST;
                    break;
                case Constants.GET_QUEST:
                    containsGet = true;
                    questType = Constants.GET_QUEST;
                    break;
                case Constants.KILL_QUEST:
                    containsKill = true;
                    questType = Constants.KILL_QUEST;
                    break;
                case Constants.EXPLORE_QUEST:
                    containsExplore = true;
                    questType = Constants.EXPLORE_QUEST;
                    break;
            }

            //TO DO: CALCULATE DISTANCES
            List<string> keys = new List<string>(_sameQuestDistance.Keys);
            foreach (string item in keys)
            {
                if ( item != lastQuest.symbolType )
                {
                    _sameQuestDistance[item][_sameQuestDistance[item].Count-1] += 1;
                }
                else
                {
                    _sameQuestDistance[item].Add(0);
                }
            }

            List<string> keys2 = new List<string>(_sameTypeQuestDistance.Keys);
            foreach (string item in keys2)
            {
                if ( item != questType )
                {
                    _sameTypeQuestDistance[item][_sameTypeQuestDistance[item].Count-1] += 1;
                }
                else
                {
                    _sameTypeQuestDistance[item].Add(0);
                }
            }
        }

        private void PopulateDictionary()
        {
            string[] questNames = {"experiment", "gather", "repair", "take", "use", "exchange", "give", "listen", "report", "read",
                "capture", "damage", "defend", "kill", "escort", "goto", "spy", "stealth"};

            string[] questTypes = {"Achievement", "Immersion", "Mastery", "Creativity"};

            _sameQuestDistance.Clear();
            _sameTypeQuestDistance.Clear();
            
            foreach (var questName in questNames)
            {
                _sameQuestDistance.Add( questName, new List<int>());
                _sameQuestDistance[questName].Add(0);
            }

            foreach (var questType in questTypes)
            {
                _sameTypeQuestDistance.Add( questType, new List<int>());
                _sameTypeQuestDistance[questType].Add(0);
            }
        }

        private void GetAverageQuesDistances()
        {
            _averageSameQuestDistance.Clear();
            _averageSameTypeQuestDistance.Clear();
            foreach (KeyValuePair<string,List<int>> pair in _sameQuestDistance)
            {
                _averageSameQuestDistance.Add( pair.Key, pair.Value.Sum()/pair.Value.Count );
            }
            foreach (KeyValuePair<string,List<int>> pair in _sameTypeQuestDistance)
            {
                _averageSameTypeQuestDistance.Add( pair.Key, pair.Value.Sum()/pair.Value.Count );
            }
        }

        private void SaveCurrentQuest ( MarkovChain questChain, List<QuestSO> questSos, List<NpcSo> possibleNpcs, TreasureRuntimeSetSO possibleTreasures, WeaponTypeRuntimeSetSO possibleEnemyTypes )
        {
            QuestSO newQuest = ScriptableObject.CreateInstance<QuestSO>();
            newQuest.symbolType = questChain.GetLastSymbol().symbolType;
            questSos.Add(newQuest);
            // switch ( questChain.GetLastSymbol().symbolType )
            // {
            //     case Constants.TALK_QUEST:
            //     case Constants.TALK_TERMINAL:
            //         var t = new Talk();
            //         t.DefineQuestSO( questSos, possibleNpcs );
            //         break;
            //     case Constants.GET_QUEST:
            //     case Constants.GET_TERMINAL:
            //     case Constants.ITEM_TERMINAL:
            //     case Constants.DROP_TERMINAL:
            //         var g = new Get();
            //         g.DefineQuestSO( questChain, questSos, possibleNpcs, possibleTreasures, possibleEnemyTypes);
            //         break;
            //     case Constants.KILL_QUEST:
            //     case Constants.KILL_TERMINAL:
            //         var k = new Kill();
            //         k.DefineQuestSO( questSos, possibleEnemyTypes );
            //         break;
            //     case Constants.EXPLORE_QUEST:
            //     case Constants.EXPLORE_TERMINAL:
            //     case Constants.SECRET_TERMINAL:
            //         var e = new Explore();
            //         e.DefineQuestSO( questSos );
            //         break;
            // }
        }

  
    }
}