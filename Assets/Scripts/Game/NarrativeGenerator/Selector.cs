using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Enums;
using static Util;

namespace Game.NarrativeGenerator
{
//classe que seleciona a linha de missões de acordo com os pesos do perfil do jogador
    public class Selector
    {

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

        private PlayerProfileEnum typePlayer;

        /*
        [7][5][1][3]
        [3][7][1][5]
        [1][5][7][3]
        [1][5][3][7]
        */

        public PlayerProfileEnum Select(Manager m, List<int> answers)
        {
            //pesos[0] = 3; //peso talk
            //pesos[1] = 7; //peso get
            //pesos[2] = 1; //peso kill
            //pesos[3] = 5; //peso explore

            weightCalculator(answers);

            DrawMissions(m);

            return typePlayer;
        }

        public PlayerProfileEnum Select(Manager m, NarrativeCreatorEventArgs eventArgs)
        {
            questWeightsbyType = eventArgs.QuestWeightsbyType;

            string favoriteQuest = questWeightsbyType.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

            GetProfileFromFavoriteQuest(favoriteQuest);

            DrawMissions(m);

            return typePlayer;
        }

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
            float newMissionChance, newMissionDraw;
            newMissionChance = 0;
            do
            {
                foreach (var item in questWeightsbyType)
                {
                    newMissionDraw = UnityEngine.Random.Range(0, 1.0f);
                    if (item.Value > newMissionDraw * 10)
                    {
                        switch (item.Key)
                        {
                            case TALK_QUEST:
                                Talk t = new Talk(0, questWeightsbyType);
                                t.Option(m);
                                break;
                            case GET_QUEST:
                                Get g = new Get(0, questWeightsbyType);
                                g.Option(m);
                                break;
                            case KILL_QUEST:
                                Kill k = new Kill(0, questWeightsbyType);
                                k.Option(m);
                                break;
                            case EXPLORE_QUEST:
                                Explore e = new Explore(0, questWeightsbyType);
                                e.Option(m);
                                break;
                        }
                    }
                }

                newMissionDraw = UnityEngine.Random.Range(0, 1.0f);
                newMissionChance += 0.5f;
            } while (newMissionDraw > newMissionChance);
        }

        private void weightCalculator(List<int> answers)
        {
            int[] pesos = new int[4];

            for (int i = 2; i < 12; i++)
            {
                if (i == 2 || i == 3 || i == 4) pesos[2] += answers[i];
                else if (i == 5 || i == 6) pesos[3] += answers[i];
                else if (i == 7 || i == 8) pesos[1] += answers[i];
                else if (i == 9 || i == 10) pesos[0] += answers[i];
                else
                {
                    pesos[3] -= answers[i];
                    pesos[1] -= answers[i];
                    pesos[0] -= answers[i];
                }
            }

            questWeights.Add(new QuestWeight(TALK_QUEST, pesos[0]));
            questWeights.Add(new QuestWeight(GET_QUEST, pesos[1]));
            questWeights.Add(new QuestWeight(KILL_QUEST, pesos[2]));
            questWeights.Add(new QuestWeight(EXPLORE_QUEST, pesos[3]));

            questWeights = questWeights.OrderBy(x => x.weight).ToList();

            for (int i = 0; i < questWeights.Count; ++i)
            {
                questWeights[i].weight = WEIGHTS[i];
                Debug.Log($"Quest Weight [{i}]: {questWeights[i].weight}");
            }

            pesos[0] = questWeights.Find(x => x.quest == TALK_QUEST).weight;
            pesos[1] = questWeights.Find(x => x.quest == GET_QUEST).weight;
            pesos[2] = questWeights.Find(x => x.quest == KILL_QUEST).weight;
            pesos[3] = questWeights.Find(x => x.quest == EXPLORE_QUEST).weight;

            questWeightsbyType.Add(TALK_QUEST, pesos[0]);
            questWeightsbyType.Add(GET_QUEST, pesos[1]);
            questWeightsbyType.Add(KILL_QUEST, pesos[2]);
            questWeightsbyType.Add(EXPLORE_QUEST, pesos[3]);

            string favoriteQuest = questWeightsbyType.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            GetProfileFromFavoriteQuest(favoriteQuest);
        }
    }
}