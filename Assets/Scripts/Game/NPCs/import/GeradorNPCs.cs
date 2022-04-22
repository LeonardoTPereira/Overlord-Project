using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using EnumRaces;
using EnumJobs;
using FitnessScript;

public class GeradorNPCs : MonoBehaviour
{
    // Mapa: RACES x JOBS

    // Declarado em funcao no start()
    [Header("Inicializações")]
    [SerializeField] public int questType = 0;
    [SerializeField] public int generateAmount = 0;
    [SerializeField] public int iterationTimes = 0;

    public int debugSeed = 1337;
    public int seed;
    public bool debugMode;
    
    private string parentFolder = "Assets/Resources/NPCs";
    private string execFolderName = "Execucao";

    private CSVManager csvm;

    // O Start() so e usado para proposito de testes
    private void Start()
    {
        csvm = new CSVManager();
        //iterationTimes = 15000;
        //generateAmount = 10;
        //questType = 1;

        //Generate(generateAmount, iterationTimes, questType);
        Generate(200, 1);
    }

    // Entrada: quantos npcs vai gerar e qual quest sera o foco. numero de iteracoes declarado no start
    public void Generate(int nITEs, int questID)
    {
        MapElites mapElite = new MapElites(questID);

        if (debugMode)
        {
            seed = debugSeed;
        }
        else
        {
            seed = Random.Range(-int.MaxValue, int.MaxValue);
        }

        Debug.Log("Seed usada: " + seed);

        // Inicializar Random
        Random.InitState(seed);

        // Recebe populacao inicial
        //NPC_SO[,] map = GenerateRandom();
        mapElite.GenerateRandomMap();

        // Evolutivo
        //mapElite.map = Evolutivo.EvolutionProcess(mapElite.map, nITEs, questID);
        Evolutivo.EvolutionProcess(mapElite, nITEs);

        // Gera arquivos sobre a populacao
        NPCToSO(mapElite.map);

        // Salva resultados de fitness em um .csv
        //csvm.SaveMapFitness(mapElite.map, questID, parentFolder);
        csvm.SaveMapFitness(mapElite, parentFolder);
    }

    /* Gera a populacao aleatoria
    // seria legal ter a opcao de gerar sem ter o mapa cheio / menos individuos
    // separar esse método
    // while com uma quantidade x para ser a populacao do map elite
    // com while, ja aplicar o map elites
    // deixar o map elites como uma classe  <!!!>. vai tornar isso numa estrutura de dados// construtor, qtd de racas trabalhos.../ depois um metodo pra inserir na matriz que vai classificar onde vai se encaixar e la ja verifica se e melhor ou nao. tendo um metodo dedicado a isso simplifica o codigo
    // usar valor default -1 -> se for -1, trocar; se for != -1, aplicar map elite?
    private NPC_SO[,] GenerateRandom()
    {
        int numRaces = Races.NumberOfRaces();
        int numJobs = Jobs.NumberOfJobs();
        NPC_SO[,] map = new NPC_SO[numRaces, numJobs];

        for(int i = 0; i < numRaces; i++)
        {
            for(int j = 0; j < numJobs; j++)
            {
                // trocar para constantes (const valor) e trocar pra facilitar edicao depois
                map[i, j] = ScriptableObject.CreateInstance<NPC_SO>();
                map[i, j].Init(
                "To be Defined",
                (int)(Random.Range(0, 100)),
                j,
                i,
                (int)(Random.Range(0, 10)),
                (int)(Random.Range(0, 10)),
                (int)(Random.Range(0, 10)),
                (int)(Random.Range(0, 10)),
                (int)(Random.Range(0, 10))
                );
            }
        }

        return map;
    }
    */

    // Cria um asset Scriptable Object para os NPCs, com caminho definido para Assets/FNPC/
    private void NPCToSO(NPC_SO[,] map)
    {
        int k = 0;
        string guid = AssetDatabase.CreateFolder(parentFolder, execFolderName);
        string uniqueFileName = AssetDatabase.GUIDToAssetPath(guid);

        for (int i = 0; i < Races.NumberOfRaces(); i++)
        {
            for(int j = 0; j < Jobs.NumberOfJobs(); j++)
            {
                // map[i, j].PrintNPCData(k, questType);

#if UNITY_EDITOR
                AssetDatabase.CreateAsset(map[i, j], uniqueFileName + "/NPC" + k + ".asset");
#endif
                k++;
            }
        }

        return;
    }
}
