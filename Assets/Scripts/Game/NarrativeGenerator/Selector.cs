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
    //Seleciona a linha de missões de acordo com os pesos do perfil do jogador
    public class Selector
    {
        Dictionary<string, Func<float,float>> startSymbolWeights = new Dictionary<string, Func<float,float>>();
        Dictionary<string, Func<float,float>> killSymbolWeights = new Dictionary<string, Func<float,float>>();
        Dictionary<string, Func<float,float>> talkSymbolWeights = new Dictionary<string, Func<float,float>>();
        Dictionary<string, Func<float,float>> getSymbolWeights = new Dictionary<string, Func<float,float>>();
        Dictionary<string, Func<float,float>> exploreSymbolWeights = new Dictionary<string, Func<float,float>>();
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
        
        /*
        [7][5][1][3]
        [3][7][1][5]
        [1][5][7][3]
        [1][5][3][7]
        */

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
            Dictionary<string, Func<float,float>> symbolWeights = startSymbolWeights;
            MarkovChain questChain = new MarkovChain();
            while ( questChain.GetLastSymbol().canDrawNext )
            {
                questChain.GetLastSymbol().SetDictionary( symbolWeights );
                questChain.GetLastSymbol().SetNextSymbol( questChain );

                if ( 
                    questChain.GetLastSymbol() as QuestSO != null &&
                    questChain.GetLastSymbol().symbolType != Constants.EMPTY_TERMINAL
                )
                {
                    questsSos.Add( questChain.GetLastSymbol() as QuestSO );
                }
                SelectNextMission( questChain, symbolWeights );
            }
            // TODO: ver se alguma parte desse código pode ser readaptada pra nova geração de narrativas pq ta muito bonitinho -lu
            // var newMissionDraw = 0.0f;
            // var chainCost = 0;
            // do
            // {
            //     foreach (var item in questWeightsbyType.Where(item => item.Value > newMissionDraw))
            //     {
            //         switch (item.Key)
            //         {
            //             case Constants.TALK_QUEST:
            //                 var t = new Talk(0, questWeightsbyType);
            //                 t.Option(questsSos, possibleNpcs);
            //                 break;
            //             case Constants.GET_QUEST:
            //                 var g = new Get(0, questWeightsbyType);
            //                 g.Option(questsSos, possibleNpcs, possibleTreasures, possibleEnemyTypes);
            //                 break;
            //             case Constants.KILL_QUEST:
            //                 var k = new Kill(0, questWeightsbyType);
            //                 k.Option(questsSos, possibleNpcs, possibleEnemyTypes);
            //                 break;
            //             case Constants.EXPLORE_QUEST:
            //                 var e = new Explore(0, questWeightsbyType);
            //                 e.Option(questsSos, possibleNpcs);
            //                 break;
            //         }
            //     }
            //     chainCost += (int)Enums.QuestWeights.Hated*2;
            //     newMissionDraw = RandomSingleton.GetInstance().Random.Next((int)Enums.QuestWeights.Loved)+chainCost;
            // } while (chainCost < (int)Enums.QuestWeights.Loved);
            return questsSos;
        }

        private void SelectNextMission ( MarkovChain questChain, Dictionary<string, Func<float,float>> symbolWeights )
        {
            switch ( questChain.GetLastSymbol().symbolType )
            {
                case Constants.KILL_QUEST:
                // case Constants.KILL_TERMINAL:
                    symbolWeights = killSymbolWeights;
                break;
                case Constants.TALK_QUEST:
                // case Constants.TALK_TERMINAL:
                    symbolWeights = talkSymbolWeights;
                break;
                case Constants.GET_QUEST:
                // case Constants.GET_TERMINAL:
                // case Constants.DROP_TERMINAL:
                // case Constants.ITEM_TERMINAL:
                    symbolWeights = getSymbolWeights;
                break;
                case Constants.EXPLORE_QUEST:
                // case Constants.SECRET_TERMINAL:
                    symbolWeights = exploreSymbolWeights;
                break;
                // case Constants.EMPTY_TERMINAL:
                // break;
                default:
                    Debug.LogError("Symbol type not found!");
                break;
            }
        }

        private void CalculateProfileWeights(List<int> answers)
        {
            int[] weights = CalculateStartSymbolWeights( answers );

           questWeightsManager.CalculateTerminalSymbolsWeights(); // TODO: remove from here and put in each quest -lu

            // TODO: Ver com o Leo se tudo bem remover isso aqui -lu
            // questWeights.Add(new QuestWeight(Constants.TALK_QUEST, weights[0]));
            // questWeights.Add(new QuestWeight(Constants.GET_QUEST, weights[1]));
            // questWeights.Add(new QuestWeight(Constants.KILL_QUEST, weights[2]));
            // questWeights.Add(new QuestWeight(Constants.EXPLORE_QUEST, weights[3]));
            // Atribui 1, 3, 5, 7 aos pesos das quests - não parece fazer sentido?
            // questWeights = questWeights.OrderBy(x => x.weight).ToList();
            // for (int i = 0; i < questWeights.Count; ++i)
            // {
            //     questWeights[i].weight = WEIGHTS[i];
            //     Debug.Log($"Quest Weight [{i}]: {questWeights[i].weight}");
            // }
            // weights[0] = questWeights.Find(x => x.quest == Constants.TALK_QUEST).weight;
            // weights[1] = questWeights.Find(x => x.quest == Constants.GET_QUEST).weight;
            // weights[2] = questWeights.Find(x => x.quest == Constants.KILL_QUEST).weight;
            // weights[3] = questWeights.Find(x => x.quest == Constants.EXPLORE_QUEST).weight;

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