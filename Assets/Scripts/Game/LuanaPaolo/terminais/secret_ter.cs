using UnityEngine;

public class secret_ter
{

    public void choose(Manager m)
    {
        Quest quest = new Quest();
        quest.Tipo = 1;
        quest.N1 = Random.Range(0, 100);
        quest.c1 = -1;
        quest.c2 = -1;
        quest.parent = -1;

        m.Graph.Add(quest);

        //m.chain.Add(2);
    }
}
