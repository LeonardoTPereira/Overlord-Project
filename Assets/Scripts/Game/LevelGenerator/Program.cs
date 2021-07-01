using EnemyGenerator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace LevelGenerator
{
    public class Program : MonoBehaviour
    {
        public TreasureRuntimeSetSO treasureRuntimeSetSO;
        public static event NewEAGenerationEvent newEAGenerationEventHandler;
        double min;
        double actual;
        //Flags if the dungeon has been gerated for Unity's Game Manager to handle things after
        public bool hasFinished;
        //Count time needed to create levels
        System.Diagnostics.Stopwatch watch;
        //The object to use the EA methods
        private EvolutionaryAlgorithm evolutionaryAlgorithm;
        private Fitness fitness;
        //Creates the first population of dungeons and generate their rooms
        List<Dungeon> dungeons;
        //The aux the Game Manager will access to load the created dungeon
        public Dungeon aux;
        JSonWriter.ParametersMonsters parametersMonsters;
        JSonWriter.ParametersItems parametersItems;
        JSonWriter.ParametersNpcs parametersNpcs;

        /**
         * The constructor of the "Main" behind the EA
         */
        public void Awake()
        {
            evolutionaryAlgorithm = new EvolutionaryAlgorithm();
            hasFinished = false;
            min = Double.MaxValue;
            watch = System.Diagnostics.Stopwatch.StartNew();
            dungeons = new List<Dungeon>(Constants.POP_SIZE);
            // Generate the first population
            for (int i = 0; i < dungeons.Capacity; ++i)
            {
                Dungeon individual = new Dungeon();
                individual.GenerateRooms();
                dungeons.Add(individual);
            }
            aux = dungeons[0];
        }

        public void OnEnable()
        {
            LevelGeneratorController.createEADungeonEventHandler += EvolveDungeonPopulation;
        }

        public void OnDisable()
        {
            LevelGeneratorController.createEADungeonEventHandler -= EvolveDungeonPopulation;
        }

        // The "Main" behind the Dungeon Generator
        public void EvolveDungeonPopulation(object sender, CreateEADungeonEventArgs eventArgs)
        {

            fitness = eventArgs.Fitness;
            parametersMonsters = eventArgs.ParametersMonsters;
            parametersItems = eventArgs.ParametersItems;
            parametersNpcs = eventArgs.ParametersNpcs;
            Thread t = new Thread(new ThreadStart(Evolve));
            t.Start();
            StartCoroutine(PrintAndSaveDungeonWhenFinished(t));
        }

        IEnumerator PrintAndSaveDungeonWhenFinished(Thread t)
        {
            while (t.IsAlive)
                yield return new WaitForSeconds(0.1f);
            aux.parametersMonters = parametersMonsters;
            aux.parametersNpcs = parametersNpcs;
            aux.parametersItems = parametersItems;
            Interface.PrintNumericalGridWithConnections(aux, fitness, treasureRuntimeSetSO);
            Debug.Log("Printed the dungeon");
        }

        public void Evolve()
        {
            Debug.Log("Start Creating Dungeon");
            int matrixOffset = Constants.MATRIXOFFSET;
            hasFinished = false;
            min = Double.MaxValue;

            watch = System.Diagnostics.Stopwatch.StartNew();
            dungeons = new List<Dungeon>(Constants.POP_SIZE);
            // Generate the first population
            for (int i = 0; i < dungeons.Capacity; ++i)
            {
                Dungeon individual = new Dungeon();
                individual.GenerateRooms();
                dungeons.Add(individual);
            }
            aux = dungeons[0];

            //Evolve all the generations from the GA
            for (int gen = 0; gen < Constants.GENERATIONS; ++gen)
            {
                //Get every dungeon's fitness
                foreach (Dungeon dun in dungeons)
                {
                    dun.fitness = fitness.CalculateFitness(dun, matrixOffset);
                }

                //Elitism = save the best solution
                aux = dungeons[0];
                foreach (Dungeon dun in dungeons)
                {
                    actual = dun.fitness;
                    if (min > actual)
                    {
                        min = actual;
                        aux = dun;
                    }
                }


                //Create the child population by doing the crossover and mutation
                List<Dungeon> childPop = new List<Dungeon>(dungeons.Count);
                for (int i = 0; i < (dungeons.Count / 2); ++i)
                {
                    int parentIdx1 = 0, parentIdx2 = 1;
                    EvolutionaryAlgorithm.Tournament(dungeons, ref parentIdx1, ref parentIdx2);
                    Dungeon parent1 = dungeons[parentIdx1].Copy();
                    Dungeon parent2 = dungeons[parentIdx2].Copy();

                    try
                    {
                        EvolutionaryAlgorithm.Crossover(ref parent1, ref parent2);

                        aux = dungeons[0];
                        EvolutionaryAlgorithm.Mutation(ref parent1);
                        EvolutionaryAlgorithm.Mutation(ref parent2);
                        //We need to fix the room list anytime a room is altered in the tree.
                        parent1.FixRoomList();
                        parent2.FixRoomList();
                    }
                    catch (System.Exception e)
                    {
                        Debug.Log(e.Message);
                        Util.OpenUri("https://stackoverflow.com/search?q=" + e.Message);
                    }
                    //Calculate the average number of children from the rooms in each children
                    parent1.CalcAvgChildren();
                    parent2.CalcAvgChildren();
                    //Add the children to the new population
                    childPop.Add(parent1);
                    childPop.Add(parent2);
                }

                //Elitism - now we get back the best one to the first position
                childPop[0] = aux;
                dungeons = childPop;
                Debug.Log("Finished another generation");
                newEAGenerationEventHandler?.Invoke(this, new NewEAGenerationEventArgs((int)(((gen + 1) / (float)Constants.GENERATIONS) * 100)));
            }
            //Find the best individual in the final population and print it as the answer
            min = Double.MaxValue;
            aux = dungeons[0];
            foreach (Dungeon dun in dungeons)
            {
                dun.fitness = fitness.CalculateFitness(dun, matrixOffset);
                actual = dun.fitness;
                if (min > actual)
                {
                    min = actual;
                    aux = dun;
                }
            }
            watch.Stop();
            long time = watch.ElapsedMilliseconds;

            hasFinished = true;

            //Saves the test file that we used in the master thesis
            //CSVManager.SaveCSVLevel(id, aux.nKeys, aux.nLocks, aux.RoomList.Count, aux.AvgChildren, aux.neededLocks, aux.neededRooms, min, time, Constants.RESULTSFILE+"-"+Constants.nV+"-" + Constants.nK + "-" + Constants.nL + "-" + Constants.lCoef + ".csv");

            //Print info from best level if needed
            /*Debug.Log("Finished - fitnes:" + aux.fitness);
            Debug.Log("R:"+ aux.RoomList.Count+"-K:" + aux.nKeys + "-L:"+ aux.nLocks + "-Lin:"+aux.AvgChildren +"-nL:"+aux.neededLocks+"-nR:"+aux.neededRooms);
            Debug.Log("nRdelta:"+System.Math.Abs(aux.RoomList.Count * 0.8f - aux.neededRooms)+"-80p:"+ aux.RoomList.Count * 0.8f);*/

            //This method prints the dungeon in the console (not unity's one) AND saves it into a file
            //We should clear the dungeons... but Game Manager is using aux. So we could and should do a hard copy. But i'm not touching that spaghetti right now
            //TODO: touch spaghetti later
            //dungeons.Clear();
        }
    }
}
