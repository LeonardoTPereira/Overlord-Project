using UnityEngine;
using static Util;

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
                case KILL_QUEST:
                    PlayerProfileEnum = PlayerProfileCategory.Mastery;
                    break;
                case GET_QUEST:
                    PlayerProfileEnum = PlayerProfileCategory.Achievement;
                    break;
                case TALK_QUEST:
                    PlayerProfileEnum = PlayerProfileCategory.Immersion;
                    break;
                case EXPLORE_QUEST:
                    PlayerProfileEnum = PlayerProfileCategory.Creativity;
                    break;
                default:
                    Debug.Log("Something went wrong");
                    break;
            }
        }
    }
}