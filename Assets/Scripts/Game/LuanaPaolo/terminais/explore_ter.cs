using UnityEngine;

public class explore_ter
{

    public void choose(Manager m)
    {
        Quest quest = new Quest();
        quest.Tipo = 4;
        quest.N1 = Random.Range(100, 200);
        quest.c1 = -1;
        quest.c2 = -1;
        quest.parent = -1;

        m.Graph.Add(quest);

        //m.chain.Add(5);
    }
}
