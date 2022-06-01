using System;
using System.Linq;
using Game.NarrativeGenerator.Quests;
using UnityEngine;
using Util;
using static Util.Enums;

namespace Game.NarrativeGenerator
{
    [Serializable]
    public class QuestDungeonsParameters
    {
        [SerializeField]
        private int _size = 0;
        public int Size
        {
            get => _size;
            set => _size = value;
        }

        [field: SerializeField]
        public int NKeys { get; set; } = 0;
        [field: SerializeField]
        public int LinearityEnum { get; set; }

        public QuestDungeonsParameters()
        {
            Size = 0;
            NKeys = 0;
            LinearityEnum = 0; 
        }

        public override string ToString()
        {
            return "Size=" + Size + "_Keys=" + NKeys + "_lin=" + GetLinearity();
        }

        public float GetLinearity()
        {
            return DungeonLinearityConverter.ToFloat((DungeonLinearity)LinearityEnum);
        }

        public void CalculateDungeonParametersFromQuests(QuestLine quests, float explorationPreference)
        {
            Size = GetSizeFromEnum(quests.questLines.Count, explorationPreference);
            var explorationQuests = 0;
            var objectiveQuests = 0;
            foreach (var quest in quests.questLines.SelectMany(questLine => questLine.Quests))
            {
                if (quest.IsExplorationQuest())
                {
                    explorationQuests++;
                    objectiveQuests++;
                }
                else if (quest.IsTalkQuest())
                {
                    explorationQuests++;
                }
            }
            LinearityEnum = GetLinearityFromEnum(explorationQuests);
            NKeys = GetNKeys(objectiveQuests);
        }

        private int GetSizeFromEnum(int totalQuests, float explorationPreference)
        {
            var explorationMultiplier = explorationPreference / 7.0f + 1;
            int dungeonSizeCoefficient = (int)(totalQuests * explorationMultiplier);
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

        private int GetLinearityFromEnum(int linearityMetric)
        {
            var linearityCoefficient = linearityMetric/(float)Size;
            if (linearityCoefficient < 0.2f)
            {
                return (int)DungeonLinearity.VeryLinear;
            }
            if (linearityCoefficient < 0.4f)
            {
                return (int)DungeonLinearity.Linear;
            }
            if (linearityCoefficient < 0.6f)
            {
                return (int)DungeonLinearity.Medium;
            }
            if (linearityCoefficient < 0.8f)
            {
                return (int)DungeonLinearity.Branched;
            }
            return (int) DungeonLinearity.VeryBranched;
        }

        private int GetNKeys(int objectiveQuests)
        {
            var achievementCoefficient = (objectiveQuests + 1) / (float) Size;
            if (achievementCoefficient < 0.2f)
            {
                return (int)DungeonKeys.AFewKeys;
            }
            if (achievementCoefficient < 0.4f)
            {
                return (int)DungeonKeys.SomeKeys;
            }
            if (achievementCoefficient < 0.6f || Size < (int) DungeonSize.Small)
            {
                return (int)DungeonKeys.SeveralKeys;
            }
            if (achievementCoefficient < 0.8f || Size < (int) DungeonSize.Medium)
            {
                return (int)DungeonKeys.ManyKeys;
            }
            return (int)DungeonKeys.LotsOfKeys;
        }
    }
}
