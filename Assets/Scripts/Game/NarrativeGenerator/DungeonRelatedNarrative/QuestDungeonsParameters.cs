using System;
using System.Collections.Generic;
using System.Linq;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using UnityEngine;
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
            return "Size=" + Size + "_Keys=" + NKeys + "_lin=" + LinearityEnum;
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
            var sizeCoefficient = CalculateSize(totalQuests, explorationPreference);
            Size = ParametersDungeon.GetSizeFromEnum(sizeCoefficient);
            LinearityEnum = ParametersDungeon.GetLinearityFromEnum(linearityCoefficient);
            NKeys = ParametersDungeon.GetNKeys(_creativityQuests/totalQuests, Size);
            #if UNITY_EDITOR
                Debug.Log("Dungeon Parameters: "+ ToString() 
                                                + $"\nCoefficients: Total Quests={totalQuests}, Linearity={linearityCoefficient}, Size={sizeCoefficient}" +
                                                $", Exploration: {explorationPreference}");
            #endif
        }

        private int CalculateTotal()
        {
            return _creativityQuests + _masteryQuests + _immersionQuests + _achievementQuests;
        }

        private int CalculateSize(int totalQuests, float explorationPreference)
        {
            var questsThatNeedSpace = totalQuests - _immersionQuests;
            return (int)(questsThatNeedSpace/2f * (1f+explorationPreference/2f));
        }

        private float CalculateLinearity(int totalQuests)
        {
            return (_creativityQuests + _achievementQuests)/(float)totalQuests;
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
    }
}
