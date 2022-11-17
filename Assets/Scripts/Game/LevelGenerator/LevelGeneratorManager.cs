using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Events;
using Game.ExperimentControllers;
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
        private FitnessInput _fitnessInput;

        /// Attributes to communicate to Game Manager
        // Flags if the dungeon has been generated for Unity's Game Manager to handle things after
        private FitnessPlot _fitnessPlot;

        private void Start()
        {
            _fitnessPlot = GetComponent<FitnessPlot>();
        }

        private void OnEnable()
        {
            DungeonMapEliteVisualizer.ContinueGenerationEventHandler += ContinueGenerationEvent;
        }

        private void ContinueGenerationEvent(object sender, EventArgs e)
        {
            _generator.waitGeneration = false;
        }

        private void OnDisable()
        {
            DungeonMapEliteVisualizer.ContinueGenerationEventHandler -= ContinueGenerationEvent;
        }

        // The "Main" behind the Dungeon Generator
        public async Task<List<DungeonFileSo>> EvolveDungeonPopulation(CreateEaDungeonEventArgs eventArgs)
        {
            var parameters = eventArgs.Parameters;
            Debug.Log("Parameters: "+parameters);
            _fitnessInput = eventArgs.Fitness;
            // Start the generation process
            _generator = new ClassicEvolutionaryAlgorithm(parameters, eventArgs.TimesToExecuteEA, 
                eventArgs.IsVisualizingDungeon ,_fitnessInput,_fitnessPlot);
            await _generator.Evolve();
            return GetListOfGeneratedDungeons();
        }

        private List<DungeonFileSo> GetListOfGeneratedDungeons()
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
            List<DungeonFileSo> generatedDungeons = new ();
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

            return generatedDungeons;
        }
    }
}