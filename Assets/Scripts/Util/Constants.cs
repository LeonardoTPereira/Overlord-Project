using System.IO;
using UnityEngine;
using System;

namespace Util
{
    public static class Constants
    {
        public static readonly char SEPARATOR_CHARACTER = Path.DirectorySeparatorChar;
        //Array com cada cor de cada ID, para diferenciar chaves e fechaduras
        public static Color[] colorId = new Color[] { Color.yellow, Color.blue, Color.green, Color.red, Color.gray, Color.white, Color.cyan, Color.black };

        public static float LogNormalization(float value, float minValue, float maxValue, float minNormalized, float maxNormalized)
        {
            return (Mathf.Log(value - minValue) / Mathf.Log(maxValue - minValue)) * (maxNormalized - minNormalized);
        }

        public const int nSpawnPointsVer = 4;
        public const int nSpawnPointsHor = 4;
        public const int distFromBorder = 1;

        public const int defaultRoomSizeX = 22;
        public const int defaultRoomSizeY = 19;

        /// Define the room codes for printing purposes.
        public static class RoomTypeString
        {
            public static readonly string CORRIDOR = "C";
            public static readonly string LOCK = "L";
            public static readonly string KEY = "K";
            public static readonly string BOSS = "B";
            public static readonly string TREASURE = "T";
            public static readonly string START = "S";
            public static readonly string NORMAL = "N";
        }

        #region Terminal quest symbols
        public const string KILL_TERMINAL = "kill";
        public const string TALK_TERMINAL = "talk";
        public const string EXPLORE_TERMINAL = "explore";
        public const string EMPTY_TERMINAL = "empty";
        public const string GET_TERMINAL = "get";
        public const string DROP_TERMINAL = "drop";
        public const string ITEM_TERMINAL = "item";
        public const string SECRET_TERMINAL = "secret";
        #endregion

        #region Creativity quests
        public const string LISTEN = "listen";
        public const string READ = "read";
        public const string REPORT = "report";
        public const string GIVE = "give";
        public const string EXCHANGE = "exchange";
        #endregion
        #region Mastery quests
        public const string KILL = "kill";
        public const string CAPTURE = "capture";
        public const string DAMAGE = "damage";
        public const string DEFEND = "defend";
        #endregion
        #region Achievment quests
        public const string GATHER = "gather";
        public const string EXPERIMENT = "experiment";
        public const string TAKE = "take";
        public const string USE = "use";
        public const string REPAIR = "repair";
        #endregion
        #region Immersion quests
        public const string STEALTH = "stealth";
        public const string ESCORT = "escort";
        public const string GOTO = "goto";
        public const string SPY = "spy";
        #endregion

        #region Quest Weights
        public static Func<int,int> OneOptionQuestLineWeight = x => 
        {
            // if ( x < 4 )
            //     return 100;
            // return 0;
            return (int)(100 - (2*Mathf.Pow(x,2)));
        };
        public static Func<int,int> OneOptionQuestEmptyWeight = x => 
        {
            // if ( x > 4 )
            //     return 100;
            // return 0;
            return (int) (2*Mathf.Pow(x,2));
        };

        public static Func<int,int> ThreeOptionQuestLineWeight = x => 
        {
            return OneOptionQuestLineWeight(x)/3;
        };
        public static Func<int,int> ThreeOptionQuestEmptyWeight = x => 
        {
            if ( x > 4 )
                return 100;
            return 0;
            // return (int)Mathf.Clamp( ( 1 -( 1/(x*0.25f))), 0, 100);  
        };

        public static Func<int,int> FourOptionQuestLineWeight = x =>
        {
            return OneOptionQuestLineWeight(x)/4;
        };

        public static Func<int,int> FiveOptionQuestLineWeight = x =>
        {
            return OneOptionQuestLineWeight(x)/5;
        };
        #endregion

        public const string KILL_QUEST = "Mastery";
        public const string GET_QUEST = "Achievement";
        public const string EXPLORE_QUEST = "Creativity";
        public const string TALK_QUEST = "Immersion";
    }
}