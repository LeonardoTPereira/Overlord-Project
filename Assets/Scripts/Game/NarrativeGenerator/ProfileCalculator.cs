using System;
using System.Collections.Generic;
using System.Linq;
using Game.DataCollection;
using Game.Events;
using UnityEngine;
using Util;
using static Util.Enums;

using UnityEngine;

namespace Game.NarrativeGenerator
{
    public static class ProfileCalculator
    {
        private static Dictionary<string, int> _questWeightsbyType = new ();
        public static Dictionary<string, Func<int, int>> StartSymbolWeights { get; private set; }

        public static PlayerProfile CreateProfile(List<int> answers)
        {
            CalculateProfileWeights(answers);
            return CreateProfileWithWeights();
        }        
        
        public static PlayerProfile CreateProfile(NarrativeCreatorEventArgs eventArgs)
        {
            _questWeightsbyType = eventArgs.QuestWeightsbyType;
            return CreateProfileWithWeights();

        }
        
        public static PlayerProfile CreateProfile(PlayerData playerData, DungeonData dungeonData)
        {
            CalculateProfileFromGameplayData(playerData, dungeonData);
            return CreateProfileWithWeights();
        }      
        
        public static void CalculateProfileFromGameplayData(PlayerData playerData, DungeonData dungeonData)
        {
            StartSymbolWeights = new Dictionary<string, Func<int, int>>();

            _questWeightsbyType = new Dictionary<string, int>
            {
                {PlayerProfile.PlayerProfileCategory.Immersion.ToString(), 0},
                {PlayerProfile.PlayerProfileCategory.Achievement.ToString(), 0},
                {PlayerProfile.PlayerProfileCategory.Mastery.ToString(), 0},
                {PlayerProfile.PlayerProfileCategory.Creativity.ToString(), 0}
            };

            _questWeightsbyType[PlayerProfile.PlayerProfileCategory.Mastery.ToString()] =
                QuestWeightsCalculator.GetMasteryWeight(playerData.TotalDeaths, playerData.TotalAttempts, playerData.TotalLostHealth);
            _questWeightsbyType[PlayerProfile.PlayerProfileCategory.Achievement.ToString()] = 
                QuestWeightsCalculator.GetAchievementWeight(playerData.EnemiesKilled, playerData.TotalEnemies, playerData.TreasuresCollected, playerData.TotalTreasure);
            _questWeightsbyType[PlayerProfile.PlayerProfileCategory.Immersion.ToString()] = 
                QuestWeightsCalculator.GetImmersionWeight(playerData.NpcsInteracted, playerData.TotalNpcs);
            _questWeightsbyType[PlayerProfile.PlayerProfileCategory.Creativity.ToString()] = 
                QuestWeightsCalculator.GetCreativityWeight(playerData.UniqueRoomsEntered, playerData.TotalRooms, playerData.LocksOpened, playerData.TotalLocks);
        }
        
        private static void CalculateProfileWeights(List<int> answers)
        {
            var weightsFromAnswers = CalculateStartSymbolWeights( answers );
            _questWeightsbyType.Add(PlayerProfile.PlayerProfileCategory.Immersion.ToString(), weightsFromAnswers[0]);
            _questWeightsbyType.Add(PlayerProfile.PlayerProfileCategory.Achievement.ToString(), weightsFromAnswers[1]);
            _questWeightsbyType.Add(PlayerProfile.PlayerProfileCategory.Mastery.ToString(), weightsFromAnswers[2]);
            _questWeightsbyType.Add(PlayerProfile.PlayerProfileCategory.Creativity.ToString(), weightsFromAnswers[3]);
        }

        private static int[] CalculateStartSymbolWeights ( List<int> answers )
        {
            int totalQuestionsWeight = QuestWeightsCalculator.CalculateTotalQuestionsWeight ( answers );

            int talkWeight = QuestWeightsCalculator.GetTalkQuestWeight( answers, totalQuestionsWeight );
            int getWeight = QuestWeightsCalculator.GetGetQuestWeight( answers, totalQuestionsWeight );
            int killWeight = QuestWeightsCalculator.GetKillQuestWeight( answers, totalQuestionsWeight );
            int exploreWeight = QuestWeightsCalculator.GetExploreQuestWeight( answers, totalQuestionsWeight );

            int [] startSymbolWeights = {talkWeight, getWeight, killWeight, exploreWeight};
            return startSymbolWeights;
        }

        private static void CalculateStartSymbolWeights ( PlayerProfile playerProfile )
        {
            float creativityPreference = RemoveZeros( playerProfile.CreativityPreference );
            float achievementPreference = RemoveZeros( playerProfile.AchievementPreference );
            float masteryPreference = RemoveZeros( playerProfile.MasteryPreference );
            float immersionPreference = RemoveZeros( playerProfile.ImmersionPreference );

            float normalizeConst = creativityPreference + achievementPreference;
            normalizeConst += masteryPreference + immersionPreference;
            
            int talkWeight = (int)(100*creativityPreference/normalizeConst);
            int getWeight = (int)(100*achievementPreference/normalizeConst);
            int killWeight = (int)(100*masteryPreference/normalizeConst);
            int exploreWeight = (int)(100*immersionPreference/normalizeConst);

            Debug.Log($"{talkWeight} {getWeight} {killWeight} {exploreWeight}");

            StartSymbolWeights = new Dictionary<string, Func<int, int>>();
            StartSymbolWeights.Add( Constants.TALK_QUEST, x => talkWeight );
            StartSymbolWeights.Add( Constants.GET_QUEST, x => getWeight );
            StartSymbolWeights.Add( Constants.KILL_QUEST, x => killWeight ); 
            StartSymbolWeights.Add( Constants.EXPLORE_QUEST, x => exploreWeight );
        }

        private static float RemoveZeros ( float playerPreference )
        {
            if ( playerPreference > 0 )
            {
                return playerPreference;
            }
            return (float) QuestWeights.Hated;
        }
        
        private static PlayerProfile CreateProfileWithWeights()
        {
            var playerProfile = new PlayerProfile
            {
                AchievementPreference = _questWeightsbyType[PlayerProfile.PlayerProfileCategory.Achievement.ToString()],
                MasteryPreference = _questWeightsbyType[PlayerProfile.PlayerProfileCategory.Mastery.ToString()],
                CreativityPreference = _questWeightsbyType[PlayerProfile.PlayerProfileCategory.Creativity.ToString()],
                ImmersionPreference = _questWeightsbyType[PlayerProfile.PlayerProfileCategory.Immersion.ToString()]
            };

            CalculateStartSymbolWeights ( playerProfile );
            var favoriteQuest = _questWeightsbyType.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            playerProfile.SetProfileFromFavoriteQuest(favoriteQuest);
            Debug.Log(playerProfile);
            return playerProfile;
        }
    }
}