﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class get_ter{
    
    public void choose(Manager m){
        Quest quest = new Quest();
        quest.tipo = 3;
        quest.n1 = Random.Range(0, 10);
        quest.c1 = -1;
        quest.c2 = -1;
        quest.parent = -1;

        m.graph.Add(quest);

        //m.chain.Add(4);
    }
}