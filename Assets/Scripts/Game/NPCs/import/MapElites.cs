using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumRaces;
using EnumJobs;
using FitnessScript;

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

    public MapElites(int questID)
    {
        map = new NPC_SO[xAxis, yAxis];
        this.questID = questID;
    }

    public MapElites(int xAxis, int yAxis, int questID)
    {
        map = new NPC_SO[xAxis, yAxis];
        this.xAxis = xAxis;
        this.yAxis = yAxis;
        this.questID = questID;
    }

    // preenche o map elite inteiro
    public void GenerateRandomMap()
    {
        if (exists)
        {
            Debug.LogError("O mapa já foi gerado.");
            return;
        }

        NPC_SO npc;
        for (int i = 0; i < xAxis; i++)
        {
            for (int j = 0; j < yAxis; j++)
            {
                npc = CreateRandomNPC(i, j);
                InsertNPC(npc, i, j);
            }
        }

        exists = true;
    }

    // gera amount npcs em lugares aleatorios do mapa
    public void GenerateRandomMap(int amount)
    {
        if (exists)
        {
            Debug.LogError("O mapa já foi gerado.");
            return;
        }

        int xRandom = 0, yRandom = 0;
        NPC_SO npc;

        if(amount > (xAxis * yAxis))
        {
            amount = xAxis * yAxis;
        }
        else if(amount <= 0)
        {
            amount = 3;
        }

        for(int i = 0; i < amount;)
        {
            xRandom = Random.Range(0, xAxis - 1);
            yRandom = Random.Range(0, yAxis - 1);

            npc = CreateRandomNPC(xRandom, yRandom);

            if(InsertNPC(npc, xRandom, yRandom))
            {
                i++;
            }
        }

        exists = true;
    }

    // cria um npc com atributos aleatorios, excluindo job e raca
    private NPC_SO CreateRandomNPC(int x, int y)
    {
        NPC_SO npc = ScriptableObject.CreateInstance<NPC_SO>();
        npc.Init(x, y);
        /*npc.Init(
            "To be Defined",
            (Random.Range(0, 100)),
            y,
            x,
            (Random.Range(0, 10)),
            (Random.Range(0, 10)),
            (Random.Range(0, 10)),
            (Random.Range(0, 10)),
            (Random.Range(0, 10))
        );*/

        return npc;
    }

    public NPC_SO GetNPC(int x, int y)
    {
        return map[x, y];
    }

    public NPC_SO GetRandomNPC()
    {
        return map[Random.Range(0, xAxis), Random.Range(0, yAxis)];
    }

    // tenta inserir um npc em xtarget e ytarget do mapa. retorna falso se ja estava ocupado e fez competicao
    private bool InsertNPC(NPC_SO npc, int xTarget, int yTarget)
    {
        if(isOccupiedOnCoords(xTarget, yTarget))
        {
            map[xTarget, yTarget] = GetBest(map[xTarget, yTarget], npc);
            return false;
        }
        else
        {
            map[xTarget, yTarget] = npc;
            return true;
        }
    }

    // Insere somente um
    public void InsertNPC(NPC_SO npc)
    {
        int xTarget = npc.race;
        int yTarget = npc.job;

        if (isOccupiedOnCoords(xTarget, yTarget))
        {
            map[xTarget, yTarget] = GetBest(map[xTarget, yTarget], npc);
        }
        else
        {
            map[xTarget, yTarget] = npc;
        }
    }

    // Insere array
    public void InsertNPC(NPC_SO[] npc)
    {
        int xTarget, yTarget;

        for (int i = 0; i < npc.Length; i++)
        {
            xTarget = npc[i].race;
            yTarget = npc[i].job;

            if (isOccupiedOnCoords(xTarget, yTarget))
            {
                map[xTarget, yTarget] = GetBest(map[xTarget, yTarget], npc[i]);
            }
            else
            {
                map[xTarget, yTarget] = npc[i];
            }
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
        float scoreA = Fitness.Calculate(a, questID);
        float scoreB = Fitness.Calculate(b, questID);

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
