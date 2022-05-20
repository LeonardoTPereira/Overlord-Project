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

            CalculateStartSymbolWeights ( playerProfile );
            string favoriteQuest = questWeightsbyType.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            playerProfile.SetProfileFromFavoriteQuest(favoriteQuest);
        }

        private List<QuestSO> DrawMissions(List<NpcSO> possibleNpcs, TreasureRuntimeSetSO possibleTreasures, WeaponTypeRuntimeSetSO possibleEnemyTypes)
        {
            var questsSos = new List<QuestSO>();
            MarkovChain questChain = new MarkovChain();
            var chainCost = 0;
            do
            {
                questChain.GetLastSymbol().SetDictionary( startSymbolWeights );
                while ( questChain.GetLastSymbol().canDrawNext )
                {
                    questChain.GetLastSymbol().SetNextSymbol( questChain );
                    SaveCurrentQuest( questChain, questsSos, possibleNpcs, possibleTreasures, possibleEnemyTypes );
                }
                chainCost += (int)Enums.QuestWeights.Hated*2;
            } while (chainCost < (int)Enums.QuestWeights.Loved );
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
                case Constants.TALK_TERMINAL:
                    var t = new Talk();
                    t.DefineQuestSO( questSos, possibleNpcs );
                    break;
                case Constants.GET_QUEST:
                case Constants.GET_TERMINAL:
                case Constants.ITEM_TERMINAL:
                case Constants.DROP_TERMINAL:
                    var g = new Get();
                    g.DefineQuestSO( questChain, questSos, possibleNpcs, possibleTreasures, possibleEnemyTypes);
                    break;
                case Constants.KILL_QUEST:
                case Constants.KILL_TERMINAL:
                    var k = new Kill();
                    k.DefineQuestSO( questSos, possibleEnemyTypes );
                    break;
                case Constants.EXPLORE_QUEST:
                case Constants.EXPLORE_TERMINAL:
                case Constants.SECRET_TERMINAL:
                    var e = new Explore();
                    e.DefineQuestSO( questSos );
                    break;
            }
        }

        private void CalculateProfileWeights(List<int> answers)
        {
            startSymbolWeights.Clear();
            int[] weights = CalculateStartSymbolWeights( answers );

            if ( weights[0] != 0 ) startSymbolWeights.Add( Constants.TALK_QUEST, x => weights[0] );
            if ( weights[1] != 0 ) startSymbolWeights.Add( Constants.GET_QUEST, x => weights[1] );
            if ( weights[2] != 0 ) startSymbolWeights.Add( Constants.KILL_QUEST, x => weights[2] ); 
            if ( weights[3] != 0 ) startSymbolWeights.Add( Constants.EXPLORE_QUEST, x => weights[3] );

            questWeightsbyType.Add(PlayerProfile.PlayerProfileCategory.Immersion.ToString(), weights[0]);
            questWeightsbyType.Add(PlayerProfile.PlayerProfileCategory.Achievement.ToString(), weights[1]);
            questWeightsbyType.Add(PlayerProfile.PlayerProfileCategory.Mastery.ToString(), weights[2]);
            questWeightsbyType.Add(PlayerProfile.PlayerProfileCategory.Creativity.ToString(), weights[3]);
        }

        private int[] CalculateStartSymbolWeights ( List<int> answers )
        {
            int totalQuestionsWeight = questWeightsManager.CalculateTotalQuestionsWeight ( answers );

            int talkWeight = questWeightsManager.GetTalkQuestWeight( answers, totalQuestionsWeight );
            int getWeight = questWeightsManager.GetGetQuestWeight( answers, totalQuestionsWeight );
            int killWeight = questWeightsManager.GetKillQuestWeight( answers, totalQuestionsWeight );
            int exploreWeight = questWeightsManager.GetExploreQuestWeight( answers, totalQuestionsWeight );

            int [] startSymbolWeights = {talkWeight, getWeight, killWeight, exploreWeight};
            return startSymbolWeights;
        }

        private void CalculateStartSymbolWeights ( PlayerProfile playerProfile )
        {
            startSymbolWeights.Clear();

            int talkWeight = (int)(100*playerProfile.CreativityPreference)/16;
            int getWeight = (int)(100*playerProfile.AchievementPreference)/16;
            int killWeight = (int)(100*playerProfile.MasteryPreference)/16;
            int exploreWeight = (int)(100*playerProfile.ImmersionPreference)/16;

            if ( talkWeight != 0 ) startSymbolWeights.Add( Constants.TALK_QUEST, x => talkWeight );
            if ( getWeight != 0 ) startSymbolWeights.Add( Constants.GET_QUEST, x => getWeight );
            if ( killWeight != 0 ) startSymbolWeights.Add( Constants.KILL_QUEST, x => killWeight ); 
            if ( exploreWeight != 0 ) startSymbolWeights.Add( Constants.EXPLORE_QUEST, x => exploreWeight );
        }
    }
}