using UnityEngine;

public class drop_ter
{

    public void choose(Manager m)
    {
        Quest quest = new Quest();
        quest.tipo = 5;

        quest.n1 = Random.Range(0, 11);
        quest.n2 = Random.Range(0, 11 - quest.n1);
        quest.n3 = 10 - quest.n1 - quest.n2;

        quest.c1 = -1;
        quest.c2 = -1;
        quest.parent = -1;

        m.graph.Add(quest);

        //m.chain.Add(6);
    }
}
