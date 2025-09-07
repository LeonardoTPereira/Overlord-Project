using System;
using System.Collections.Generic;
using System.Linq;
using Game.DataCollection;
using Game.Events;
using Util;
using Game.NarrativeGenerator;
using static Util.Enums;

namespace Overlord.ProfileAnalyst
{
    public static class YeeProfileCalculator
    {
        private static Dictionary<string, int> _questWeightsByType;
        public static Dictionary<string, Func<int, float>> StartSymbolWeights { get; private set; }

        public static YeePlayerProfile CreateProfile(List<int> answers, bool enableRandomProfileToPlayer, int probabilityToGetTrueProfile)
        {
            if (enableRandomProfileToPlayer)
            {
                if (RandomSingleton.GetInstance().Random.Next(100) < probabilityToGetTrueProfile)
                {
                    CalculateProfileWeights(answers);
                }
                else
                {
                    CalculateFakeProfile(answers);
                }
            }
            else
            {
                CalculateProfileWeights(answers);
            }
            return CreateProfileWithWeights();
        }
        
        public static YeePlayerProfile CreateProfile(NarrativeCreatorEventArgs eventArgs)
        {
            _questWeightsByType = eventArgs.QuestWeightsbyType;
            return CreateProfileWithWeights();
        }
        
        public static YeePlayerProfile CreateProfile(PlayerData playerData, DungeonData dungeonData)
        {
            CalculateProfileFromGameplayData(playerData, dungeonData);
            return CreateProfileWithWeights();
        }

        private static void CalculateProfileFromGameplayData(PlayerData playerData, DungeonData dungeonData)
        {
            StartSymbolWeights = new Dictionary<string, Func<int, float>>();

            _questWeightsByType = new Dictionary<string, int>
            {
                {YeePlayerProfile.PlayerProfileCategory.Immersion.ToString(), 0},
                {YeePlayerProfile.PlayerProfileCategory.Achievement.ToString(), 0},
                {YeePlayerProfile.PlayerProfileCategory.Mastery.ToString(), 0},
                {YeePlayerProfile.PlayerProfileCategory.Creativity.ToString(), 0}
            };

            _questWeightsByType[YeePlayerProfile.PlayerProfileCategory.Mastery.ToString()] =
                QuestWeightsCalculator.GetMasteryWeight(playerData.SerializedData.TotalDeaths, playerData.SerializedData.TotalAttempts, playerData.SerializedData.TotalLostHealth);
            _questWeightsByType[YeePlayerProfile.PlayerProfileCategory.Achievement.ToString()] = 
                QuestWeightsCalculator.GetAchievementWeight(playerData.SerializedData.EnemiesKilled, playerData.SerializedData.TotalEnemies, playerData.SerializedData.TreasuresCollected, playerData.SerializedData.TotalTreasure);
            _questWeightsByType[YeePlayerProfile.PlayerProfileCategory.Immersion.ToString()] = 
                QuestWeightsCalculator.GetImmersionWeight(playerData.SerializedData.NpcsInteracted, playerData.SerializedData.TotalNpcs);
            _questWeightsByType[YeePlayerProfile.PlayerProfileCategory.Creativity.ToString()] = 
                QuestWeightsCalculator.GetCreativityWeight(playerData.SerializedData.UniqueRoomsEntered, playerData.SerializedData.TotalRooms, playerData.SerializedData.LocksOpened, playerData.SerializedData.TotalLocks);
        }
        
        private static void CalculateProfileWeights(List<int> answers)
        {
            _questWeightsByType = new Dictionary<string, int>();
            var weightsFromAnswers = CalculateStartSymbolWeights( answers );
            _questWeightsByType.Add(YeePlayerProfile.PlayerProfileCategory.Immersion.ToString(), (int) weightsFromAnswers[0]);
            _questWeightsByType.Add(YeePlayerProfile.PlayerProfileCategory.Achievement.ToString(), (int) weightsFromAnswers[1]);
            _questWeightsByType.Add(YeePlayerProfile.PlayerProfileCategory.Mastery.ToString(), (int) weightsFromAnswers[2]);
            _questWeightsByType.Add(YeePlayerProfile.PlayerProfileCategory.Creativity.ToString(), (int) weightsFromAnswers[3]);
        }
        
        private static void CalculateFakeProfile(List<int> answers)
        {
            _questWeightsByType = new Dictionary<string, int>();
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