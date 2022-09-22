using System;
using System.Collections.Generic;
using System.Linq;
using Game.DataCollection;
using Game.Events;
using Util;
using static Util.Enums;

namespace Game.NarrativeGenerator
{
    public static class ProfileCalculator
    {
        private static Dictionary<string, int> _questWeightsByType = new ();
        public static Dictionary<string, Func<int, int>> StartSymbolWeights { get; private set; }


        public static PlayerProfile CreateProfile(List<int> answers, bool enableRandomProfileToPlayer, int probabilityToGetTrueProfile)
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
        
        public static PlayerProfile CreateProfile(NarrativeCreatorEventArgs eventArgs)
        {
            _questWeightsByType = eventArgs.QuestWeightsbyType;
            return CreateProfileWithWeights();

        }
        
        public static PlayerProfile CreateProfile(PlayerData playerData, DungeonData dungeonData)
        {
            CalculateProfileFromGameplayData(playerData, dungeonData);
            return CreateProfileWithWeights();
        }

        private static void CalculateProfileFromGameplayData(PlayerData playerData, DungeonData dungeonData)
        {
            StartSymbolWeights = new Dictionary<string, Func<int, int>>();

            _questWeightsByType = new Dictionary<string, int>
            {
                {PlayerProfile.PlayerProfileCategory.Immersion.ToString(), 0},
                {PlayerProfile.PlayerProfileCategory.Achievement.ToString(), 0},
                {PlayerProfile.PlayerProfileCategory.Mastery.ToString(), 0},
                {PlayerProfile.PlayerProfileCategory.Creativity.ToString(), 0}
            };

            _questWeightsByType[PlayerProfile.PlayerProfileCategory.Mastery.ToString()] =
                QuestWeightsCalculator.GetMasteryWeight(playerData.TotalDeaths, playerData.TotalAttempts, playerData.TotalLostHealth);
            _questWeightsByType[PlayerProfile.PlayerProfileCategory.Achievement.ToString()] = 
                QuestWeightsCalculator.GetAchievementWeight(playerData.EnemiesKilled, playerData.TotalEnemies, playerData.TreasuresCollected, playerData.TotalTreasure);
            _questWeightsByType[PlayerProfile.PlayerProfileCategory.Immersion.ToString()] = 
                QuestWeightsCalculator.GetImmersionWeight(playerData.NpcsInteracted, playerData.TotalNpcs);
            _questWeightsByType[PlayerProfile.PlayerProfileCategory.Creativity.ToString()] = 
                QuestWeightsCalculator.GetCreativityWeight(playerData.UniqueRoomsEntered, playerData.TotalRooms, playerData.LocksOpened, playerData.TotalLocks);
        }
        
        private static void CalculateProfileWeights(List<int> answers)
        {
            var weightsFromAnswers = CalculateStartSymbolWeights( answers );
            _questWeightsByType.Add(PlayerProfile.PlayerProfileCategory.Immersion.ToString(), weightsFromAnswers[0]);
            _questWeightsByType.Add(PlayerProfile.PlayerProfileCategory.Achievement.ToString(), weightsFromAnswers[1]);
            _questWeightsByType.Add(PlayerProfile.PlayerProfileCategory.Mastery.ToString(), weightsFromAnswers[2]);
            _questWeightsByType.Add(PlayerProfile.PlayerProfileCategory.Creativity.ToString(), weightsFromAnswers[3]);
        }
        
        private static void CalculateFakeProfile(List<int> answers)
        {
            //TODO make logic circle at every new dungeon
            var weightsFromAnswers = CalculateStartSymbolWeights( answers );
            _questWeightsByType.Add(PlayerProfile.PlayerProfileCategory.Immersion.ToString(), weightsFromAnswers[3]);
            _questWeightsByType.Add(PlayerProfile.PlayerProfileCategory.Achievement.ToString(), weightsFromAnswers[2]);
            _questWeightsByType.Add(PlayerProfile.PlayerProfileCategory.Mastery.ToString(), weightsFromAnswers[1]);
            _questWeightsByType.Add(PlayerProfile.PlayerProfileCategory.Creativity.ToString(), weightsFromAnswers[0]);
        }

        private static int[] CalculateStartSymbolWeights ( List<int> answers )
        {
            
            var immersionPreference = QuestWeightsCalculator.GetWeightFromPreTest( answers[2] );
            var achievementPreference = QuestWeightsCalculator.GetWeightFromPreTest( answers[0] );
            var masteryPreference = QuestWeightsCalculator.GetWeightFromPreTest( answers[3] );
            var creativityPreference = QuestWeightsCalculator.GetWeightFromPreTest( answers[1] );

            var normalizeConst = immersionPreference + achievementPreference + masteryPreference + creativityPreference;

            var talkWeight = (int) (100*(immersionPreference/normalizeConst));
            var getWeight = (int) (100*(achievementPreference/normalizeConst));
            var killWeight = (int) (100*(masteryPreference/normalizeConst));
            var exploreWeight = (int) (100*(creativityPreference/normalizeConst));

            int [] startSymbolWeights = {talkWeight, getWeight, killWeight, exploreWeight};
            return startSymbolWeights;
        }

        private static void CalculateStartSymbolWeights ( PlayerProfile playerProfile )
        {
            var creativityPreference = RemoveZeros( playerProfile.CreativityPreference );
            var achievementPreference = RemoveZeros( playerProfile.AchievementPreference );
            var masteryPreference = RemoveZeros( playerProfile.MasteryPreference );
            var immersionPreference = RemoveZeros( playerProfile.ImmersionPreference );

            var normalizeConst = creativityPreference + achievementPreference;
            normalizeConst += masteryPreference + immersionPreference;
            
            var talkWeight = (int) RemoveZeros( (100*creativityPreference/normalizeConst) );
            var getWeight = (int) RemoveZeros( (100*achievementPreference/normalizeConst) );
            var killWeight = (int) RemoveZeros( (100*masteryPreference/normalizeConst) );
            var exploreWeight = (int) RemoveZeros( (100*immersionPreference/normalizeConst) );

            StartSymbolWeights = new Dictionary<string, Func<int, int>>
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
        
        private static PlayerProfile CreateProfileWithWeights()
        {
            var playerProfile = new PlayerProfile
            {
                AchievementPreference = _questWeightsByType[PlayerProfile.PlayerProfileCategory.Achievement.ToString()],
                MasteryPreference = _questWeightsByType[PlayerProfile.PlayerProfileCategory.Mastery.ToString()],
                CreativityPreference = _questWeightsByType[PlayerProfile.PlayerProfileCategory.Creativity.ToString()],
                ImmersionPreference = _questWeightsByType[PlayerProfile.PlayerProfileCategory.Immersion.ToString()]
            };

            CalculateStartSymbolWeights ( playerProfile );
            var favoriteQuest = _questWeightsByType.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            playerProfile.SetProfileFromFavoriteQuest(favoriteQuest);
            return playerProfile;
        }
    }
}