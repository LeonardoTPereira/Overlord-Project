﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using static Util.Enums;

namespace Game.NarrativeGenerator
{
    public static class DungeonLinearityConverter
    {
        private static readonly ReadOnlyDictionary<DungeonLinearity, float> DungeonLinearityEnumToFloat
            = new (new Dictionary<DungeonLinearity, float>
            {
                { DungeonLinearity.VeryLinear, 1.0f},
                { DungeonLinearity.Linear, 1.2f},
                { DungeonLinearity.Medium, 1.4f},
                { DungeonLinearity.Branched, 1.6f},
                { DungeonLinearity.VeryBranched, 1.8f},
            });

        public static float ToFloat(this DungeonLinearity dungeonLinearity)
        {
            return DungeonLinearityEnumToFloat[dungeonLinearity];
        }

    }
    
    [CreateAssetMenu(menuName = "NarrativeComponents/Dungeons")]
    public class ParametersDungeon : ScriptableObject
    {
        public static int GetSizeFromEnum(float dungeonSizeCoefficient)
        {

            var sizeFromEnum = dungeonSizeCoefficient - (int)DungeonSize.Large;
            if (sizeFromEnum > 0)
                return (int)DungeonSize.VeryLarge;
            sizeFromEnum = dungeonSizeCoefficient - (int)DungeonSize.Medium;
            if (sizeFromEnum > 0)
                return (int)DungeonSize.Large;
            sizeFromEnum = dungeonSizeCoefficient - (int)DungeonSize.Small;
            if (sizeFromEnum > 0)
                return (int)DungeonSize.Medium;
            sizeFromEnum = dungeonSizeCoefficient - (int)DungeonSize.VerySmall;
            if (sizeFromEnum > 0)
                return (int)DungeonSize.Small;
            return (int) DungeonSize.VerySmall;
        }

        public static int GetLinearityFromEnum(float linearityMetric)
        {
            if (linearityMetric < 0.2f)
            {
                return (int)DungeonLinearity.VeryLinear;
            }
            if (linearityMetric < 0.4f)
            {
                return (int)DungeonLinearity.Linear;
            }
            if (linearityMetric < 0.6f)
            {
                return (int)DungeonLinearity.Medium;
            }
            if (linearityMetric < 0.8f)
            {
                return (int)DungeonLinearity.Branched;
            }
            return (int) DungeonLinearity.VeryBranched;
        }

        public static int GetNKeys(int objectiveQuests, int dungeonSize)
        {
            if (objectiveQuests < 0.2f)
            {
                return (int)DungeonKeys.AFewKeys;
            }
            if (objectiveQuests < 0.4f)
            {
                return (int)DungeonKeys.SomeKeys;
            }
            if (objectiveQuests < 0.6f || dungeonSize < (int) DungeonSize.Small)
            {
                return (int)DungeonKeys.SeveralKeys;
            }
            if (objectiveQuests < 0.8f || dungeonSize < (int) DungeonSize.Medium)
            {
                return (int)DungeonKeys.ManyKeys;
            }
            return (int)DungeonKeys.LotsOfKeys;
        }
    }    
}
