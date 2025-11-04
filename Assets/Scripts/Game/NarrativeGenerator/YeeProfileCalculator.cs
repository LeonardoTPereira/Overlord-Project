using Game.DataCollection;
using Game.Events;
using Game.ExperimentControllers;
using Game.NarrativeGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using Util;
using static Util.Enums;
using Overlord.ProfileAnalyst;

namespace Game.Overlord.ProfileAnalyst
{
    public class YeeProfileCalculator: IPlayerProfileCalculator
    {
        private static Dictionary<string, float> _questWeightsByType;
        public static Dictionary<string, Func<int, float>> StartSymbolWeights { get; private set; }

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

        private static float[] CalculateStartSymbolWeights ( List<int> answers )
        {
            
            float immersionPreference = QuestWeightsCalculator.GetWeightFromPreTest( answers[2] );
            float achievementPreference = QuestWeightsCalculator.GetWeightFromPreTest( answers[0] );
            float masteryPreference = QuestWeightsCalculator.GetWeightFromPreTest( answers[3] );
            float creativityPreference = QuestWeightsCalculator.GetWeightFromPreTest( answers[1] );

            float normalizeConst = immersionPreference + achievementPreference + masteryPreference + creativityPreference;

            float talkWeight = (100*(immersionPreference/normalizeConst));
            float getWeight = (100*(achievementPreference/normalizeConst));
            float killWeight = (100*(masteryPreference/normalizeConst));
            float exploreWeight = (100*(creativityPreference/normalizeConst));

            float [] startSymbolWeights = {talkWeight, getWeight, killWeight, exploreWeight};
            return startSymbolWeights;
        }

        private static void CalculateStartSymbolWeights ( YeePlayerProfile playerProfile )
        {
            float creativityPreference = RemoveZeros( playerProfile.CreativityPreference );
            float achievementPreference = RemoveZeros( playerProfile.AchievementPreference );
            float masteryPreference = RemoveZeros( playerProfile.MasteryPreference );
            float immersionPreference = RemoveZeros( playerProfile.ImmersionPreference );

            float normalizeConst = creativityPreference + achievementPreference;
            normalizeConst += masteryPreference + immersionPreference;
            
            float talkWeight = RemoveZeros( (100*immersionPreference/normalizeConst) );
            float getWeight = RemoveZeros( (100*achievementPreference/normalizeConst) );
            float killWeight = RemoveZeros( (100*masteryPreference/normalizeConst) );
            float exploreWeight = RemoveZeros( (100*creativityPreference/normalizeConst) );

            StartSymbolWeights = new Dictionary<string, Func<int, float>>
            {
                {Constants.ImmersionQuest, _ => talkWeight},
                {Constants.AchievementQuest, _ => getWeight},
                {Constants.MasteryQuest, _ => killWeight},
                {Constants.CreativityQuest, _ => exploreWeight}
            };
        }

        private static float RemoveZeros ( float playerPreference )
        {
            if ( playerPreference > 1 )
            {
                return playerPreference;
            }
            return (float) QuestWeights.Hated;
        }
        
        private static YeePlayerProfile CreateProfileWithWeights()
        {
            var playerProfile = new YeePlayerProfile
            {
                AchievementPreference = _questWeightsByType[YeePlayerProfile.PlayerProfileCategory.Achievement.ToString()],
                MasteryPreference = _questWeightsByType[YeePlayerProfile.PlayerProfileCategory.Mastery.ToString()],
                CreativityPreference = _questWeightsByType[YeePlayerProfile.PlayerProfileCategory.Creativity.ToString()],
                ImmersionPreference = _questWeightsByType[YeePlayerProfile.PlayerProfileCategory.Immersion.ToString()]
            };

            CalculateStartSymbolWeights ( playerProfile );
            var favoriteQuest = _questWeightsByType.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            playerProfile.SetProfileFromFavoriteQuest(favoriteQuest);
            playerProfile.Normalize();
            return playerProfile;
        }
    }
}