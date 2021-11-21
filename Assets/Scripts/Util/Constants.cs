using System.IO;
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



        public const string KILL_QUEST = "Kill";
        public const string GET_QUEST = "Get";
        public const string EXPLORE_QUEST = "Explore";
        public const string TALK_QUEST = "Talk";

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
    }
}