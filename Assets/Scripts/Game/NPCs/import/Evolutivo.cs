using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FitnessScript;
using EnumJobs;
using EnumRaces;

// evolutivo - referenciar breno
public class Evolutivo
{
    //usar random na classe do map elite e puxar as funcoes de la aqui
    //ou colocar o getrandom no npc_so?
    private static int thisQuest = 0;
    private static NPC_SO[,] map;

    //teste
    private MapElites mapElite;

    // Realiza o processo evolutivo da população
    public static NPC_SO[,] EvolutionProcess(NPC_SO[,] randomPopulation, int numIter, int questType)
    {
        map = randomPopulation;

        NPC_SO[] parents = new NPC_SO[2];
        NPC_SO[] children = new NPC_SO[2];
        thisQuest = questType;

        //no map elites ja teria esse dado salvo
        // lembrar de transformar o map elite em um tipo de dados
        int numRaces = Races.NumberOfRaces();
        int numJobs = Jobs.NumberOfJobs();

        // numIter = numero de individuos que vai ser gerado num ciclo de evolucao?
        for (int i = 0; i < numIter; i++)
        {
            //escolhe dois pais
            for (int j = 0; j < 2; j++)
            {
                // duelo podia fazer o random dentro da funcao
                parents[j] = GetBest(map[(int)Random.Range(0, numRaces), (int)Random.Range(0, numJobs)],
                                     map[(int)Random.Range(0, numRaces), (int)Random.Range(0, numJobs)]
                );
            }

            children = Crossover(parents);

            //adicionar uma chance de ocorrer uma mutacao <aqui>
            //s e m p r e fazer mutacao tem risco (pouca mutacao eh okay (mas mais tentativa e erro))

            if (Random.value < 0.5f)
            {
                Mutate(children[0]);
                Mutate(children[1]);
            }

            AttemptInsert(children);
        }

        return map;
    }
    
    public static NPC_SO[,] EvolutionProcess(MapElites mapElite, int numIter)
    {
        NPC_SO[] parents = new NPC_SO[2];
        NPC_SO[] children = new NPC_SO[2];

        // numIter = numero de individuos que vai ser gerado num ciclo de evolucao?
        for (int i = 0; i < numIter; i++)
        {
            // escolhe dois pais
            for (int j = 0; j < 2; j++)
            {
                // ainda assumindo mapa sempre cheio
                parents[j] = mapElite.GetBest(mapElite.GetRandomNPC(), mapElite.GetRandomNPC());
            }

            children = Crossover(parents);
            
            // s e m p r e  fazer mutacao tem risco (pouca mutacao é ok (mas e mais tentativa e erro))
            if (Random.value < 0.5f)
            {
                Mutate(children[0]);
                Mutate(children[1]);
            }

            mapElite.InsertNPC(children);
        }

        return mapElite.map;
    }

    // Muta atributos aleatorios
    private static void Mutate(NPC_SO person)
    {
        float threshold = 0.3f;
        // mesma ideia de colocar constantes
        // a constante pode estar no NPC_SO pra ficar tudo num lugar so
        if (Random.value < threshold)
        {
            person.age = Random.Range(0, 100);
        }
        if (Random.value < threshold)
        {
            person.job = Random.Range(0, Jobs.NumberOfJobs() - 1);
        }
        if (Random.value < threshold)
        {
            person.race = Random.Range(0, Races.NumberOfRaces() - 1);
        }
        if (Random.value < threshold)
        {
            person.trauma = Random.Range(0, 10);
        }
        if (Random.value < threshold)
        {
            person.violence = Random.Range(0, 10);
        }
        if (Random.value < threshold)
        {
            person.attain = Random.Range(0, 10);
        }
        if (Random.value < threshold)
        {
            person.explore = Random.Range(0, 10);
        }
        if (Random.value < threshold)
        {
            person.social = Random.Range(0, 10);
        }
    }

    // Força duelo. NPC ganhador é retornado
    // pode deixar essa funcao no fitness
    private static NPC_SO GetBest(NPC_SO a, NPC_SO b)
    {
        float scoreA = Fitness.Calculate(a, thisQuest);
        float scoreB = Fitness.Calculate(b, thisQuest);

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

    // Realiza o processo de crossover gerando dois novos filhos (?)
    private static NPC_SO[] Crossover(NPC_SO[] parents)
    {
        NPC_SO[] children = new NPC_SO[2];

        for (int i = 0; i < 2; i++)
        {
            children[i] = ScriptableObject.CreateInstance<NPC_SO>();

            //trocar divisao por dois por algo melhor no futuro - blxalpha?
            children[i].identity = "To be Defined";
            children[i].age = (parents[0].age + parents[1].age) / 2;

            children[i].trauma = (parents[0].trauma + parents[1].trauma) / 2;
            children[i].violence = (parents[0].violence + parents[1].violence) / 2;
            children[i].attain = (parents[0].attain + parents[1].attain) / 2;
            children[i].explore = (parents[0].explore + parents[1].explore) / 2;
            children[i].social = (parents[0].social + parents[1].social) / 2;
        }

        // Realização de crossover tradicional a fim de evitar competição dos filhos com os próprios pais
        // Isso possibilita uma melhor exploração do Map Elite
        // swap
        children[0].job = parents[0].job;
        children[0].race = parents[1].race;
        children[1].job = parents[1].job;
        children[1].race = parents[0].race;

        return children;
    }

    // inserir um individuo por vez (nem sempre vamos ter uma lista)
    // A partir dos filhos obtidos, tenta inserir no Map Elite. Se falhar, nao insere.
    private static void AttemptInsert(NPC_SO[] children)
    {
        int race, job;

        for (int i = 0; i < 2; i++)
        {
            race = children[i].race;
            job = children[i].job;

            map[race, job] = GetBest(children[i], map[race, job]);
        }
    }

}


