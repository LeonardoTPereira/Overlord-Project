using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator.Quests.nao_terminais
{
    public abstract class NonTerminalQuest
    {
        protected float r;
        protected int lim;
        protected float maxQuestChance;
        protected readonly Dictionary<string, int> QuestWeightsByType;
        private const int QuestLimit = 2;

        protected NonTerminalQuest(int lim, Dictionary<string, int> questWeightsByType)
        {
            this.QuestWeightsByType = questWeightsByType;
            this.lim = lim;
        }

        protected void DrawQuestType()
        {
            r = ((QuestWeightsByType[Constants.TALK_QUEST] +
                  QuestWeightsByType[Constants.GET_QUEST] * 2 +
                  QuestWeightsByType[Constants.KILL_QUEST] * 3 +
                  QuestWeightsByType[Constants.EXPLORE_QUEST] * 4) / 16.0f) *
                Random.Range(0f, 3f);
            if (lim == QuestLimit)
            {
                r = maxQuestChance;
            }
            lim++;
        }
    }
}