using Game.EnemyGenerator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using MyBox;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using static Util.Enums;

namespace LevelGenerator
{
    public class LevelGeneratorManager : MonoBehaviour
    {
        /// The parameters of the evolutionary process
        private static readonly int MAX_TIME = 60;
        private static readonly int INITIAL_POPULATION_SIZE = 20;
        private static readonly int MUATION_RATE = 5;
        private static readonly int NUMBER_OF_COMPETITORS = 2;
        private Parameters _parameters;

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
        [MustBeAssigned]
        public TreasureRuntimeSetSO treasureRuntimeSetSO;
        [MustBeAssigned]
        public WeaponTypeRuntimeSetSO weaponTypeRuntimeSetSO;
        private QuestLine _questLine;
        private List<DungeonFileSo> _dungeonFileSos;

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
            QuestGeneratorManager.CreateEaDungeonEventHandler += EvolveDungeonPopulation;
        }

        public void OnDisable()
        {
            LevelGeneratorController.createEADungeonEventHandler -= EvolveDungeonPopulation;
            QuestGeneratorManager.CreateEaDungeonEventHandler -= EvolveDungeonPopulation;
        }

        // The "Main" behind the Dungeon Generator
        public void EvolveDungeonPopulation(object sender, CreateEADungeonEventArgs eventArgs)
        {
            fitness = eventArgs.Fitness;
            Debug.Log($"Fitness - room={fitness.DesiredRooms}, keys={fitness.DesiredKeys}, " +
                      $"locks={fitness.DesiredLocks}, linearity={fitness.DesiredLinearity}, " +
                      $"enemies{fitness.DesiredEnemies}");
            _questLine = eventArgs.QuestLineForDungeon;
            Debug.Log("Quest Line: "+_questLine.name);
            // Define the evolutionary parameters
            _parameters = new Parameters(
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
            _dungeonFileSos = new List<DungeonFileSo>();
            // Write all the generated dungeons in ScriptableObjects
            Population solution = generator.Solution;
            for (var e = 0; e < solution.dimension.exp; e++)
            {
                for (var l = 0; l < solution.dimension.len; l++)
                {
                    var individual = solution.map[e, l];
                    if (individual != null)
                    {
                        Interface.PrintNumericalGridWithConnections(individual, fitness, _questLine);
                    }
                }
            }
            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #endif
            Debug.Log("The dungeons were printed!");
            // Set the first level as the option to be played in the scene
            aux = solution.map[0, 0].dungeon;
        }

        public void Evolve()
        {
            hasFinished = false;
            Debug.Log("Start creating dungeons...");
            generator = new LevelGenerator(_parameters, newEAGenerationEventHandler);
            generator.Evolve();
            Debug.Log("The dungeons were created!");
            hasFinished = true;
        }
    }
}