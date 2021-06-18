using UnityEngine;

public class get_ter
{

    public void choose(Manager m)
    {
        Quest quest = new Quest();
        quest.Tipo = 3;
        quest.N1 = Random.Range(0, 10);
        quest.c1 = -1;
        quest.c2 = -1;
        quest.parent = -1;

        m.Graph.Add(quest);

        //m.chain.Add(4);
    }
}
