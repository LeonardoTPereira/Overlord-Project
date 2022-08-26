using System.IO;
using UnityEngine;
using System;

namespace Util
{
    public static class Constants
    {
        public static readonly char SeparatorCharacter = Path.DirectorySeparatorChar;

        public static readonly Color[] ColorId = new Color[] { Color.yellow, Color.blue, Color.green, Color.red, Color.gray, Color.white, Color.cyan, Color.black };

        public static float LogNormalization(float value, float minValue, float maxValue, float minNormalized, float maxNormalized)
        {
            return (Mathf.Log(value - minValue) / Mathf.Log(maxValue - minValue)) * (maxNormalized - minNormalized);
        }

        public const int NSpawnPointsVer = 6;
        public const int NSpawnPointsHor = 6;
        public const int DistFromBorder = 1;

        public const int DefaultRoomSizeX = 22;
        public const int DefaultRoomSizeY = 19;

        /// Define the room codes for printing purposes.
        public static class RoomTypeString
        {
            public const string Corridor = "C";
            public const string Lock = "L";
            public const string Key = "K";
            public const string Boss = "B";
            public const string Treasure = "T";
            public const string Start = "S";
            public const string Normal = "N";
        }

        #region Terminal quest symbols
        //Mastery
        public const string KILL_QUEST = "kill";
        public const string DAMAGE_QUEST = "damage";
        //Immersion
        public const string LISTEN_QUEST = "listen";
        public const string READ_QUEST = "read";
        public const string REPORT_QUEST = "report";
        public const string GIVE_QUEST = "give";
        //Creativity
        public const string EXPLORE_QUEST = "explore";
        public const string GOTO_QUEST = "goto";
        //Achievement
        public const string GATHER_QUEST = "gather";
        public const string EXCHANGE_QUEST = "exchange";
        //Not implemented
        public const string ITEM_QUEST = "item";
        public const string SECRET_QUEST = "secret";
        //Empty
        public const string EMPTY_QUEST = "empty";
        public const string START ="start";
        #endregion

        #region Quest Weights
        public static Func<int,int> OneOptionQuestLineWeight = x => 
        {
            return (int)(100 - Mathf.Pow(x,2));
        };
        public static Func<int,int> OneOptionQuestEmptyWeight = x => 
        {
            return (int) Mathf.Pow(x,2);
        };

        public static Func<int,int> TwoOptionQuestLineWeight = x => 
        {
            return OneOptionQuestLineWeight(x)/2;
        };
        public static Func<int,int> TwoOptionQuestEmptyWeight = x => 
        {
            return OneOptionQuestEmptyWeight(x);
        };

        public static Func<int,int> FourOptionQuestLineWeight = x => 
        {
            return OneOptionQuestLineWeight(x)/4;
        };
        #endregion

        public const string MasteryQuest = "Mastery";
        public const string AchievementQuest = "Achievement";
        public const string CreativityQuest = "Creativity";
        public const string ImmersionQuest = "Immersion";
    }
}