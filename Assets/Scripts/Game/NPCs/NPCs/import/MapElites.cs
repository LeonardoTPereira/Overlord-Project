using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumRaces;
using EnumJobs;
using FitnessScript;
using Game.NPCsa;

namespace MapElite
{
    public class MapElites
    {
        // mapa local
        public NPC_SO[,] map { get; set; }

        // mapa ja existe
        public bool exists { get; private set; } = false;

        // quest
        public int questID;

        //salvar dados como a quantidade de jobs e racas
        public int xAxis = Races.NumberOfRaces();
        public int yAxis = Jobs.NumberOfJobs();

        float[] weights;

        // sem questID
        public MapElites(float[] weights)
        {
            map = new NPC_SO[xAxis, yAxis];
            this.weights = new float[3];
            this.weights[0] = weights[0];
            this.weights[1] = weights[0] + weights[1];
            this.weights[2] = weights[0] + weights[1] + weights[2];
            questID = QuestID(weights);
        }

        private int QuestID(float[] w)
        {
            int index = 0;
            float aux = float.MinValue;

            for (int i = 0; i < w.Length; i++)
            {
                if (w[i] > aux)
                {
                    aux = w[i];
                    index = i;
                }
            }

            return index;
        }

        public void InsertNPCList(NPC_SO[] list, int popSize)
        {
            for (int i = 0; i < popSize; i++)
            {
                InsertNPC(list[i]);
            }
        }

        public void InsertNPC(NPC_SO npc)
        {
            //int xTarget = npc.combination;
            //int yTarget = npc.quest;

            float px = npc.race;
            float py = npc.job;
            int xTarget = 0, yTarget = 0;

            if (px >= 0 && px < weights[0])
            {
                // talk
                xTarget = (int)Races.raceID.Anao;
            }
            else if (px >= weights[0] && px < weights[1])
            {
                // attain
                xTarget = (int)Races.raceID.Elfo;
            }
            else if (px >= weights[1] && px < weights[2])
            {
                // violence
                xTarget = (int)Races.raceID.Ogro;
            }
            else if (px >= weights[2] && px <= 1)
            {
                // explore
                xTarget = (int)Races.raceID.Humano;
            }

            if (py >= 0 && py < weights[0])
            {
                // talk
                yTarget = (int)Jobs.jobID.Comerciante;
            }
            else if (py >= weights[0] && py < weights[1])
            {
                // attain
                yTarget = (int)Jobs.jobID.Mago;
            }
            else if (py >= weights[1] && py < weights[2])
            {
                // violence
                yTarget = (int)Jobs.jobID.Soldado;
            }
            else if (py >= weights[2] && py <= 1)
            {
                // explore
                yTarget = (int)Jobs.jobID.Pirata;
            }

            if (isOccupiedOnCoords(xTarget, yTarget))
            {
                map[xTarget, yTarget] = GetBest(map[xTarget, yTarget], npc);
            }
            else
            {
                map[xTarget, yTarget] = npc;
            }
        }

        // verifica se uma coordenada do mapa está ocupada
        private bool isOccupiedOnCoords(int xTarget, int yTarget)
        {
            if (map[xTarget, yTarget] == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // escolhe o melhor dentre dois npcs
        public NPC_SO GetBest(NPC_SO a, NPC_SO b)
        {
            float scoreA = a.fitness;
            float scoreB = b.fitness;

            //pode ter o isbest no lugar
            if (scoreA >= scoreB)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

    }

}