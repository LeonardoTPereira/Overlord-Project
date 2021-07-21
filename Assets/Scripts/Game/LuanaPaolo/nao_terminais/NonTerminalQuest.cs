using System.Collections.Generic;
using UnityEngine;
using static Util;

public abstract class NonTerminalQuest
{
    protected float r;
    protected int lim;
    protected float maxQuestChance;
    protected Dictionary<string, int> questWeightsbyType;
    private static readonly int QUEST_LIMIT = 3;

    protected NonTerminalQuest(int lim, Dictionary<string, int> questWeightsbyType)
    {
        this.questWeightsbyType = questWeightsbyType;
        this.lim = lim;
    }

    public void Option(Manager m)
    {
        DrawQuestType();
        DefineNextQuest(m);
    }

    protected abstract void DefineNextQuest(Manager m);

    private void DrawQuestType()
    {
        r = ((questWeightsbyType[TALK_QUEST] +
            questWeightsbyType[GET_QUEST] * 2 +
            questWeightsbyType[KILL_QUEST] * 3 +
            questWeightsbyType[EXPLORE_QUEST] * 4) / 16) *
        Random.Range(0f, 3f);
        if (lim == QUEST_LIMIT)
        {
            r = maxQuestChance;
        }
        lim++;
    }
}