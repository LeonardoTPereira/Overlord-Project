using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kill{
    int r;

    public void option(Manager m, int lim){
        r = Random.Range(0, 2);
        if(lim == 3) r = 1;

        lim++;

        kill_ter k = new kill_ter();

        if(r == 0){
            k.choose(m);
            talk t = new talk();
            t.option(m, lim);
            this.option(m, lim);
        }
        if(r == 1) k.choose(m);
    }
}