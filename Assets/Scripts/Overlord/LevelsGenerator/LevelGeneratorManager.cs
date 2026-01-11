using MyBox;
using Overlord.GenerationController;
using Overlord.Events;
using Overlord.LevelGenerator.EvolutionaryAlgorithm;
using Overlord.LevelGenerator.LevelSOs;
using Overlord.NarrativeGenerator;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlord.LevelGenerator.Manager
{
    public class LevelGeneratorManager : MonoBehaviour
    {
        [field: Foldout("Desired Parameters", true)]
        [DisplayInspector]
        [field: SerializeField] protected FitnessDesiredValuesSO _fitnessDesiredValues; // Apenas funciona se não utilizar o NarrativeManager (valores obtidos da narrativa sobrescrevem-o)
        /// Level generator
        protected GeneticAlgorithmManager _generator;
        protected FitnessInput _fitnessInput;

        /// Attributes to communicate to Game Manager
        // Flags if the dungeon has been generated for Unity's Game Manager to handle things after
        protected FitnessPlot _fitnessPlot;

        [field: Foldout("EA Parameters", true)]
        [DisplayInspector]
        public DungeonGeneratorGeneticAlgorithmSettings GeneticAlgorithmSettings;

        private void Awake()
        {
            FitnessInput.DesiredValues = _fitnessDesiredValues;
            _fitnessPlot = GetComponent<FitnessPlot>();
        }

        public async Task<List<DungeonFileSo>> EvolveDungeonPopulation(CreateEaDungeonEventArgs eventArgs)
        {
            var parameters = eventArgs.Parameters;
            Debug.Log("Parameters: " + parameters);
            _fitnessInput = eventArgs.Fitness;
            // Start the generation process
            _generator = new ClassicEvolutionaryAlgorithm(parameters, eventArgs.TimesToExecuteEA,
                eventArgs.IsVisualizingDungeon, _fitnessInput, _fitnessPlot);
            await _generator.Evolve();
            return GetListOfGeneratedDungeons();
        }

        protected List<DungeonFileSo> GetListOfGeneratedDungeons()
        {
            List<Individual> solutions = new List<Individual>();
            // Write all the generated dungeons in ScriptableObjects   
            if (_generator is ClassicEvolutionaryAlgorithm)
            {
                solutions.Add(_generator.Solution.EliteList[0]);
            }
            else
            {
                solutions = _generator.Solution.GetBestEliteForEachBiome();
            }
            List<DungeonFileSo> generatedDungeons = new();
            var totalEnemies = _fitnessInput.DesiredEnemies;
            var totalItems = _fitnessInput.DesiredItems;
            var totalNpcs = _fitnessInput.DesiredNpcs;

            foreach (var individual in solutions)
            {
                var dungeon =
                    Interface.CreateDungeonSoFromIndividual(individual, totalEnemies, totalItems, totalNpcs);
                generatedDungeons.Add(dungeon);
            }
            Debug.LogWarning($"Needed Enemies: {totalEnemies}, Generated Enemies: {generatedDungeons[0].TotalEnemies}");

            GenerationStatus.EndedDungeonGeneration = true;
            return generatedDungeons;
        }
    }
}