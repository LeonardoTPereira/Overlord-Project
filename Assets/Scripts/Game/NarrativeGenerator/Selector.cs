using System.Collections.Generic;
using System.Linq;
using System;
using Game.ExperimentControllers;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;
using UnityEngine;

namespace Game.NarrativeGenerator
{
    public static class Selector
    {
        private static Dictionary<string,bool> _wasQuestAdded;
        private static GeneratorSettings _generatorSettings;

        public static QuestLineList CreateMissions(in GeneratorSettings generatorSettings)
        {
            _generatorSettings = generatorSettings;
            _wasQuestAdded = new Dictionary<string,bool>();
            return DrawMissions();
        }

        private static QuestLineList DrawMissions()
        {
            CreateQuestDict();
            var questLineList = ScriptableObject.CreateInstance<QuestLineList>();
            questLineList.Init();            
            CreateQuestLineForEachNpc(questLineList);

            var i = 0;
            while ( _wasQuestAdded.ContainsValue(false) && i < 100 )
            {
                i++;
                var selectedNpc = _generatorSettings.PlaceholderNpcs[UnityEngine.Random.Range(0, _generatorSettings.PlaceholderNpcs.Count)];
                ContinueQuestLineForNpc(selectedNpc, questLineList);
            }
            return questLineList;
        }

        private static void CreateQuestLineForEachNpc(QuestLineList questLineList)
        {
            foreach (var npcInCharge in _generatorSettings.PlaceholderNpcs)
            {
               CreateQuestLineForNpc(npcInCharge, questLineList);
            }
        }

        private static void CreateQuestLineForNpc ( NpcSo npcInCharge, QuestLineList questLineList)
        {
            var questLine = CreateQuestLine();
            questLine.PopulateQuestLine(_generatorSettings, ProfileCalculator.StartSymbolWeights);
            UpdateListContents(questLine);
            questLine.Quests[^1].EndsStoryLine = true;
            questLine.NpcInCharge = npcInCharge;
            questLineList.QuestLines.Add(questLine);
        }
        
        private static void ContinueQuestLineForNpc ( NpcSo npcInCharge, QuestLineList questLineList)
        {
            var questLine = questLineList.QuestLines.Single(questLine => questLine.NpcInCharge.NpcName == npcInCharge.NpcName);
            if (questLine != null)
            {
                questLine.Quests[^1].EndsStoryLine = false;
                // Dictionary<string, Func<int, int>> startSymbolWeights = CalculateNewStartWeight();
                // questLine.PopulateQuestLine(_generatorSettings, startSymbolWeights);
                questLine.PopulateQuestLine(_generatorSettings, ProfileCalculator.StartSymbolWeights);
                UpdateListContents(questLine);
                questLine.Quests[^1].EndsStoryLine = true;
            }
            else
            {
                Debug.LogError($"No QuestLine Found With {npcInCharge.NpcName} In Charge");
                CreateQuestLineForNpc(npcInCharge, questLineList);
            }
        }

        // private static Dictionary<string, Func<int, int>> CalculateNewStartWeight ()
        // {
        //     Dictionary<string, Func<int, int>> dict = ProfileCalculator.StartSymbolWeights;
        //     foreach (KeyValuePair<string,bool> questType in _wasQuestAdded)
        //     {
        //         if ( questType.Value )
        //         {
        //             dict.Remove(questType.Key);
        //             Debug.Log("Removing "+questType.Key);
        //         }
        //     }

        //     int normalizeConst = 0;
        //     foreach (KeyValuePair<string, Func<int, int>> item in dict)
        //     {
        //         normalizeConst += item.Value(0);
        //     }

        //     string[] obrigatoryKeys = {nameof(KillQuestSo), nameof(AchievementQuestSo), nameof(ImmersionQuestSo), nameof(CreativityQuestSo)};
        //     for (int i =0; i< obrigatoryKeys.Length ; i++ )
        //     {
        //         if ( dict.ContainsKey(obrigatoryKeys[i]))
        //         {
        //             dict[ obrigatoryKeys[i] ] = _ => (100*dict[obrigatoryKeys[i]](0))/normalizeConst;
        //             Debug.Log( obrigatoryKeys[i] + " "+ dict[ obrigatoryKeys[i] ]);
        //         }

        //     }
        //     Debug.Log(dict.ElementAt(0).Key +" "+ dict.ElementAt(0).Value(0));
        //     return dict;
        // }

        private static QuestLine CreateQuestLine()
        {
            var questLine = ScriptableObject.CreateInstance<QuestLine>();
            questLine.Init();
            return questLine;
        }

        private static void CreateQuestDict ()
        {
            _wasQuestAdded.Add(nameof(KillQuestSo), false);
        }

        private static void UpdateListContents (QuestLine questLine)
        {
            foreach (var quest in questLine.Quests.Where(quest => quest != null))
            {
                _wasQuestAdded[quest.GetType().Name] = true;
            }
        }
    }
}