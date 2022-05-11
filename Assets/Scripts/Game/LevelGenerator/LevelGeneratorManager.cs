using System.Collections;
using System.Threading;
using Game.Events;
using Game.GameManager;
using Game.LevelGenerator.EvolutionaryAlgorithm;
using Game.NarrativeGenerator.Quests;
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
        private FitnessPlot fitnessPlot;

        /**
         * The constructor of the "Main" behind the EA
         */
        public void Awake()
        {
            hasFinished = false;
        }

        private void Start()
        {
            fitnessPlot = GetComponent<FitnessPlot>();
        }

        public void OnEnable()
        {
            LevelGeneratorController.CreateEaDungeonEventHandler += EvolveDungeonPopulation;
        }

        public void OnDisable()
        {
            LevelGeneratorController.CreateEaDungeonEventHandler -= EvolveDungeonPopulation;
        }

        // The "Main" behind the Dungeon Generator
        public void EvolveDungeonPopulation(object sender, CreateEADungeonEventArgs eventArgs)
        {
            parameters = eventArgs.Parameters;
            _questLine = eventArgs.QuestLineForDungeon;

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
            // Write all the generated dungeons in ScriptableObjects
            var solutions = generator.Solution.GetBestEliteForEachBiome();
            foreach (var individual in solutions)
            {
                Interface.PrintNumericalGridWithConnections(individual, _questLine);
            }
            hasFinished = true;
        }

        public void Evolve()
        {
            hasFinished = false;
            generator = new LevelGenerator(parameters, fitnessPlot);
            generator.Evolve();
        }
    }
}