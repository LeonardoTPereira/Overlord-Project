using UnityEngine;

public class talk_ter
{

    public void choose(Manager m)
    {
        Quest quest = new Quest();
        quest.Tipo = 6;
        quest.N1 = Random.Range(0, 3);
        quest.c1 = -1;
        quest.c2 = -1;
        quest.parent = -1;

        m.Graph.Add(quest);

        //m.chain.Add(0);
    }
}