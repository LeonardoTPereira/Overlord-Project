using System;
using System.Collections.Generic;
using System.Linq;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using UnityEngine;
using Util;
using static Util.Enums;

namespace Game.NarrativeGenerator
{
    [Serializable]
    public class QuestDungeonsParameters
    {
        [field: SerializeField] public int Size { get; set; }
        private int _creativityQuests;
        private int _achievementQuests;
        private int _immersionQuests;
        private int _masteryQuests;

        [field: SerializeField]
        public int NKeys { get; set; }
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
            return ((DungeonLinearity)LinearityEnum).ToFloat();
        }

        public void CalculateDungeonParametersFromQuests(IEnumerable<QuestLine> questLines, float explorationPreference)
        {
            foreach (var quest in questLines.SelectMany(questLine => questLine.Quests))
            {
                AddQuestTypeToCounter(quest);
            }

            var totalQuests = CalculateTotal();
            var linearityCoefficient = CalculateLinearity(totalQuests);
            var sizeCoefficient = CalculateSize(totalQuests);
            Size = GetSizeFromEnum(sizeCoefficient, explorationPreference);
            LinearityEnum = GetLinearityFromEnum(linearityCoefficient);
            NKeys = GetNKeys(_creativityQuests);
        }

        private int CalculateTotal()
        {
            return _creativityQuests + _masteryQuests + _immersionQuests + _achievementQuests;
        }

        private int CalculateSize(int totalQuests)
        {
            return _creativityQuests/totalQuests;
        }

        private int CalculateLinearity(int totalQuests)
        {
            return _creativityQuests/totalQuests;
        }

        private void AddQuestTypeToCounter(QuestSo questSo)
        {
            switch (questSo)
            {
                case AchievementQuestSo:
                    _achievementQuests++;
                    break;
                case CreativityQuestSo:
                    _creativityQuests++;
                    break;
                case ImmersionQuestSo:
                    _immersionQuests++;
                    break;
                case MasteryQuestSo:
                    _masteryQuests++;
                    break;
                default:
                    Debug.LogError($"No quest type for this quest {questSo.GetType()} " +
                                   "was found to create dialogue");
                    break;
            }
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
            var linearityCoefficient = linearityMetric;
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
            var achievementCoefficient = objectiveQuests;
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
