using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explore{
    int r;

    public void option(Manager m, int lim){
        r = Random.Range(0, 4);
        if(lim == 3) r = 1;

        lim++;

        if(r == 0){
            explore_ter e = new explore_ter();
            e.choose(m);
            talk t = new talk();
            t.option(m, lim);
            this.option(m, lim);
        }
        if(r == 1){
            explore_ter e = new explore_ter();
            e.choose(m);
        }
        if(r == 2){
            secret_ter s = new secret_ter();
            s.choose(m);
            talk t = new talk();
            t.option(m, lim);
            this.option(m, lim);
        }
        if(r == 3){
            secret_ter s = new secret_ter();
            s.choose(m);
        }
    }
}