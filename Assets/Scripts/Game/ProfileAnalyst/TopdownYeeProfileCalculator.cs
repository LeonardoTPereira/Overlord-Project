using Game.DataCollection;
using Game.Events;
using Game.ExperimentControllers;
using Game.NarrativeGenerator;
using Overlord.ProfileAnalyst;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Overlord.ProfileAnalyst
{
    public class TopdownYeeProfileCalculator: YeeProfileCalculator
    {
        public IPlayerProfile CreateProfileFromFormAnswers(List<int> answers, GeneratorSettings settings)
        {
            // if (settings.EnableRandomProfileToPlayer)
            // {
            //     if (RandomSingleton.GetInstance().Random.Next(100) < settings.ProbabilityToGetTrueProfile)
            //     {
                    CalculateProfileWeights(answers);
            //     }
            //     else
            //     {
            //         CalculateFakeProfile(answers);
            //     }
            // }
            // else
            // {
            //     CalculateProfileWeights(answers);
            // }
            return CreateProfileWithWeights();
        }

        public IPlayerProfile CreateProfileFromNarrative(NarrativeCreatorEventArgs eventArgs)
        {
            _questWeightsByType = eventArgs.QuestWeightsbyType;
            return CreateProfileWithWeights();
        }
        
        public IPlayerProfile CreateProfileFromGameplay(PlayerData playerData, DungeonData dungeonData)
        {
            CalculateProfileFromGameplayData(playerData, dungeonData);
            return CreateProfileWithWeights();
        }

        private static void CalculateProfileFromGameplayData(PlayerData playerData, DungeonData dungeonData)
        {
            StartSymbolWeights = new Dictionary<string, Func<int, float>>();

            _questWeightsByType = new Dictionary<string, float>
            {
                {YeePlayerProfile.PlayerProfileCategory.Immersion.ToString(), 0},
                {YeePlayerProfile.PlayerProfileCategory.Achievement.ToString(), 0},
                {YeePlayerProfile.PlayerProfileCategory.Mastery.ToString(), 0},
                {YeePlayerProfile.PlayerProfileCategory.Creativity.ToString(), 0}
            };

            _questWeightsByType[YeePlayerProfile.PlayerProfileCategory.Mastery.ToString()] =
                QuestWeightsCalculator.GetMasteryWeight(
                    playerData.SerializedData.PlayerProfile.MasteryPreference,
                    playerData.SerializedData.EnemiesKilled,
                    playerData.SerializedData.TotalEnemies,
                    playerData.SerializedData.TotalLostHealth,
                    playerData.SerializedData.InitialHealth
                );

            _questWeightsByType[YeePlayerProfile.PlayerProfileCategory.Achievement.ToString()] = 
                QuestWeightsCalculator.GetAchievementWeight(
                    playerData.SerializedData.PlayerProfile.AchievementPreference,
                    playerData.SerializedData.EnemiesKilled,
                    playerData.SerializedData.TotalEnemies,
                    playerData.SerializedData.TreasuresCollected,
                    playerData.SerializedData.TotalCollectableItems,
                    playerData.SerializedData.UniqueRoomsEntered,
                    playerData.SerializedData.TotalRooms
                );


            _questWeightsByType[YeePlayerProfile.PlayerProfileCategory.Immersion.ToString()] = 
                QuestWeightsCalculator.GetImmersionWeight(
                    playerData.SerializedData.PlayerProfile.ImmersionPreference,
                    playerData.SerializedData.CompletedImmersionQuests,
                    playerData.SerializedData.TotalImmersionQuests
                );

            _questWeightsByType[YeePlayerProfile.PlayerProfileCategory.Creativity.ToString()] = 
                QuestWeightsCalculator.GetCreativityWeight(
                    playerData.SerializedData.PlayerProfile.CreativityPreference,
                    playerData.SerializedData.UniqueRoomsEntered,
                    playerData.SerializedData.TotalRooms,
                    playerData.SerializedData.LocksOpened,
                    playerData.SerializedData.TotalLocks
                );
        }
        
        private static void CalculateProfileWeights(List<int> answers)
        {
            _questWeightsByType = new Dictionary<string, float>();
            var weightsFromAnswers = CalculateStartSymbolWeights( answers );
            _questWeightsByType.Add(YeePlayerProfile.PlayerProfileCategory.Immersion.ToString(), (int) weightsFromAnswers[0]);
            _questWeightsByType.Add(YeePlayerProfile.PlayerProfileCategory.Achievement.ToString(), (int) weightsFromAnswers[1]);
            _questWeightsByType.Add(YeePlayerProfile.PlayerProfileCategory.Mastery.ToString(), (int) weightsFromAnswers[2]);
            _questWeightsByType.Add(YeePlayerProfile.PlayerProfileCategory.Creativity.ToString(), (int) weightsFromAnswers[3]);
        }
        
        private static void CalculateFakeProfile(List<int> answers)
        {
            _questWeightsByType = new Dictionary<string, float>();
            //TODO make logic circle at every new dungeon
            var weightsFromAnswers = CalculateStartSymbolWeights( answers );
            _questWeightsByType.Add(YeePlayerProfile.PlayerProfileCategory.Immersion.ToString(), (int) weightsFromAnswers[3]);
            _questWeightsByType.Add(YeePlayerProfile.PlayerProfileCategory.Achievement.ToString(), (int) weightsFromAnswers[2]);
            _questWeightsByType.Add(YeePlayerProfile.PlayerProfileCategory.Mastery.ToString(), (int) weightsFromAnswers[1]);
            _questWeightsByType.Add(YeePlayerProfile.PlayerProfileCategory.Creativity.ToString(), (int) weightsFromAnswers[0]);
        }

        private static float[] CalculateStartSymbolWeights(List<int> answers)
        {

            float immersionPreference = QuestWeightsCalculator.GetWeightFromPreTest(answers[2]);
            float achievementPreference = QuestWeightsCalculator.GetWeightFromPreTest(answers[0]);
            float masteryPreference = QuestWeightsCalculator.GetWeightFromPreTest(answers[3]);
            float creativityPreference = QuestWeightsCalculator.GetWeightFromPreTest(answers[1]);

            float normalizeConst = immersionPreference + achievementPreference + masteryPreference + creativityPreference;

            float talkWeight = (100 * (immersionPreference / normalizeConst));
            float getWeight = (100 * (achievementPreference / normalizeConst));
            float killWeight = (100 * (masteryPreference / normalizeConst));
            float exploreWeight = (100 * (creativityPreference / normalizeConst));

            float[] startSymbolWeights = { talkWeight, getWeight, killWeight, exploreWeight };
            return startSymbolWeights;
        }
    }
}