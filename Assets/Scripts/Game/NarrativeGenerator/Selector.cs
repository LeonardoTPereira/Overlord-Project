using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.NarrativeGenerator.Quests;
using Util;

namespace Game.NarrativeGenerator
{
//classe que seleciona a linha de missões de acordo com os pesos do perfil do jogador
    public class Selector
    {
        Dictionary<float,SymbolType> startSymbolWeights = new Dictionary<float,SymbolType>();
        Dictionary<float,SymbolType> killSymbolWeights = new Dictionary<float,SymbolType>();
        Dictionary<float,SymbolType> talkSymbolWeights = new Dictionary<float,SymbolType>();
        Dictionary<float,SymbolType> getSymbolWeights = new Dictionary<float,SymbolType>();
        Dictionary<float,SymbolType> exploreSymbolWeights = new Dictionary<float,SymbolType>();
        // private PlayerProfile.PlayerProfileCategory typePlayer;

        // public PlayerProfile.PlayerProfileCategory Select(Manager m, List<int> answers)
        // {
        //     weightCalculator(answers);
        // }

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

        public List<QuestWeight> questWeights = new List<QuestWeight>();
        Dictionary<string, int> questWeightsbyType = new Dictionary<string, int>();
        private static readonly int[] WEIGHTS = {1, 3, 5, 7};

        private PlayerProfile playerProfile;

        public PlayerProfile Select(Manager m, List<int> answers)
        {
            //pesos[0] = 3; //peso talk
            //pesos[1] = 7; //peso get
            //pesos[2] = 1; //peso kill
            //pesos[3] = 5; //peso explore

            CalculateProfileWeights(answers);

            CreateProfileWithWeights();
            
            DrawMissions(m);
            
            return playerProfile;
        }

        public PlayerProfile Select(Manager m, NarrativeCreatorEventArgs eventArgs)
        {
            questWeightsbyType = eventArgs.QuestWeightsbyType;

            CreateProfileWithWeights();

        //     DrawMissions(m);

            return playerProfile;
        }

        private void CreateProfileWithWeights()
        {
            playerProfile = new PlayerProfile();

            playerProfile.AchievementPreference = questWeightsbyType[PlayerProfile.PlayerProfileCategory.Achievement.ToString()];
            playerProfile.MasteryPreference = questWeightsbyType[PlayerProfile.PlayerProfileCategory.Mastery.ToString()];
            playerProfile.CreativityPreference = questWeightsbyType[PlayerProfile.PlayerProfileCategory.Creativity.ToString()];
            playerProfile.ImmersionPreference = questWeightsbyType[PlayerProfile.PlayerProfileCategory.Immersion.ToString()];
            
            string favoriteQuest = questWeightsbyType.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            playerProfile.SetProfileFromFavoriteQuest(favoriteQuest);
        }

        public void DrawMissions(Manager m)
        {
            Dictionary<float,SymbolType> symbolWeights = startSymbolWeights;
            MarkovChain questChain = new MarkovChain();

            while ( questChain.symbol.canDrawNext )
            {
                Debug.Log( questChain.symbolType );
                questChain.symbol.SetDictionary( symbolWeights );
                questChain.symbol.SetNextSymbol( questChain );

                if ( questChain.symbol as QuestSO != null )
                    m.Quests.graph.Add( questChain.symbol as QuestSO );

                switch ( questChain.symbolType )
                {
                    case SymbolType.Kill:
                        symbolWeights = killSymbolWeights;
                    break;
                    case SymbolType.Talk:
                        symbolWeights = talkSymbolWeights;
                    break;
                    case SymbolType.Get:
                        symbolWeights = getSymbolWeights;
                    break;
                    case SymbolType.Explore:
                        symbolWeights = exploreSymbolWeights;
                    break;
                    case SymbolType.kill:
                        symbolWeights = killSymbolWeights;
                    break;
                    case SymbolType.talk:
                        symbolWeights = talkSymbolWeights;
                    break;
                    case SymbolType.empty:
                    break;
                    case SymbolType.get:
                        symbolWeights = getSymbolWeights;
                    break;
                    case SymbolType.drop:
                        symbolWeights = getSymbolWeights;
                    break;
                    case SymbolType.item:
                        symbolWeights = getSymbolWeights;
                    break;
                    case SymbolType.secret:
                        symbolWeights = exploreSymbolWeights;
                    break;
                    default:
                        Debug.LogError("Symbol type not found!");
                    break;

                }
            }
            Debug.Log( questChain.symbolType );
        }

        private void CalculateProfileWeights(List<int> answers)
        {
            answers.Add(0);
            answers.Add(1);
            answers.Add(2);
            answers.Add(3);
            answers.Add(4);
            answers.Add(0); answers.Add(0);  answers.Add(0); answers.Add(0);  answers.Add(1); answers.Add(0);  answers.Add(0); // TESTES

            float totalQuestionsWeight = answers[5] + answers[6] +  answers[7] + answers[8]  +  answers[9] + answers[10] + answers[11];
            // Kill questions = 5, 6;
            // Explore questions = 7, 8;
            // Get questions = 9;
            // Talk questions = 10, 11;
            // Puzzle questions = 12, 13;

            float killWeight = ( answers[5] + answers[6] )/ ( totalQuestionsWeight );
            killWeight = float.IsNaN( killWeight ) ? 0 : killWeight;

            float exploreWeight = ( answers[7] + answers[8] )/ ( totalQuestionsWeight );
            exploreWeight = float.IsNaN(exploreWeight ) ? 0 : exploreWeight;

            float getWeight = ( answers[9] )/ ( totalQuestionsWeight );
            getWeight = float.IsNaN(getWeight ) ? 0 : getWeight;

            float talkWeight = ( answers[10] + answers[11] )/ ( totalQuestionsWeight );
            talkWeight = float.IsNaN(talkWeight ) ? 0 : talkWeight;

            //float puzzleWeight = ( answers[12] + answers[13] )/ ( maxQuestionWeight * 2 );
            //puzzleWeight = float.IsNaN(puzzleWeight ) ? 0 : puzzleWeight;

            float[] pesos = new float[4];

            if ( exploreWeight != 0 ) startSymbolWeights.Add( talkWeight + getWeight + killWeight + exploreWeight, SymbolType.Explore ); // 100%
            if ( killWeight != 0 ) startSymbolWeights.Add( talkWeight + getWeight + killWeight, SymbolType.Kill );
            if ( getWeight != 0 ) startSymbolWeights.Add(talkWeight + getWeight, SymbolType.Get );
            if ( talkWeight != 0 ) startSymbolWeights.Add(talkWeight, SymbolType.Talk);

            killSymbolWeights.Add( (5f/6f), SymbolType.kill );
            killSymbolWeights.Add((1f/6f), SymbolType.empty);

            talkSymbolWeights.Add( (5f/6f), SymbolType.talk );
            talkSymbolWeights.Add((1f/6f), SymbolType.empty );

            getSymbolWeights.Add( (10f/10f), SymbolType.item );
            getSymbolWeights.Add( (7f/10f), SymbolType.drop );
            getSymbolWeights.Add( (2f/10f), SymbolType.get);
            getSymbolWeights.Add( (1f/10f), SymbolType.empty );

            exploreSymbolWeights.Add( (5f/6f), SymbolType.secret );
            exploreSymbolWeights.Add( (1f/6f), SymbolType.empty );

            // Não sei dizer se isto é realmente necessário...
            // string favoriteQuest = startSymbolWeights.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            // GetProfileFromFavoriteQuest(favoriteQuest);
        }
    }
}