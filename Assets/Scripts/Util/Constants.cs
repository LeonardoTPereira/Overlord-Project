using System.IO;
using UnityEngine;
using System;

namespace Util
{
    public static class Constants
    {
        public static readonly char SeparatorCharacter = Path.DirectorySeparatorChar;

        public static readonly Color[] ColorId = new Color[] { Color.yellow, Color.blue, Color.green, Color.red, Color.gray, Color.white, Color.cyan, Color.black };
        public static readonly Color VisitedColor = new Color(0.5433761f, 0.2772784f, 0.6320754f, 1.0f);
        public static readonly Color MarkedColor = new Color(0.8433761f, 0.2772784f, 0.2320754f, 1.0f);

        public static float LogNormalization(float value, float minValue, float maxValue, float minNormalized, float maxNormalized)
        {
            return (Mathf.Log(value - minValue) / Mathf.Log(maxValue - minValue)) * (maxNormalized - minNormalized);
        }

        public const int NSpawnPointsVer = 6;
        public const int NSpawnPointsHor = 6;
        public const int DistFromBorder = 2;

        /// Define the room codes for printing purposes.
        public static class RoomTypeString
        {
            public const string Corridor = "Corridor";
            public const string LockedCorridor = "LockedCorridor";
            public const string LockedRoom = "LockedRoom";
            public const string Key = "Key";
            public const string Boss = "Boss";
            public const string Leaf = "Leaf";
            public const string Start = "Start";
            public const string Normal = "Normal";
        }

        #region Terminal quest symbols
        //Mastery
        public const string KillQuest = "kill";
        public const string DamageQuest = "damage";
        //Immersion
        public const string ListenQuest = "listen";
        public const string ReadQuest = "read";
        public const string ReportQuest = "report";
        public const string GiveQuest = "give";
        //Creativity
        public const string ExploreQuest = "explore";
        public const string GotoQuest = "goto";
        //Achievement
        public const string GatherQuest = "gather";
        public const string ExchangeQuest = "exchange";
        //Not implemented
        public const string ItemQuest = "item";
        public const string SecretQuest = "secret";
        //Empty
        public const string EmptyQuest = "empty";
        public const string StartChain ="start";
        #endregion

        #region Quest Weights
        public static readonly Func<int,float> OneOptionQuestLineWeight = x => (100 - Mathf.Pow((0.5f*x),2));

        public static readonly Func<int,float> TwoOptionQuestLineWeight = x => OneOptionQuestLineWeight(x)/2;

        public static readonly Func<int,float> ThreeOptionQuestLineWeight = x => OneOptionQuestLineWeight(x)/3;
        
        public static readonly Func<int,float> FourOptionQuestLineWeight = x => OneOptionQuestLineWeight(x)/4;
        
        public static readonly Func<int,float> OneOptionQuestEmptyWeight = x => Mathf.Pow((0.5f*x),2);

        public static readonly Func<int,float> TwoOptionQuestEmptyWeight = x => OneOptionQuestEmptyWeight(x);

        #endregion

        public const string MasteryQuest = "Mastery";
        public const string AchievementQuest = "Achievement";
        public const string CreativityQuest = "Creativity";
        public const string ImmersionQuest = "Immersion";
    }
}