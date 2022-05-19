using System.Collections.Generic;
using System.Linq;
using Game.DataCollection;
using Game.Events;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarNonterminals;
using Game.NPCs;
using ScriptableObjects;
using Util;
using Enums = Util.Enums;

namespace Game.NarrativeGenerator
{
//classe que seleciona a linha de missões de acordo com os pesos do perfil do jogador
    public class Selector
    {

        private Dictionary<string, int> _questWeightsbyType;

        private PlayerProfile playerProfile;
        
        public PlayerProfile SelectProfile(List<int> answers)
        {
            _questWeightsbyType = ProfileWeightCalculator.CalculateProfileWeights(answers);

            CreateProfileWithWeights();
            
            return playerProfile;
        }        
        
        public PlayerProfile SelectProfile(NarrativeCreatorEventArgs eventArgs)
        {
            _questWeightsbyType = eventArgs.QuestWeightsbyType;

            CreateProfileWithWeights();
            
            return playerProfile;
        }
        
        public PlayerProfile SelectProfile(PlayerData playerData, DungeonData dungeonData)
        {
            _questWeightsbyType = ProfileWeightCalculator.CalculateProfileFromGameplayData(playerData, dungeonData);
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
                AchievementPreference = _questWeightsbyType[PlayerProfile.PlayerProfileCategory.Achievement.ToString()],
                MasteryPreference = _questWeightsbyType[PlayerProfile.PlayerProfileCategory.Mastery.ToString()],
                CreativityPreference = _questWeightsbyType[PlayerProfile.PlayerProfileCategory.Creativity.ToString()],
                ImmersionPreference = _questWeightsbyType[PlayerProfile.PlayerProfileCategory.Immersion.ToString()]
            };

            string favoriteQuest = _questWeightsbyType.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            playerProfile.SetProfileFromFavoriteQuest(favoriteQuest);
        }

        private List<QuestSO> DrawMissions(List<NpcSo> possibleNpcs, TreasureRuntimeSetSO possibleTreasures, WeaponTypeRuntimeSetSO possibleEnemyTypes)
        {
            var questsSos = new List<QuestSO>();
            var newMissionDraw = 0.0f;
            var chainCost = 0;
            do
            {
                foreach (var item in _questWeightsbyType.Where(item => item.Value > newMissionDraw))
                {
                    switch (item.Key)
                    {
                        case Constants.TALK_QUEST:
                            var t = new Talk(0, _questWeightsbyType);
                            t.Option(questsSos, possibleNpcs);
                            break;
                        case Constants.GET_QUEST:
                            var g = new Get(0, _questWeightsbyType);
                            g.Option(questsSos, possibleNpcs, possibleTreasures, possibleEnemyTypes);
                            break;
                        case Constants.KILL_QUEST:
                            var k = new Kill(0, _questWeightsbyType);
                            k.Option(questsSos, possibleNpcs, possibleEnemyTypes);
                            break;
                        case Constants.EXPLORE_QUEST:
                            var e = new Explore(0, _questWeightsbyType);
                            e.Option(questsSos, possibleNpcs);
                            break;
                    }
                }
                chainCost += (int)Enums.QuestWeights.Hated*2;
                newMissionDraw = RandomSingleton.GetInstance().Random.Next((int)Enums.QuestWeights.Disliked)+chainCost;
            } while (chainCost < (int)Enums.QuestWeights.Loved*2);

            return questsSos;
        }
    }
}