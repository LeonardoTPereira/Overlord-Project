using System.IO;
using System.Text;
using UnityEngine;

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
        //TODO: e isso aqui? -lu
        // public const string KILL_QUEST = "Kill";
        // public const string GET_QUEST = "Get";
        // public const string EXPLORE_QUEST = "Explore";
        // public const string TALK_QUEST = "Talk";
        public const string KILL_QUEST = "Mastery";
        public const string GET_QUEST = "Achievement";
        public const string EXPLORE_QUEST = "Creativity";
        public const string TALK_QUEST = "Immersion";
    }
}