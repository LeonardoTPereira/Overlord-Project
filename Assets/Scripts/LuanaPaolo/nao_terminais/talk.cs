using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talk{
    int r;
    
    public void option(Manager m, int lim){
        r = Random.Range(0, 2);
        if(lim == 3) r = 1;

        talk_ter t = new talk_ter();

        lim++;

        if(r == 0){
            t.choose(m);
            this.option(m, lim);
        }
        if(r == 1) t.choose(m);
    }
}