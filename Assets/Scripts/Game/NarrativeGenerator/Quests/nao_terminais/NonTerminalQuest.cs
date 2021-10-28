using System.Collections.Generic;
using Game.NarrativeGenerator;
using UnityEngine;
using static Util;

public class NonTerminalQuest : Symbol
{
    public Dictionary<SymbolType,float> nextSymbolChance {get; set;}
    // Symbol symbol = new NonTerminalQuest();
    protected float r;
    protected int lim;
    protected float maxQuestChance;
    protected Dictionary<string, int> questWeightsbyType;
    private static readonly int QUEST_LIMIT = 2;

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

    protected virtual void DefineNextQuest(Manager m){}

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
    
    public virtual void SetDictionary( Dictionary<SymbolType, float> _nextSymbolChances  )
    {
        nextSymbolChance = _nextSymbolChances;
    }

    public virtual void SetNextSymbol(MarkovChain chain)
    {
        float chance = (float) Random.Range( 0, 100 ) / 100 ;
        
    }
}