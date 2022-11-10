using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FitnessScript;
using EnumJobs;
using EnumRaces;
using MapElite;
using Game.NPCsa;

namespace EvolutivoScript
{
    public class Evolutivo
    {
        public static NPC_SO[] list { get; set; }
        private static NPC_SO[] parents = new NPC_SO[2];
        private static NPC_SO[] children = new NPC_SO[2];
        private static NPC_SO[] mergeList;

        // lista ja existe
        public static bool exists { get; private set; } = false;

        // tamanho da lista
        public static int listSize;
        public static int childCount = 0;

        private static int step = 0;

        public static MapElites EvolutionProcess(int numIter, int popSize, float[] w)
        {
            MapElites mapElite = new MapElites(w);
            NPC_SO best;
            listSize = popSize;
            InitList();

            // Repetir este loop ate formar uma nova populacao de 100 individuos
            // Esta sempre pegando o melhor entre dois pais aleatorios, mas nao parece estar evoluindo apropriadamente...
            // Talvez precise de uma quantidade abstratamente maior de individuos para funcionar?
            for (int i = 0; i < numIter; i++)
            {
                while (NeedsToGenerate())
                {
                    SetParents();

                    Crossover();

                    if (Random.value < 0.5f)
                    {
                        Mutate();
                    }

                    InsertChildren();
                }

                UpdatePopulation();

                best = SeekBest();
                mapElite.InsertNPC(best);
            }

            return mapElite;
        }

        public static void InitList()
        {
            list = new NPC_SO[listSize];
            mergeList = new NPC_SO[listSize];

            exists = true;

            for (int i = 0; i < listSize; i++)
            {
                list[i] = ScriptableObject.CreateInstance<NPC_SO>();
                list[i].Init();
            }
        }

        public static void SetParents()
        {
            if (!exists)
            {
                Debug.LogError("Lista nao existe");
                return;
            }
            if (step != 0)
            {
                Debug.LogError("Pais já gerados");
                return;
            }

            for (int i = 0; i < 2; i++)
            {
                parents[i] = GetBest(GetRandom(), GetRandom());
            }

            step++;
        }

        public static NPC_SO GetRandom()
        {
            return list[Random.Range(0, listSize)];
        }

        public static NPC_SO GetBest(NPC_SO npc1, NPC_SO npc2)
        {
            if (npc1.fitness > npc2.fitness)
            {
                return npc1;
            }
            else
            {
                return npc2;
            }
        }

        public static void Crossover()
        {
            if (!exists)
            {
                Debug.LogError("Lista nao existe");
                return;
            }
            if (step == 0)
            {
                Debug.LogError("Pais não encontrados.");

            }
            else if (step != 1)
            {
                Debug.LogError("Crossover já executado.");
            }

            int atrmax = 0;
            float alpha = Random.Range(0f, 1f);
            float race, job, violence, attain, explore, social;

            for (int i = 0; i < 2; i++)
            {
                // Define atributos com blx-alpha
                atrmax = (int)(((1 - alpha) * parents[0].atrmax) + (alpha * parents[1].atrmax));

                if (i == 0)
                {
                    race = parents[0].race;
                    job = parents[1].job;
                }
                else
                {
                    race = parents[1].race;
                    job = parents[0].job;
                }

                violence = ((1 - alpha) * parents[0].violence) + (alpha * parents[1].violence);
                attain = ((1 - alpha) * parents[0].attain) + (alpha * parents[1].attain);
                explore = ((1 - alpha) * parents[0].explore) + (alpha * parents[1].explore);
                social = ((1 - alpha) * parents[0].social) + (alpha * parents[1].social);

                // Cria filhos
                children[i] = ScriptableObject.CreateInstance<NPC_SO>();
                children[i].Init(atrmax, race, job, violence, attain, explore, social);

                // Inverte alpha
                alpha = (1 - alpha);
            }

            step++;
        }

        private static void Mutate()
        {
            for (int i = 0; i < 2; i++)
            {
                float threshold = 0.3f;
                // mesma ideia de colocar constantes
                // a constante pode estar no NPC_SO pra ficar tudo num lugar so
                if (Random.value < threshold)
                {
                    children[i].atrmax = Random.Range(10, 30);
                }
                if (Random.value < threshold)
                {
                    children[i].race = Random.Range(0.0f, 1.0f);
                }
                if (Random.value < threshold)
                {
                    children[i].job = Random.Range(0.0f, 1.0f);
                }

                if (Random.value < threshold)
                {
                    children[i].violence = Random.Range(1f, 10f);
                }
                if (Random.value < threshold)
                {
                    children[i].attain = Random.Range(1f, 10f);
                }
                if (Random.value < threshold)
                {
                    children[i].explore = Random.Range(1f, 10f);
                }
                if (Random.value < threshold)
                {
                    children[i].social = Random.Range(1f, 10f);
                }

                children[i].UpdateFitness();
            }
        }

        public static void InsertChildren()
        {
            if (!exists)
            {
                Debug.LogError("Lista nao existe");
                return;
            }
            if (step != 2)
            {
                Debug.LogError("Filhos não gerados.");
                return;

            }
            else if (childCount >= listSize)
            {
                Debug.LogError("População nova não precisa de mais filhos.");
                return;
            }

            mergeList[childCount] = children[0];
            mergeList[childCount + 1] = children[1];

            childCount += 2;

            step = 0;
        }

        public static bool NeedsToGenerate()
        {
            if (!exists)
            {
                Debug.LogError("Lista nao existe");
                return false;
            }

            return (childCount < listSize);
        }

        public static void UpdatePopulation()
        {
            if (!exists)
            {
                Debug.LogError("Lista nao existe");
                return;
            }
            list = mergeList;
            childCount = 0;
            step = 0;
        }

        public static NPC_SO SeekBest()
        {
            NPC_SO best = list[0];

            for (int i = 1; i < list.Length; i++)
            {
                best = GetBest(best, list[i]);
            }

            return best;
        }
    }

}

