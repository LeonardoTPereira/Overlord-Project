using System.Collections.Generic;
using UnityEngine;

public class Explore : NonTerminalQuest
{
    public Explore(int lim, Dictionary<string, int> questWeightsbyType) : base(lim, questWeightsbyType)
    {
        maxQuestChance = 2.5f;
    }

    protected override void DefineNextQuest(Manager m)
    {

        if (r > 2.85)
        {
            explore_ter e = new explore_ter();
            e.choose(m);
            Talk t = new Talk(lim, questWeightsbyType);
            t.Option(m);
            Option(m);
        }
        if (r > 2.6 && r <= 2.85)
        {
            explore_ter e = new explore_ter();
            e.choose(m);
        }
        if (r > 2.35 && r <= 2.6)
        {
            secret_ter s = new secret_ter();
            s.choose(m);
            Talk t = new Talk(lim, questWeightsbyType);
            t.Option(m);
            Option(m);
        }
        if (r <= 2.35)
        {
            secret_ter s = new secret_ter();
            s.choose(m);
        }
    }
}