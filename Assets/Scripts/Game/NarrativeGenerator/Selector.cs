using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.NarrativeGenerator.Quests;
using Util;

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

            DrawMissions(m);

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
            Dictionary<string, Func<float,float>> symbolWeights = startSymbolWeights;
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
                    case Constants.KILL_TERMINAL:
                        symbolWeights = killSymbolWeights;
                    break;
                    case Constants.TALK_TERMINAL:
                        symbolWeights = talkSymbolWeights;
                    break;
                    case Constants.EMPTY_TERMINAL:
                    break;
                    case Constants.GET_TERMINAL:
                        symbolWeights = getSymbolWeights;
                    break;
                    case Constants.DROP_TERMINAL:
                        symbolWeights = getSymbolWeights;
                    break;
                    case Constants.ITEM_TERMINAL:
                        symbolWeights = getSymbolWeights;
                    break;
                    case Constants.SECRET_TERMINAL:
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

            if ( exploreWeight != 0 ) startSymbolWeights.Add( Constants.EXPLORE_QUEST, x => exploreWeight );
            if ( killWeight != 0 ) startSymbolWeights.Add( Constants.KILL_QUEST, x => killWeight );
            if ( getWeight != 0 ) startSymbolWeights.Add( Constants.GET_QUEST, x => getWeight );
            if ( talkWeight != 0 ) startSymbolWeights.Add( Constants.TALK_QUEST, x => talkWeight );

            killSymbolWeights.Add( Constants.KILL_TERMINAL, x => (5f/6f) );
            killSymbolWeights.Add( Constants.EMPTY_TERMINAL, x => (1f/6f) );

            talkSymbolWeights.Add( Constants.TALK_TERMINAL, x => (5f/6f) );
            talkSymbolWeights.Add( Constants.EMPTY_TERMINAL, x => (1f/6f) );

            getSymbolWeights.Add( Constants.ITEM_TERMINAL, x => (10f/10f) );
            getSymbolWeights.Add( Constants.DROP_TERMINAL, x => (7f/10f) );
            getSymbolWeights.Add( Constants.GET_TERMINAL, x => (2f/10f) );
            getSymbolWeights.Add( Constants.EMPTY_TERMINAL, x => (1f/10f)  );

            exploreSymbolWeights.Add( Constants.SECRET_TERMINAL, x => (5f/6f) );
            exploreSymbolWeights.Add( Constants.EMPTY_TERMINAL, x => (1f/6f) );

            // string favoriteQuest = startSymbolWeights.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            // GetProfileFromFavoriteQuest(favoriteQuest);
        }
    }
}