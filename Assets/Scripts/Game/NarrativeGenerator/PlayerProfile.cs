using System;

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
            PlayerProfileEnum = (PlayerProfileCategory) Enum.Parse(typeof(PlayerProfileCategory), favoriteQuest);
        }
    }
}