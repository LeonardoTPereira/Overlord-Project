using System.Collections.Generic;
using UnityEngine;

public class Get : NonTerminalQuest
{
    public Get(int lim, Dictionary<string, int> questWeightsbyType) : base(lim, questWeightsbyType)
    {
        maxQuestChance = 2.8f;
    }

    protected override void DefineNextQuest(Manager m)
    {
        if (r > 2.85)
        {
            get_ter g = new get_ter();
            g.choose(m);
            Talk t = new Talk(lim, questWeightsbyType);
            t.Option(m);
            Option(m);
        }
        if (r > 2.7 && r <= 2.85)
        {
            get_ter g = new get_ter();
            g.choose(m);
        }
        if (r > 2.55 && r <= 2.7)
        {
            drop_ter d = new drop_ter();
            d.choose(m);
            Talk t = new Talk(lim, questWeightsbyType);
            t.Option(m);
            Option(m);
        }
        if (r > 2.4 && r <= 2.55)
        {
            drop_ter d = new drop_ter();
            d.choose(m);
        }
        if (r > 2.25 && r <= 2.4)
        {
            chest_ter c = new chest_ter();
            c.choose(m);
            Talk t = new Talk(lim, questWeightsbyType);
            t.Option(m);
            Option(m);
        }
        if (r <= 2.25)
        {
            chest_ter c = new chest_ter();
            c.choose(m);
        }
    }
}