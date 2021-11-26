using System;
using System.Collections.Generic;
using System.Linq;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.nao_terminais;
using ScriptableObjects;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

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

            string favoriteQuest = questWeightsbyType.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            playerProfile.SetProfileFromFavoriteQuest(favoriteQuest);
        }

        private List<QuestSO> DrawMissions(List<NpcSO> possibleNpcs, TreasureRuntimeSetSO possibleTreasures, WeaponTypeRuntimeSetSO possibleEnemyTypes)
        {
            List<QuestSO> questsSos = new List<QuestSO>();
            float newMissionDraw = 0;
            float newMissionChance = 0;
            do
            {
                foreach (var item in questWeightsbyType)
                {
                    if (item.Value > newMissionDraw)
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
                    newMissionDraw = RandomSingleton.GetInstance().Random.Next(10);
                }
                newMissionChance += 2f;
            } while (newMissionDraw > newMissionChance);

            return questsSos;
        }

        private void CalculateProfileWeights(List<int> answers)
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