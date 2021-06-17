using UnityEngine;

public class drop_ter
{

    public void choose(Manager m)
    {
        Quest quest = new Quest();
        quest.Tipo = 5;

        quest.N1 = Random.Range(0, 11);
        quest.N2 = Random.Range(0, 11 - quest.N1);
        quest.N3 = 10 - quest.N1 - quest.N2;

        quest.c1 = -1;
        quest.c2 = -1;
        quest.parent = -1;

        m.Graph.Add(quest);

        //m.chain.Add(6);
    }
}
