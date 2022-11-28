using System;
using System.Text;
using UnityEngine;
#if !UNITY_WEBGL || UNITY_EDITOR
using Firebase.Firestore;
#endif

namespace Game.NarrativeGenerator
{
    #if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreData]
    #endif
    [Serializable]
    public class PlayerProfile
    {
        [Serializable]
        public enum PlayerProfileCategory
        {
            Mastery, //Challenge, Enemies
            Immersion, //Story, NPCs
            Creativity, //Discovery, Exploration
            Achievement, // Completion, Items
            Null
        }
        
        #if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
        #endif 
        [field: SerializeField] public PlayerProfileCategory PlayerProfileEnum { get; set; }
        #if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
        #endif 
        [field: SerializeField] public float MasteryPreference { get; set; }
        #if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
        #endif 
        [field: SerializeField] public float ImmersionPreference { get; set; }
        #if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
        #endif 
        [field: SerializeField] public float CreativityPreference { get; set; }
        #if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
        #endif 
        [field: SerializeField] public float AchievementPreference { get; set; }
        
        public PlayerProfile()
        {
            PlayerProfileEnum = PlayerProfileCategory.Null;
            AchievementPreference = -1;
            CreativityPreference = -1;
            ImmersionPreference = -1;
            MasteryPreference = -1;
        }

        public void SetProfileFromFavoriteQuest(string favoriteQuest)
        {
            PlayerProfileEnum = (PlayerProfileCategory) Enum.Parse(typeof(PlayerProfileCategory), favoriteQuest);
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("M:" + MasteryPreference);
            stringBuilder.Append("T:" + ImmersionPreference);
            stringBuilder.Append("G:" + AchievementPreference);
            stringBuilder.Append("E:" + CreativityPreference);
            return stringBuilder.ToString();
        }
    }
}