using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Game.Events;
using Game.GameManager;
using Game.LevelGenerator.EvolutionaryAlgorithm;
using Game.LevelGenerator.LevelSOs;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using MyBox;
using ScriptableObjects;
using UnityEngine;

namespace Game.LevelGenerator
{
    public class LevelGeneratorManager : MonoBehaviour
    {
        /// Level generator
        private LevelGenerator generator;
        private Parameters parameters;

        /// Attributes to communicate to Game Manager
        // Flags if the dungeon has been gerated for Unity's Game Manager to handle things after
        public bool hasFinished;
        private QuestLine _questLine;

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
            parameters = eventArgs.Parameters;
            _questLine = eventArgs.QuestLineForDungeon;
            var fitnessParameters = parameters.FitnessParameters;
            Debug.Log(fitnessParameters.DesiredEnemies + " " + fitnessParameters.DesiredKeys + " " + fitnessParameters.DesiredRooms);
            Debug.Log("Quest Line to Evolve: " + _questLine);
            Debug.Log("Quest Line to Evolve: " + _questLine.graph);
            Debug.Log("Quest Line to Evolve: " + _questLine.EnemySos.Count);

            // Start the generation process
            Thread t = new Thread(Evolve);
            t.Start();
            StartCoroutine(PrintAndSaveDungeonWhenFinished(t));
        }

        private IEnumerator PrintAndSaveDungeonWhenFinished(Thread t)
        {
            // Wait until the dungeons were generated
            while (t.IsAlive)
                yield return new WaitForSeconds(0.1f);
            Debug.Log("Finished evolving, save dungeon SOs");
            // Write all the generated dungeons in ScriptableObjects
            var solution = generator.Solution;
            for (var e = 0; e < solution.dimension.exp; e++)
            {
                for (var l = 0; l < solution.dimension.len; l++)
                {
                    var individual = solution.map[e, l];
                    if (individual != null)
                    {
                        Interface.PrintNumericalGridWithConnections(individual, _questLine);
                    }
                }
            }
            hasFinished = true;
        }

        public void Evolve()
        {
            hasFinished = false;
            generator = new LevelGenerator(parameters);
            generator.Evolve();
        }
    }
}