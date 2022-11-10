using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using EnumRaces;
using EnumJobs;
using FitnessScript;
using MapElite;
using EvolutivoScript;
using Game.NPCsa;
using CsvManagement;

namespace GeradorNPC
{
    public class GeradorNPCs : MonoBehaviour
    {
        // Mapa: RACES x JOBS

        // Declarado em funcao no start()
        [Header("Inicializações")]
        [SerializeField] public int questType = 0;
        [SerializeField] public int generateAmount = 0;
        [SerializeField] public int iterationTimes = 0;

        //debug
        float[] previewWeights = new float[4] { 0.10f, 0.25f, 0.05f, 0.6f };
        //float[] previewWeights = new float[4] { 0.25f, 0.10f, 0.6f, 0.05f };
        //float[] previewWeights = new float[4] { 0.05f, 0.6f, 0.10f, 0.25f };
        //float[] previewWeights = new float[4] { 0.6f, 0.25f, 0.10f, 0.05f };

        public int debugSeed = 1234;
        public int seed;
        public int popSize = 100;
        public bool debugMode;

        private string filename;
        private string parentFolder = "Assets/Resources/NPCs";
        private string execFolderName = "Execucao";
        private string csvFileName = "/iterations.csv";
        private string execFileName = "";

        private CSVManager csvm;

        // O Start() so e usado para proposito de testes
        private void Start()
        {
            //csvm = new CSVManager();
            //debugMode = true;
            iterationTimes = 1000;
            //generateAmount = 10;
            //questType = 1;

            Generate(iterationTimes, previewWeights);
        }

        public void Generate(int nITEs, float[] w)
        {

            MapElites mapElite;
            filename = "Assets/Resources/NPCs";

            if (debugMode)
            {
                seed = debugSeed;
            }
            else
            {
                seed = Random.Range(-int.MaxValue, int.MaxValue);
            }

            Debug.Log("Seed usada: " + seed);
            Random.InitState(seed);

            mapElite = Evolutivo.EvolutionProcess(nITEs, popSize, w);

            // Gera arquivos sobre a populacao
            execFileName = NPCToSO(mapElite.map);
        }

        // Cria um asset Scriptable Object para os NPCs, com caminho definido para Assets/FNPC/
        private string NPCToSO(NPC_SO[,] map)
        {
            int k = 0;
            string guid = AssetDatabase.CreateFolder(parentFolder, execFolderName);
            string uniqueFileName = AssetDatabase.GUIDToAssetPath(guid);

            for (int i = 0; i < Races.NumberOfRaces(); i++)
            {
                for (int j = 0; j < Jobs.NumberOfJobs(); j++)
                {
#if UNITY_EDITOR
                    if (map[i, j] != null)
                    {
                        AssetDatabase.CreateAsset(map[i, j], uniqueFileName + "/NPC" + k + ".asset");
                    }
#endif
                    k++;
                }
            }
            Debug.Log("filename = " + uniqueFileName);
            return uniqueFileName;
        }
    }

}