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
        Dictionary<string, Func<int,int>> killSymbolWeights = new Dictionary<string, Func<int,int>>();
        Dictionary<string, Func<int,int>> talkSymbolWeights = new Dictionary<string, Func<int,int>>();
        Dictionary<string, Func<int,int>> getSymbolWeights = new Dictionary<string, Func<int,int>>();
        Dictionary<string, Func<int,int>> exploreSymbolWeights = new Dictionary<string, Func<int,int>>();
        //leo
        public class QuestWeight
        {
            public string quest;
            public int weight;

            public QuestWeight(string quest, int weight)
            {
                this.quest = quest;
                this.weight = weight;
            }
        }

        // public List<QuestWeight> questWeights = new List<QuestWeight>();
        Dictionary<string, int> questWeightsbyType = new Dictionary<string, int>();
        private static readonly int[] WEIGHTS = {1, 3, 5, 7};

        private PlayerProfile playerProfile;
        private QuestWeightsManager questWeightsManager;

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
            while ( questChain.GetLastSymbol().canDrawNext )
            {
                SetSymbolWeights( ref questChain );
                DefineNextQuest( ref questChain, questsSos, possibleNpcs, possibleTreasures, possibleEnemyTypes );

                if ( IsLastQuestValid ( questChain ) )
                {
                    questsSos.Add( questChain.GetLastSymbol() as QuestSO );
                    Debug.Log( questChain.GetLastSymbol().symbolType );
                }
            }
            return questsSos;
            //TODO: Selecionar mais de apenas uma linha de quests
            // var chainCost = 0;
            // do
            // {
            //     colocar aqui o drawmissions atual ?
            //     chainCost += (int)Enums.QuestWeights.Hated*2;
            // } while (chainCost < (int)Enums.QuestWeights.Loved);
        }

        private bool IsLastQuestValid ( MarkovChain questChain )
        {
            return questChain.GetLastSymbol() as QuestSO != null && questChain.GetLastSymbol().symbolType != Constants.EMPTY_TERMINAL;
        }

        private void DefineNextQuest ( ref MarkovChain questChain, List<QuestSO> questsSos, List<NpcSO> possibleNpcs, TreasureRuntimeSetSO possibleTreasures, WeaponTypeRuntimeSetSO possibleEnemyTypes )
        {
            switch ( questChain.GetLastSymbol().symbolType )
            {
                case Constants.TALK_QUEST:
                    var t = new Talk(0, questWeightsbyType);
                    t.Option( questChain, questsSos, possibleNpcs);
                    break;
                case Constants.GET_QUEST:
                    var g = new Get(0, questWeightsbyType);
                    g.Option( questChain, questsSos, possibleNpcs, possibleTreasures, possibleEnemyTypes);
                    break;
                case Constants.KILL_QUEST:
                    var k = new Kill(0, questWeightsbyType);
                    k.Option( questChain, questsSos, possibleNpcs, possibleEnemyTypes);
                    break;
                case Constants.EXPLORE_QUEST:
                    var e = new Explore(0, questWeightsbyType);
                    e.Option( questChain, questsSos, possibleNpcs);
                    break;
            }
        }

        private void SetSymbolWeights ( ref MarkovChain questChain )
        {
            Dictionary<string, Func<int,int>> symbolWeights = startSymbolWeights;
            switch ( questChain.GetLastSymbol().symbolType )
            {
                case Constants.KILL_QUEST:
                    symbolWeights = killSymbolWeights;
                    break;
                case Constants.TALK_QUEST:
                    symbolWeights = talkSymbolWeights;
                    break;
                case Constants.GET_QUEST:
                    symbolWeights = getSymbolWeights;
                    break;
                case Constants.EXPLORE_QUEST:
                    symbolWeights = exploreSymbolWeights;
                    break;
                default:
                    Debug.LogWarning("Symbol type not found!");
                break;
            }
            questChain.GetLastSymbol().SetDictionary( symbolWeights );
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