using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explore_ter{
    
    public void choose(Manager m){
        Quest quest = new Quest();
        quest.tipo = 4;
        quest.n = Random.Range(100, 200);
        quest.c1 = -1;
        quest.c2 = -1;
        quest.parent = -1;

        m.graph.Add(quest);

        //m.chain.Add(5);
    }
}
