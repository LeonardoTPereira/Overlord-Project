using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCsa;

namespace FitnessScript
{
    public class Fitness
    {
        //private static float[] p = new float[3] {0.5f, 0.35f, 0.15f};
        //private static float[] p = new float[4] { 0.10f, 0.25f, 0.05f, 0.6f };
        //private static float[] p = new float[4] { 0.25f, 0.10f, 0.6f, 0.05f };
        //private static float[] p = new float[4] { 0.05f, 0.6f, 0.10f, 0.25f };
        private static float[] p = new float[4] { 0.5f, 0.25f, 0.15f, 0.1f };

        public static float Calculate(NPC_SO npc)
        {
            float fitness = 0;

            fitness = (0.5f * Fitness2(npc)) - (0.5f * Fitness1(npc));

            return fitness;
        }

        public static float Fitness1(NPC_SO npc)
        {
            return Mathf.Abs((npc.social + npc.attain + npc.violence + npc.explore) - npc.atrmax);
        }

        public static float Fitness2(NPC_SO npc)
        {
            return Social(npc) + Attain(npc) + Violence(npc) + Explore(npc);
        }

        public static float Social(NPC_SO npc)
        {
            float social = npc.social / ((p[1] * npc.attain) + (p[2] * npc.violence) + (p[3] * npc.explore));
            //float social = npc.social / npc.violence;
            return social;
        }

        public static float Attain(NPC_SO npc)
        {
            float attain = npc.attain / ((p[0] * npc.social) + (p[2] * npc.violence) + (p[3] * npc.explore));
            //float attain = npc.attain / npc.explore;
            //float attain = npc.attain;
            return attain;
        }

        public static float Violence(NPC_SO npc)
        {
            float violence = npc.violence / ((p[0] * npc.social) + (p[1] * npc.attain) + (p[3] * npc.explore));
            //float violence = npc.violence / npc.social;
            //float violence = npc.violence / (npc.attain + npc.social);
            return violence;
        }

        public static float Explore(NPC_SO npc)
        {
            float explore = npc.explore / ((p[0] * npc.social) + (p[1] * npc.attain) + (p[2] * npc.violence));
            //float explore = npc.explore / npc.attain;
            //float explore = npc.explore / npc.violence;
            return explore;
        }
    }
}