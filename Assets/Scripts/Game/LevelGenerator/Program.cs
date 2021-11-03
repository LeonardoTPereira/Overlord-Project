using EnemyGenerator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using ScriptableObjects;
using UnityEngine;
using static Enums;

namespace LevelGenerator
{
    public class Program : MonoBehaviour
    {
        /// The parameters of the evolutionary process
        private static readonly int MAX_TIME = 60;
        private static readonly int INITIAL_POPULATION_SIZE = 20;
        private static readonly int MUATION_RATE = 5;
        private static readonly int NUMBER_OF_COMPETITORS = 3;
        Parameters prs;

        /// Level generator
        private LevelGenerator generator;
        private Fitness fitness;

        /// Attributes to communicate to Game Manager
        // Flags if the dungeon has been gerated for Unity's Game Manager to handle things after
        public bool hasFinished;
        // The aux the Game Manager will access to load the created dungeon
        public Dungeon aux;
        // The event to handle the progress bar update
        public static event NewEAGenerationEvent newEAGenerationEventHandler;

        /// The external parameters of printing purposes
        public TreasureRuntimeSetSO treasureRuntimeSetSO;
        JSonWriter.ParametersMonsters parametersMonsters;
        JSonWriter.ParametersItems parametersItems;
        JSonWriter.ParametersNpcs parametersNpcs;
        string playerProfile;
        string narrativeName;

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
            playerProfile = eventArgs.PlayerProfile;
            narrativeName = eventArgs.NarrativeName;
            // Define the evolutionary parameters
            prs = new Parameters(
                (new System.Random()).Next(), // Random seed
                MAX_TIME,                 // Maximum time
                INITIAL_POPULATION_SIZE,  // Initial population size
                MUATION_RATE,             // Mutation chance
                NUMBER_OF_COMPETITORS,    // Number of tournament competitors
                fitness.DesiredRooms,     // Number of rooms
                fitness.DesiredKeys,      // Number of keys
                fitness.DesiredLocks,     // Number of locks
                fitness.DesiredEnemies,   // Number of enemies
                fitness.DesiredLinearity, // Linear coefficient
                fitness // Object that calculates the fitness of individuals
            );
            // Start the generation process
            Thread t = new Thread(new ThreadStart(Evolve));
            t.Start();
            StartCoroutine(PrintAndSaveDungeonWhenFinished(t));
        }

        IEnumerator PrintAndSaveDungeonWhenFinished(Thread t)
        {
            // Wait until the dungeons were generated
            while (t.IsAlive)
                yield return new WaitForSeconds(0.1f);
            // Write all the generated dungeons in ScriptableObjects
            Population solution = generator.Solution;
            for (int e = 0; e < solution.dimension.exp; e++)
            {
                for (int l = 0; l < solution.dimension.len; l++)
                {
                    Individual individual = solution.map[e, l];
                    if (individual != null)
                    {
                        individual.dungeon.SetNarrativeParameters(parametersMonsters, parametersNpcs, parametersItems, playerProfile, narrativeName);
                        Interface.PrintNumericalGridWithConnections(individual, fitness, treasureRuntimeSetSO);
                    }
                }
            }
            Debug.Log("The dungeons were printed!");
            // Set the first level as the option to be played in the scene
            aux = solution.map[0, 0].dungeon;
        }

        public void Evolve()
        {
            int matrixOffset = Constants.MATRIXOFFSET;
            hasFinished = false;
            Debug.Log("Start creating dungeons...");
            generator = new LevelGenerator(prs, newEAGenerationEventHandler);
            generator.Evolve();
            Debug.Log("The dungeons were created!");
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
