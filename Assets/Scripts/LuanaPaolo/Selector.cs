using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//classe que seleciona a linha de missões de acordo com os pesos do perfil do jogador
public class Selector{

    private int r;
    public int[] pesos = new int[4]; //vetor de pesos

    public void select(Manager m){
        r = Random.Range(0, 4);

        if(r == 0){
            talk t = new talk();
            t.option(m, 0);
        }
        if(r == 1){
            get g = new get();
            g.option(m, 0);
        }
        if(r == 2){
            kill k = new kill();
            k.option(m, 0);
        }
        if(r == 3){
            explore e = new explore();
            e.option(m, 0);
        }
    }
}