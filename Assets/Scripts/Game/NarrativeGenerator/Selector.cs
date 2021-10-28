using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Enums;
using static Util;
using Game.NarrativeGenerator.Quests;

namespace Game.NarrativeGenerator
{
//classe que seleciona a linha de missões de acordo com os pesos do perfil do jogador
    public class Selector
    {
        public MarkovChain questChain = new MarkovChain();
        Dictionary<float,SymbolType> startSymbolWeights = new Dictionary<float,SymbolType>();
        Dictionary<float,SymbolType> killSymbolWeights = new Dictionary<float,SymbolType>();
        Dictionary<float,SymbolType> talkSymbolWeights = new Dictionary<float,SymbolType>();
        Dictionary<float,SymbolType> getSymbolWeights = new Dictionary<float,SymbolType>();
        Dictionary<float,SymbolType> exploreSymbolWeights = new Dictionary<float,SymbolType>();
        private PlayerProfileEnum typePlayer;

        public PlayerProfileEnum Select(Manager m, List<int> answers)
        {
            weightCalculator(answers);

            DrawMissions(m);

            return typePlayer;
        }

        // public PlayerProfileEnum Select(Manager m, NarrativeCreatorEventArgs eventArgs)
        // {
        //     // startSymbolWeights = eventArgs.startSymbolWeights;

        //     // string favoriteQuest = startSymbolWeights.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

        //     // GetProfileFromFavoriteQuest(favoriteQuest);

        //     DrawMissions(m);

        //     // return typePlayer;
        // }

        private void GetProfileFromFavoriteQuest(string favoriteQuest)
        {
            switch (favoriteQuest)
            {
                case KILL_QUEST:
                    typePlayer = PlayerProfileEnum.Mastery;
                    break;
                case GET_QUEST:
                    typePlayer = PlayerProfileEnum.Achievement;
                    break;
                case TALK_QUEST:
                    typePlayer = PlayerProfileEnum.Immersion;
                    break;
                case EXPLORE_QUEST:
                    typePlayer = PlayerProfileEnum.Creativity;
                    break;
                default:
                    Debug.Log("Something went wrong");
                    break;
            }
        }

        private void DrawMissions(Manager m)
        {
            Dictionary<float,SymbolType> symbolWeights = startSymbolWeights;

            while ( questChain.symbol.canDrawNext )
            {
                questChain.symbol.SetDictionary( symbolWeights );
                questChain.symbol.SetNextSymbol( questChain );

                switch ( questChain.symbolType )
                {
                    case SymbolType.Kill:
                        
                    break;
                    case SymbolType.Talk:

                    break;
                    case SymbolType.Get:

                    break;
                    case SymbolType.Explore:

                    break;
                    case SymbolType.kill:

                    break;
                    case SymbolType.talk:

                    break;
                    case SymbolType.empty:

                    break;
                    case SymbolType.get:

                    break;
                    case SymbolType.drop:

                    break;
                    case SymbolType.item:

                    break;
                    case SymbolType.secret:

                    break;
                    default:
                        Debug.LogError("Symbol type not found!");
                    break;

                }
            }
        }

        private void weightCalculator(List<int> answers)
        {
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

            startSymbolWeights.Add(talkWeight, SymbolType.Talk);
            startSymbolWeights.Add(talkWeight + getWeight, SymbolType.Get );
            startSymbolWeights.Add( talkWeight + getWeight + killWeight, SymbolType.Kill );
            startSymbolWeights.Add( talkWeight + getWeight + killWeight + exploreWeight, SymbolType.Explore ); // 100%

            // Não sei dizer se isto é realmente necessário...
            // string favoriteQuest = startSymbolWeights.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            // GetProfileFromFavoriteQuest(favoriteQuest);
        }
    }
}