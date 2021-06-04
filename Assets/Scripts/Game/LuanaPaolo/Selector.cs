using System.Collections.Generic;
using UnityEngine;

//classe que seleciona a linha de missões de acordo com os pesos do perfil do jogador
public class Selector
{
    private float r;
    public int[] pesos = new int[4]; //vetor de pesos

    private int typePlayer;

    /*
    [7][5][1][3]
    [3][7][1][5]
    [1][5][7][3]
    [1][5][3][7]
    */

    public void Select(Manager m, List<int> answers)
    {
        //pesos[0] = 3; //peso talk
        //pesos[1] = 7; //peso get
        //pesos[2] = 1; //peso kill
        //pesos[3] = 5; //peso explore

        weightCalculator(answers);

        int r = ((pesos[0] + pesos[1]*2 + pesos[2]*3 + pesos[3]*4)/16);// * Random.Range(0f, 3f); <<-- equação ainda inutilizada devido a testes específicos

        if (r <= 2.35)
        {
            talk t = new talk();
            t.option(m, 0, pesos);
        }
        if (r > 2.35 && r <= 2.6)
        {
            get g = new get();
            g.option(m, 0, pesos);
        }
        if (r > 2.6 && r <= 2.85)
        {
            kill k = new kill();
            k.option(m, 0, pesos);
        }
        if (r > 2.85)
        {
            explore e = new explore();
            e.option(m, 0, pesos);
        }
    }

    private void weightCalculator(List<int> answers){
        for(int i = 2; i < 12; i++){
            if(i == 2 || i == 3 || i == 4) pesos[2] += answers[i];
            else if(i == 5 || i == 6) pesos[4] += answers[i];
            else if(i == 7 || i == 8) pesos[1] += answers[i];
            else if(i == 9 || i == 10) pesos[0] += answers[i];
            else{
                pesos[4] -= answers[i];
                pesos[1] -= answers[i];
                pesos[0] -= answers[i];
            }
        }

        int max = -100, min = 100, mid1 = -100, mid2 = 100;

        for(int i = 0; i < 4; i++){
            if(pesos[i] > max) max = i;
            else if(pesos[i] < min) min = i;
        }

        pesos[max] = 7;
        pesos[min] = 1;

        for(int i = 0; i < 4; i++){
            if(i != max && i != min){
                if(pesos[i] > mid1) mid1 = 1;
                if(pesos[i] < mid2) mid2 = 1;
            } 
        }

        pesos[mid1] = 5;
        pesos[mid2] = 3;

        if(max == 0) typePlayer = 1;
        else if(max == 1) typePlayer = 3;
        else if(max == 2) typePlayer = 0;
        else typePlayer = 2;
    }
}