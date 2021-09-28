using System.Collections.Generic;
using UnityEngine;

public class Kill : NonTerminalQuest
{
    public Kill(int lim, Dictionary<string, int> questWeightsbyType) : base(lim, questWeightsbyType)
    {
        maxQuestChance = 2.5f;
    }

    protected override void DefineNextQuest(Manager m)
    {
        kill_ter k = new kill_ter();

        if (r <= 2.3)
        {
            k.choose(m);
            Talk t = new Talk(lim, questWeightsbyType);
            t.Option(m);
            Option(m);
        }
        if (r > 2.3) 
            k.choose(m);
    }
}