﻿using EnemyGenerator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
            hasFinished = false;
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
            while (t.IsAlive)
                yield return new WaitForSeconds(0.1f);
            // TODO: for in the solution
            // dungeon.SetNarrativeParameters(parametersMonsters, parametersNpcs, parametersItems, playerProfile, narrativeName);
            // Interface.PrintNumericalGridWithConnections(dungeon, fitness, treasureRuntimeSetSO);
            Debug.Log("The dungeons were printed!");
        }

        public void Evolve()
        {
            hasFinished = false;
            Debug.Log("Start creating dungeons...");
            generator = new LevelGenerator(prs, newEAGenerationEventHandler);
            generator.Evolve();
            Debug.Log("The dungeons were created!");
            hasFinished = true;
        }
    }
}
