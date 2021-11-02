using UnityEngine;
using Util;

namespace Game.NarrativeGenerator
{
    public class PlayerProfile
    {
        public enum PlayerProfileCategory
        {
            Mastery, //Challenge, Enemies
            Immersion, //Story, NPCs
            Creativity, //Discovery, Exploration
            Achievement // Completion, Items
        }

        public PlayerProfileCategory PlayerProfileEnum { get; set; }
        public float MasteryPreference { get; set; }
        public float ImmersionPreference { get; set; }
        public float CreativityPreference { get; set; }
        public float AchievementPreference { get; set; }
        
        public void SetProfileFromFavoriteQuest(string favoriteQuest)
        {
            switch (favoriteQuest)
            {
                case Constants.KILL_QUEST:
                    PlayerProfileEnum = PlayerProfileCategory.Mastery;
                    break;
                case Constants.GET_QUEST:
                    PlayerProfileEnum = PlayerProfileCategory.Achievement;
                    break;
                case Constants.TALK_QUEST:
                    PlayerProfileEnum = PlayerProfileCategory.Immersion;
                    break;
                case Constants.EXPLORE_QUEST:
                    PlayerProfileEnum = PlayerProfileCategory.Creativity;
                    break;
                default:
                    Debug.Log("Something went wrong");
                    break;
            }
        }
    }
}