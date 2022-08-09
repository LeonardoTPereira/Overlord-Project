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

        public const int NSpawnPointsVer = 4;
        public const int NSpawnPointsHor = 4;
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
        //TODO: Get some actual decent functions
        public static Func<int,int> OneOptionQuestLineWeight = x => 
        {
            if ( x < 100 )
                return 100;
            return 0;
            // return (int)(100 - Mathf.Pow(x,2));
        };
        public static Func<int,int> OneOptionQuestEmptyWeight = x => 
        {
            if ( x > 100 )
                return 100;
            return 0;
            // return (int) Mathf.Pow(x,2);
        };

        public static Func<int,int> TwoOptionQuestLineWeight = x => 
        {
            if ( x < 100 )
                return 50;
            return 0;
            // return (int)(100 - Mathf.Pow(x,2));
        };
        public static Func<int,int> TwoOptionQuestEmptyWeight = x => 
        {
            if ( x > 100 )
                return 100;
            return 0;
            // return (int) Mathf.Pow(x,2);
        };

        public static Func<int,int> FourOptionQuestLineWeight = x => 
        {
            if ( x < 100 )
                return (100/4);
            return 0;
            // return (int)(100 - Mathf.Pow(x,2));
        };

        public static Func<int,int> ThreeOptionQuestLineWeight = x => 
        {
            if ( x < 100 )
                return 34;
            return 0;
            // return (int)Mathf.Clamp( 0.3f*(1/(x*0.25f)), 0, 30);
        };
        public static Func<int,int> ThreeOptionQuestEmptyWeight = x => 
        {
            if ( x > 100 )
                return 100;
            return 0;
            // return (int)Mathf.Clamp( ( 1 -( 1/(x*0.25f))), 0, 100);  
        };
        #endregion

        public const string MasteryQuest = "Mastery";
        public const string AchievementQuest = "Achievement";
        public const string CreativityQuest = "Creativity";
        public const string ImmersionQuest = "Immersion";
    }
}