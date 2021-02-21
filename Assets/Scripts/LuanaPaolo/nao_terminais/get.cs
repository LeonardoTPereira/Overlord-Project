using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class get{
    int r;

    public void option(Manager m, int lim){
        r = Random.Range(0, 6);
        if(lim == 3) r = 1;

        lim++;

        if(r == 0){
            get_ter g = new get_ter();
            g.choose(m);
            talk t = new talk();
            t.option(m, lim);
            this.option(m, lim);
        }
        if(r == 1){
            get_ter g = new get_ter();
            g.choose(m);
        }
        if(r == 2){
            drop_ter d = new drop_ter();
            d.choose(m);
            talk t = new talk();
            t.option(m, lim);
            this.option(m, lim);
        }
        if(r == 3){
            drop_ter d = new drop_ter();
            d.choose(m);
        }
        if(r == 4){
            chest_ter c = new chest_ter();
            c.choose(m);
            talk t = new talk();
            t.option(m, lim);
            this.option(m, lim);
        }
        if(r == 5){
            chest_ter c = new chest_ter();
            c.choose(m);
        }
    }
}