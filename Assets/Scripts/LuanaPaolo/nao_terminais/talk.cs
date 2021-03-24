using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talk{
    float r;
    
    public void option(Manager m, int lim, int[] pesos){
        r = ((pesos[0] + pesos[1]*2 + pesos[2]*3 + pesos[3]*4)/16) * Random.Range(0f, 3f);
        if(lim == 3) r = 2.5f;

        talk_ter t = new talk_ter();

        lim++;

        if(r > 2.5){
            t.choose(m);
            this.option(m, lim, pesos);
        }
        if(r <= 2.5) t.choose(m);
    }
}