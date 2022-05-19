using UnityEngine;
using Util;
using static Util.Enums;

namespace Game.NarrativeGenerator
{
    [CreateAssetMenu(menuName = "NarrativeComponents/Dungeons")]
    public class ParametersDungeon : ScriptableObject
    {
        public int Size { get; } = 0;
        public int NKeys { get; } = 0;
        public int NEnemies { get; } = -1;
        public float Linearity { get; set; }
        private int linearityEnum;

        public int LinearityMetric { get; set; } = 0;

        public int LinearityEnum
        {
            get => linearityEnum;
            set
            {
                linearityEnum = value;
                Linearity = GetLinearity();
            }
        }

        public override string ToString()
        {
            return "Size=" + Size + "_Keys=" + NKeys + "_lin=" + GetLinearity() + "_NEnemies=" + NEnemies;
        }

        public float GetLinearity()
        {
            return DungeonLinearityConverter.ToFloat((DungeonLinearity)LinearityEnum);
        }
    }    
}
