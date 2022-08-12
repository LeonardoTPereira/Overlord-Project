using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Events;
using Game.LevelGenerator.EvolutionaryAlgorithm;
using Game.LevelGenerator.LevelSOs;
using Game.NarrativeGenerator.Quests;
using UnityEngine;

namespace Game.LevelGenerator
{
    public class LevelGeneratorManager : MonoBehaviour
    {
        /// Level generator
        private LevelGenerator _generator;
        private Parameters _parameters;

        /// Attributes to communicate to Game Manager
        // Flags if the dungeon has been gerated for Unity's Game Manager to handle things after
        private FitnessPlot _fitnessPlot;

        private void Start()
        {
            _fitnessPlot = GetComponent<FitnessPlot>();
        }

        // The "Main" behind the Dungeon Generator
        public async Task<List<DungeonFileSo>> EvolveDungeonPopulation(CreateEaDungeonEventArgs eventArgs)
        {
            _parameters = eventArgs.Parameters;
            Debug.Log("Start Evolving Dungeons");
            // Start the generation process
            _generator = new LevelGenerator(_parameters, _fitnessPlot);
            await _generator.Evolve();
            return GetListOfGeneratedDungeons();
        }

        private List<DungeonFileSo> GetListOfGeneratedDungeons()
        {

            // Write all the generated dungeons in ScriptableObjects
            Debug.Log("Finished Creating Dungeons");
            var solutions = _generator.Solution.GetBestEliteForEachBiome();
            List<DungeonFileSo> generatedDungeons = new ();
            var totalEnemies = _parameters.FitnessParameters.DesiredEnemies;
            var totalItems = _parameters.FitnessParameters.DesiredItems;
            var totalNpcs = _parameters.FitnessParameters.DesiredNpcs;
            foreach (var individual in solutions)
            {
                var dungeon =
                    Interface.CreateDungeonSoFromIndividual(individual, totalEnemies, totalItems, totalNpcs);
                generatedDungeons.Add(dungeon);
            }

            return generatedDungeons;
        }
    }
}