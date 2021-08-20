using System.Collections.Generic;
using UnityEngine;

public class Talk : NonTerminalQuest
{

    public Talk(int lim, Dictionary<string, int> questWeightsbyType) : base(lim, questWeightsbyType)
    {
        maxQuestChance = 2.4f;
    }
    
    protected override void DefineNextQuest(Manager m)
    {
        talk_ter t = new talk_ter();
        if (r > 2.7)
        {
            t.choose(m);
            Option(m);
        }
        if (r <= 2.7) t.choose(m);
    }
}