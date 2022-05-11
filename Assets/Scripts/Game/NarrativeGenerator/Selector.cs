using System;
using System.Collections.Generic;
using System.Linq;
using Game.Events;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarNonterminals;
using ScriptableObjects;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Game.NarrativeGenerator
{
    public class Selector
    {
        Dictionary<string, Func<int,int>> startSymbolWeights = new Dictionary<string, Func<int,int>>();
        Dictionary<string, int> questWeightsbyType = new Dictionary<string, int>();
        private static readonly int[] WEIGHTS = {1, 3, 5, 7};

        private PlayerProfile playerProfile;
        public QuestWeightsManager questWeightsManager;

        public PlayerProfile SelectProfile(List<int> answers)
        {
            CalculateProfileWeights(answers);

            CreateProfileWithWeights();
            
            return playerProfile;
        }        
        
        public PlayerProfile SelectProfile(NarrativeCreatorEventArgs eventArgs)
        {
            questWeightsbyType = eventArgs.QuestWeightsbyType;

            CreateProfileWithWeights();
            
            return playerProfile;
        }
        
        public void CreateMissions(QuestGeneratorManager m)
        {
            m.Quests.graph = DrawMissions(m.PlaceholderNpcs, m.PlaceholderItems, m.PossibleWeapons);
        }

        private void CreateProfileWithWeights()
        {
            playerProfile = new PlayerProfile
            {
                AchievementPreference = questWeightsbyType[PlayerProfile.PlayerProfileCategory.Achievement.ToString()],
                MasteryPreference = questWeightsbyType[PlayerProfile.PlayerProfileCategory.Mastery.ToString()],
                CreativityPreference = questWeightsbyType[PlayerProfile.PlayerProfileCategory.Creativity.ToString()],
                ImmersionPreference = questWeightsbyType[PlayerProfile.PlayerProfileCategory.Immersion.ToString()]
            };
            
            string favoriteQuest = questWeightsbyType.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            playerProfile.SetProfileFromFavoriteQuest(favoriteQuest);
        }

        private List<QuestSO> DrawMissions(List<NpcSO> possibleNpcs, TreasureRuntimeSetSO possibleTreasures, WeaponTypeRuntimeSetSO possibleEnemyTypes)
        {
            var questsSos = new List<QuestSO>();
            MarkovChain questChain = new MarkovChain();
            var chainCost = 0;
            int i =0;
            do
            {
                i = 0;
                while ( questChain.GetLastSymbol().canDrawNext && i < 10 )
                {
                    // Dictionary<string, Func<int,int>> symbolWeights = GetSymbolWeights( questChain );
                    questChain.GetLastSymbol().SetNextSymbol( questChain );//, symbolWeights );
                    SaveCurrentQuest( questChain, questsSos, possibleNpcs, possibleTreasures, possibleEnemyTypes );
                    i++;
                    break;
                }
                chainCost += (int)Enums.QuestWeights.Hated*2;
            } while (chainCost < (int)Enums.QuestWeights.Loved );
            Debug.Log("FINAL I: "+i);
            Debug.Log("FINAL QUEST SO:");
            foreach (QuestSO quest in questsSos)
            {
                Debug.Log(quest.symbolType);
            }
            return questsSos;
        }

        private void SaveCurrentQuest ( MarkovChain questChain, List<QuestSO> questSos, List<NpcSO> possibleNpcs, TreasureRuntimeSetSO possibleTreasures, WeaponTypeRuntimeSetSO possibleEnemyTypes )
        {
            switch ( questChain.GetLastSymbol().symbolType )
            {
                case Constants.TALK_QUEST:
                    var t = new Talk();
                    t.DefineQuestSO( questSos, possibleNpcs );
                    break;
                case Constants.GET_QUEST:
                    var g = new Get();
                    g.DefineQuestSO( questChain, questSos, possibleNpcs, possibleTreasures, possibleEnemyTypes);
                    break;
                case Constants.KILL_QUEST:
                    var k = new Kill();
                    k.DefineQuestSO( questSos, possibleEnemyTypes );
                    break;
                case Constants.EXPLORE_QUEST:
                    var e = new Explore();
                    e.DefineQuestSO( questSos );
                    break;
            }
        }

        private Dictionary<string, Func<int,int>> GetSymbolWeights ( MarkovChain questChain )
        {
            Dictionary<string, Func<int,int>> symbolWeights = startSymbolWeights;
            switch ( questChain.GetLastSymbol().symbolType )
            {
                case Constants.KILL_QUEST:
                    symbolWeights = questWeightsManager.killSymbolWeights;
                    break;
                case Constants.TALK_QUEST:
                    symbolWeights = questWeightsManager.talkSymbolWeights;
                    break;
                case Constants.GET_QUEST:
                    symbolWeights = questWeightsManager.getSymbolWeights;
                    break;
                case Constants.EXPLORE_QUEST:
                    symbolWeights = questWeightsManager.exploreSymbolWeights;
                    break;
                default:
                    Debug.LogWarning("Symbol type not found!");
                break;
            }
            Debug.Log("new weight!!");
            foreach ( KeyValuePair<string,Func<int,int>> weight in symbolWeights )
            {
                Debug.Log($"weight of {weight.Key} isn't null!");
            }
            return symbolWeights;
        }

        private void CalculateProfileWeights(List<int> answers)
        {
            int[] weights = CalculateStartSymbolWeights( answers );

           questWeightsManager.CalculateTerminalSymbolsWeights(); // TODO: remove from here and put it in each quest -lu

            if ( weights[0] != 0 ) startSymbolWeights.Add( Constants.TALK_QUEST, x => weights[0] );
            if ( weights[1] != 0 ) startSymbolWeights.Add( Constants.GET_QUEST, x => weights[1] );
            if ( weights[2] != 0 ) startSymbolWeights.Add( Constants.KILL_QUEST, x => weights[2] ); 
            if ( weights[3] != 0 ) startSymbolWeights.Add( Constants.EXPLORE_QUEST, x => weights[3] );

            questWeightsbyType.Add(PlayerProfile.PlayerProfileCategory.Immersion.ToString(), weights[0]);
            questWeightsbyType.Add(PlayerProfile.PlayerProfileCategory.Achievement.ToString(), weights[1]);
            questWeightsbyType.Add(PlayerProfile.PlayerProfileCategory.Mastery.ToString(), weights[2]);
            questWeightsbyType.Add(PlayerProfile.PlayerProfileCategory.Creativity.ToString(), weights[3]);
        }

        private int[] CalculateStartSymbolWeights (List<int> answers)
        {
            int totalQuestionsWeight = questWeightsManager.CalculateTotalQuestionsWeight ( answers );

            int talkWeight = questWeightsManager.GetTalkQuestWeight( answers, totalQuestionsWeight );
            int getWeight = questWeightsManager.GetGetQuestWeight( answers, totalQuestionsWeight );
            int killWeight = questWeightsManager.GetKillQuestWeight( answers, totalQuestionsWeight );
            int exploreWeight = questWeightsManager.GetExploreQuestWeight( answers, totalQuestionsWeight );

            int [] startSymbolWeights = {talkWeight, getWeight, killWeight, exploreWeight};
            return startSymbolWeights;
        }
    }
}