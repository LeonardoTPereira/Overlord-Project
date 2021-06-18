using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Enums;

//classe que seleciona a linha de missões de acordo com os pesos do perfil do jogador
public class Selector
{

    public class QuestWeight
    {
        public string quest;
        public int weight;

        public QuestWeight(string quest, int weight)
        {
            this.quest = quest;
            this.weight = weight;
        }
    }
    public List<QuestWeight> questWeights = new List<QuestWeight>();
    public int[] pesos = new int[4]; //vetor de pesos

    private static readonly int[] WEIGHTS = { 1, 3, 5, 7 };

    private PlayerProfileEnum typePlayer;

    /*
    [7][5][1][3]
    [3][7][1][5]
    [1][5][7][3]
    [1][5][3][7]
    */

    public PlayerProfileEnum Select(Manager m, List<int> answers)
    {
        //pesos[0] = 3; //peso talk
        //pesos[1] = 7; //peso get
        //pesos[2] = 1; //peso kill
        //pesos[3] = 5; //peso explore

        weightCalculator(answers);

        float newMissionChance, newMissionDraw;
        newMissionChance = 0;
        do
        {
            for (int i = 0; i < pesos.Length; i++)
            {
                newMissionDraw = UnityEngine.Random.Range(0, 1.0f);
                if(pesos[i] > newMissionDraw*10)
                {
                    if(i == 0)
                    {
                        talk t = new talk();
                        t.option(m, 0, pesos);
                    }

                    else if(i == 1)
                    {
                        Get g = new Get();
                        g.Option(m, 0, pesos);
                    }

                    else if(i == 2)
                    {
                        kill k = new kill();
                        k.option(m, 0, pesos);
                    }

                    else
                    {
                        Explore e = new Explore();
                        e.Option(m, 0, pesos);
                    }

                }

            }

            newMissionDraw = UnityEngine.Random.Range(0, 1.0f);
            newMissionChance += 0.3f;
        } while (newMissionDraw > newMissionChance);
        /*int r = ((pesos[0] + pesos[1]*2 + pesos[2]*3 + pesos[3]*4)/16);// * Random.Range(0f, 3f); <<-- equação ainda inutilizada devido a testes específicos

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
        }*/
        /*switch(typePlayer)
        {
            case (PlayerProfileEnum.Mastery):
                kill k = new kill();
                k.option(m, 0, pesos);
                break;
            case (PlayerProfileEnum.Achievement):
                get g = new get();
                g.option(m, 0, pesos);
                break;
            case (PlayerProfileEnum.Immersion):
                talk t = new talk();
                t.option(m, 0, pesos);
                break;
            case (PlayerProfileEnum.Creativity):
                explore e = new explore();
                e.option(m, 0, pesos);
                break;

        }*/

        return typePlayer;
    }

    private void weightCalculator(List<int> answers){
        for(int i = 2; i < 12; i++){
            if(i == 2 || i == 3 || i == 4) pesos[2] += answers[i];
            else if(i == 5 || i == 6) pesos[3] += answers[i];
            else if(i == 7 || i == 8) pesos[1] += answers[i];
            else if(i == 9 || i == 10) pesos[0] += answers[i];
            else{
                pesos[3] -= answers[i];
                pesos[1] -= answers[i];
                pesos[0] -= answers[i];
            }
        }
        questWeights.Add(new QuestWeight("talk", pesos[0]));
        questWeights.Add(new QuestWeight("get", pesos[1]));
        questWeights.Add(new QuestWeight("kill", pesos[2]));
        questWeights.Add(new QuestWeight("explore", pesos[3]));

        questWeights = questWeights.OrderBy(x => x.weight).ToList();

        for (int i = 0; i < questWeights.Count; ++i)
        {
            questWeights[i].weight = WEIGHTS[i];
            Debug.Log($"Quest Weight [{i}]: {questWeights[i].weight}");
        }

        pesos[0] = questWeights.Find(x => x.quest == "talk").weight;
        pesos[1] = questWeights.Find(x => x.quest == "get").weight;
        pesos[2] = questWeights.Find(x => x.quest == "kill").weight;
        pesos[3] = questWeights.Find(x => x.quest == "explore").weight;

        for (int i = 0; i < pesos.Length; ++i)
        {
            Debug.Log($"Pesos [{i}]: {pesos[i]}");
        }

        if (questWeights[questWeights.Count-1].quest == "talk") typePlayer = PlayerProfileEnum.Immersion;
        else if (questWeights[questWeights.Count - 1].quest == "get") typePlayer = PlayerProfileEnum.Achievement;
        else if (questWeights[questWeights.Count - 1].quest == "kill") typePlayer = PlayerProfileEnum.Mastery;
        else typePlayer = PlayerProfileEnum.Creativity;
    }
}