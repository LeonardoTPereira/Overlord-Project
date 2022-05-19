using System.Collections;
using System.Threading.Tasks;
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
        private QuestLine _questLine;
        private FitnessPlot fitnessPlot;

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
        public async Task EvolveDungeonPopulation(object sender, CreateEADungeonEventArgs eventArgs)
        {
            parameters = eventArgs.Parameters;
            _questLine = eventArgs.QuestLineForDungeon;
            Debug.Log("Start Evolving Dungeons");
            // Start the generation process
            generator = new LevelGenerator(parameters, fitnessPlot);
            await generator.Evolve();
            PrintAndSaveDungeonWhenFinished();
        }

        private void PrintAndSaveDungeonWhenFinished()
        {

            // Write all the generated dungeons in ScriptableObjects
            Debug.Log("Finished Creating Dungeons");
            var solutions = generator.Solution.GetBestEliteForEachBiome();
            foreach (var individual in solutions)
            {
                Interface.PrintNumericalGridWithConnections(individual, _questLine);
            }
        }
    }
}