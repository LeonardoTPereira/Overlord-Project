using System;
using System.Text;
#if !UNITY_WEBGL || UNITY_EDITOR
using Firebase.Firestore;
#endif

namespace Game.NarrativeGenerator
{
    #if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreData]
    #endif
    public class PlayerProfile
    {
        public enum PlayerProfileCategory
        {
            Mastery, //Challenge, Enemies
            Immersion, //Story, NPCs
            Creativity, //Discovery, Exploration
            Achievement // Completion, Items
        }

        #if !UNITY_WEBGL || UNITY_EDITOR
            [FirestoreProperty]
        #endif 
        public PlayerProfileCategory PlayerProfileEnum { get; set; }
        #if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
        #endif 
        public float MasteryPreference { get; set; }
        #if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
        #endif 
        public float ImmersionPreference { get; set; }
        #if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
        #endif 
        public float CreativityPreference { get; set; }
        #if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
        #endif 
        public float AchievementPreference { get; set; }

        public void SetProfileFromFavoriteQuest(string favoriteQuest)
        {
            PlayerProfileEnum = (PlayerProfileCategory) Enum.Parse(typeof(PlayerProfileCategory), favoriteQuest);
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Category: " + PlayerProfileEnum);
            stringBuilder.Append("Kill: " + MasteryPreference);
            stringBuilder.Append("Talk: " + ImmersionPreference);
            stringBuilder.Append("Get: " + AchievementPreference);
            stringBuilder.Append("Explore: " + CreativityPreference);
            return base.ToString();
        }
    }
}