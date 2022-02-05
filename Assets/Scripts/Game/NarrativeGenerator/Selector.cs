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

        public List<QuestWeight> questWeights = new List<QuestWeight>();
        Dictionary<string, int> questWeightsbyType = new Dictionary<string, int>();
        private static readonly int[] WEIGHTS = {1, 3, 5, 7};

        private PlayerProfile playerProfile;
        
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
            // TODO: ver onde isso deveria estar
            // questWeightsbyType.Add( PlayerProfile.PlayerProfileCategory.Achievement.ToString(), 3 );
            // questWeightsbyType.Add( PlayerProfile.PlayerProfileCategory.Creativity.ToString(), 7 );
            // questWeightsbyType.Add( PlayerProfile.PlayerProfileCategory.Mastery.ToString(), 1 );
            // questWeightsbyType.Add( PlayerProfile.PlayerProfileCategory.Immersion.ToString(), 5 );
        }        
        
        public PlayerProfile SelectProfile(NarrativeCreatorEventArgs eventArgs)
        {
            questWeightsbyType = eventArgs.QuestWeightsbyType;

            CreateProfileWithWeights();
            
            return playerProfile;
        }
        
        public void CreateMissions(QuestGeneratorManager m)
        {
            //pesos[0] = 3; //peso talk
            //pesos[1] = 7; //peso get
            //pesos[2] = 1; //peso kill
            //pesos[3] = 5; //peso explore
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

            // playerProfile.AchievementPreference = questWeightsbyType[PlayerProfile.PlayerProfileCategory.Achievement.ToString()];
            // playerProfile.MasteryPreference = questWeightsbyType[PlayerProfile.PlayerProfileCategory.Mastery.ToString()];
            // playerProfile.CreativityPreference = questWeightsbyType[PlayerProfile.PlayerProfileCategory.Creativity.ToString()];
            // playerProfile.ImmersionPreference = questWeightsbyType[PlayerProfile.PlayerProfileCategory.Immersion.ToString()];
            
            // string favoriteQuest = questWeightsbyType.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            string favoriteQuest = startSymbolWeights.Aggregate((x, y) => x.Value(0) > y.Value(0) ? x : y).Key;
            playerProfile.SetProfileFromFavoriteQuest(favoriteQuest);
        }

        private List<QuestSO> DrawMissions(List<NpcSO> possibleNpcs, TreasureRuntimeSetSO possibleTreasures, WeaponTypeRuntimeSetSO possibleEnemyTypes)
        {
            // Dictionary<string, Func<float,float>> symbolWeights = startSymbolWeights;
            // MarkovChain questChain = new MarkovChain();
            // while ( questChain.GetLastSymbol().canDrawNext )
            // {
            //     questChain.GetLastSymbol().SetDictionary( symbolWeights );
            //     questChain.GetLastSymbol().SetNextSymbol( questChain );

            //     if ( 
            //         questChain.GetLastSymbol() as QuestSO != null &&
            //         questChain.GetLastSymbol().symbolType != Constants.EMPTY_TERMINAL
            //     )
            //     {
            //         m.Quests.graph.Add( questChain.GetLastSymbol() as QuestSO );
            //     }

            //     switch ( questChain.GetLastSymbol().symbolType )
            //     {
            //         case Constants.KILL_QUEST:
            //             symbolWeights = killSymbolWeights;
            //         break;
            //         case Constants.TALK_QUEST:
            //             symbolWeights = talkSymbolWeights;
            //         break;
            //         case Constants.GET_QUEST:
            //             symbolWeights = getSymbolWeights;
            //         break;
            //         case Constants.EXPLORE_QUEST:
            //             symbolWeights = exploreSymbolWeights;
            //         break;
            //         case Constants.KILL_TERMINAL:
            //             symbolWeights = killSymbolWeights;
            //         break;
            //         case Constants.TALK_TERMINAL:
            //             symbolWeights = talkSymbolWeights;
            //         break;
            //         case Constants.EMPTY_TERMINAL:
            //         break;
            //         case Constants.GET_TERMINAL:
            //             symbolWeights = getSymbolWeights;
            //         break;
            //         case Constants.DROP_TERMINAL:
            //             symbolWeights = getSymbolWeights;
            //         break;
            //         case Constants.ITEM_TERMINAL:
            //             symbolWeights = getSymbolWeights;
            //         break;
            //         case Constants.SECRET_TERMINAL:
            //             symbolWeights = exploreSymbolWeights;
            //         break;
            //         default:
            //             Debug.LogError("Symbol type not found!");
            //         break;
            //     }
            // }
            // leo
            var questsSos = new List<QuestSO>();
            var newMissionDraw = 0.0f;
            var chainCost = 0;
            do
            {
                foreach (var item in questWeightsbyType.Where(item => item.Value > newMissionDraw))
                {
                    switch (item.Key)
                    {
                        case Constants.TALK_QUEST:
                            var t = new Talk(0, questWeightsbyType);
                            t.Option(questsSos, possibleNpcs);
                            break;
                        case Constants.GET_QUEST:
                            var g = new Get(0, questWeightsbyType);
                            g.Option(questsSos, possibleNpcs, possibleTreasures, possibleEnemyTypes);
                            break;
                        case Constants.KILL_QUEST:
                            var k = new Kill(0, questWeightsbyType);
                            k.Option(questsSos, possibleNpcs, possibleEnemyTypes);
                            break;
                        case Constants.EXPLORE_QUEST:
                            var e = new Explore(0, questWeightsbyType);
                            e.Option(questsSos, possibleNpcs);
                            break;
                    }
                }
                chainCost += (int)Enums.QuestWeights.Hated*2;
                newMissionDraw = RandomSingleton.GetInstance().Random.Next((int)Enums.QuestWeights.Loved)+chainCost;
            } while (chainCost < (int)Enums.QuestWeights.Loved);

            return questsSos;
        }

        private void CalculateProfileWeights(List<int> answers)
        {
            // // testing
            // answers.Clear();
            // answers.Add(0);
            // answers.Add(1);
            // answers.Add(2);
            // answers.Add(3);
            // answers.Add(4);
            // answers.Add(0); answers.Add(0);  answers.Add(0); answers.Add(0);  answers.Add(1); answers.Add(0);  answers.Add(0); // TESTES
            // //

            // float totalQuestionsWeight = answers[5] + answers[6] +  answers[7] + answers[8]  +  answers[9] + answers[10] + answers[11];
            // // Kill questions = 5, 6;
            // // Explore questions = 7, 8;
            // // Get questions = 9;
            // // Talk questions = 10, 11;
            // // Puzzle questions = 12, 13;

            // float killWeight = ( answers[5] + answers[6] )/ ( totalQuestionsWeight );
            // killWeight = float.IsNaN( killWeight ) ? 0 : killWeight;

            // float exploreWeight = ( answers[7] + answers[8] )/ ( totalQuestionsWeight );
            // exploreWeight = float.IsNaN(exploreWeight ) ? 0 : exploreWeight;

            // float getWeight = ( answers[9] )/ ( totalQuestionsWeight );
            // getWeight = float.IsNaN(getWeight ) ? 0 : getWeight;

            // float talkWeight = ( answers[10] + answers[11] )/ ( totalQuestionsWeight );
            // talkWeight = float.IsNaN(talkWeight ) ? 0 : talkWeight;

            // //float puzzleWeight = ( answers[12] + answers[13] )/ ( maxQuestionWeight * 2 );
            // //puzzleWeight = float.IsNaN(puzzleWeight ) ? 0 : puzzleWeight;

            // float[] pesos = new float[4];

            // if ( exploreWeight != 0 ) startSymbolWeights.Add( Constants.EXPLORE_QUEST, x => exploreWeight );
            // if ( killWeight != 0 ) startSymbolWeights.Add( Constants.KILL_QUEST, x => killWeight );
            // if ( getWeight != 0 ) startSymbolWeights.Add( Constants.GET_QUEST, x => getWeight );
            // if ( talkWeight != 0 ) startSymbolWeights.Add( Constants.TALK_QUEST, x => talkWeight );

            // killSymbolWeights.Add( Constants.KILL_TERMINAL, x => Mathf.Clamp( 1/(x*0.25f), 0, 1) );
            // killSymbolWeights.Add( Constants.EMPTY_TERMINAL, x => Mathf.Clamp( ( 1 -( 1/(x*0.25f))), 0, 1));

            // talkSymbolWeights.Add( Constants.TALK_TERMINAL, x => Mathf.Clamp( 1/(x*0.25f), 0, 1) );
            // talkSymbolWeights.Add( Constants.EMPTY_TERMINAL, x => Mathf.Clamp( ( 1 -( 1/(x*0.25f))), 0, 1));

            // getSymbolWeights.Add( Constants.ITEM_TERMINAL, x => Mathf.Clamp( 0.3f*(1/(x*0.25f)), 0, .3f));
            // getSymbolWeights.Add( Constants.DROP_TERMINAL, x => Mathf.Clamp( 0.3f*(1/(x*0.25f)), 0, .3f));
            // getSymbolWeights.Add( Constants.GET_TERMINAL, x => Mathf.Clamp(  0.3f*(1/(x*0.25f)), 0, .3f));
            // getSymbolWeights.Add( Constants.EMPTY_TERMINAL, x => Mathf.Clamp( ( 1 -( 1/(x*0.25f))), 0, 1));

            // exploreSymbolWeights.Add( Constants.SECRET_TERMINAL, x => Mathf.Clamp( 1/(x*0.25f), 0, 1) );
            // exploreSymbolWeights.Add( Constants.EMPTY_TERMINAL, x => Mathf.Clamp( ( 1 -( 1/(x*0.25f))), 0, 1));

            // string favoriteQuest = startSymbolWeights.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            // GetProfileFromFavoriteQuest(favoriteQuest);
            //leo
            var pesos = new int[4];

            for (var i = 2; i < 12; i++)
            {
                switch (i)
                {
                    case 2:
                        pesos[2] += answers[i]-3;
                        break;
                    case 3:
                    case 4:
                        pesos[2] += answers[i];
                        break;
                    case 5:
                    case 6:
                        pesos[3] += answers[i];
                        break;
                    case 7:
                    case 8:
                        pesos[1] += answers[i];
                        break;
                    case 9:
                    case 10:
                        pesos[0] += answers[i];
                        break;
                    default:
                        pesos[3] -= answers[i]-3;
                        pesos[1] -= answers[i]-3;
                        pesos[0] -= answers[i]-3;
                        break;
                }
            }

            questWeights.Add(new QuestWeight(Constants.TALK_QUEST, pesos[0]));
            questWeights.Add(new QuestWeight(Constants.GET_QUEST, pesos[1]));
            questWeights.Add(new QuestWeight(Constants.KILL_QUEST, pesos[2]));
            questWeights.Add(new QuestWeight(Constants.EXPLORE_QUEST, pesos[3]));

            questWeights = questWeights.OrderBy(x => x.weight).ToList();

            for (int i = 0; i < questWeights.Count; ++i)
            {
                questWeights[i].weight = WEIGHTS[i];
                Debug.Log($"Quest Weight [{i}]: {questWeights[i].weight}");
            }

            pesos[0] = questWeights.Find(x => x.quest == Constants.TALK_QUEST).weight;
            pesos[1] = questWeights.Find(x => x.quest == Constants.GET_QUEST).weight;
            pesos[2] = questWeights.Find(x => x.quest == Constants.KILL_QUEST).weight;
            pesos[3] = questWeights.Find(x => x.quest == Constants.EXPLORE_QUEST).weight;

            questWeightsbyType.Add(PlayerProfile.PlayerProfileCategory.Immersion.ToString(), pesos[0]);
            questWeightsbyType.Add(PlayerProfile.PlayerProfileCategory.Achievement.ToString(), pesos[1]);
            questWeightsbyType.Add(PlayerProfile.PlayerProfileCategory.Mastery.ToString(), pesos[2]);
            questWeightsbyType.Add(PlayerProfile.PlayerProfileCategory.Creativity.ToString(), pesos[3]);
        }
    }
}