using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FitnessScript
{
    public class Fitness
    {

        public static float Calculate(NPC_SO npc, int quest)
        {
            float fitness = 0;
            float p1 = 0, p2 = 0, p3 = 0, p4 = 0;

            // age x trauma ver depois
            //transformar o tipo de quest em enum
            //com enum nao precisa nem de default

            switch (quest)
            {
                case 0:
                    // pesos talk 1.3
                    p1 = 0.6f;
                    p2 = 0.1f;
                    p3 = 0.25f;
                    p4 = 0.05f;
                    break;

                case 1:
                    // pesos get 1.3
                    p1 = 0.1f;
                    p2 = 0.6f;
                    p3 = 0.05f;
                    p4 = 0.25f;
                    break;

                case 2:
                    // pesos kill 1.3
                    p1 = 0.25f;
                    p2 = 0.05f;
                    p3 = 0.6f;
                    p4 = 0.1f;
                    break;

                case 3:
                    // pesos explore 1.3
                    p1 = 0.05f;
                    p2 = 0.25f;
                    p3 = 0.1f;
                    p4 = 0.6f;
                    break;

                default:
                    //  Caso de erro
                    Debug.Log("Erro no calculate() do fitness");
                    break;
            }

            fitness = p1 * npc.social + p2 * npc.attain + p3 * npc.violence + p4 * npc.explore;

            return fitness;
        }
    }
}

