using UnityEngine;

public class talk_ter
{

    public void choose(Manager m)
    {
        Quest quest = new Quest();
        quest.tipo = 6;
        quest.n1 = Random.Range(0, 3);
        quest.c1 = -1;
        quest.c2 = -1;
        quest.parent = -1;

        m.graph.Add(quest);

        //m.chain.Add(0);
    }
}