using System;
using System.Collections.Generic;
using Game.Maestro;
using Util;

namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    /// This struct represents a MAP-Elites population.
    ///
    /// The MAP-Elites population is an N-dimensional array of individuals,
    /// where each matrix's ax corresponds to a different feature.
    ///
    /// This particular population is mapped into the level's coefficient of
    /// exploration and leniency. Thus, each Elite (or matrix cell) corresponds
    /// to a combination of a different degree of exploration and leniency.
    public class Population
    {
        /// The MAP-Elites map (a matrix of individuals).
        public int LeniencyEliteCount { get; }
        public int ExplorationEliteCount { get; }
        public int TotalElites { get; private set; }
        public List<Individual> EliteList { get; set; }
        public BiomeMap BiomeMap { get; set; }
        public MapElites MapElites { get; set; }

        /// Population constructor.
        public Population(int explorationSize, int leniencySize)
        {
            LeniencyEliteCount = leniencySize;
            ExplorationEliteCount = explorationSize;
            MapElites = new MapElites(ExplorationEliteCount, LeniencyEliteCount);
            EliteList = new List<Individual>();
            TotalElites = 0;
            BiomeMap = new BiomeMap();
        }

        /// Add an individual in the MAP-Elites population.
        ///
        /// First, we identify which Elite the individual is classified in.
        /// Then, if the corresponding Elite is empty, the individual is placed
        /// there. Otherwise, we compare the both old and new individuals, and
        /// the best individual is placed in the corresponding Elite.
        public void PlaceIndividual(Individual individual) {
            int explorationIndex = SearchSpace.GetCoefficientOfExplorationIndex(individual.exploration);
            int leniencyIndex = SearchSpace.GetLeniencyIndex(individual.leniency);
            // Check if the level is within the search space
            if (explorationIndex < 0 || explorationIndex >= ExplorationEliteCount || leniencyIndex < 0 || leniencyIndex >= LeniencyEliteCount) {
                return;
            }
            var currentElite = MapElites.GetElite(explorationIndex, leniencyIndex);
            if (currentElite == null)
            {
                TotalElites++;
                EliteList.Add(individual);
            }
            else
            {
                if (!Fitness.IsBest(individual, currentElite)) return;
                EliteList[EliteList.IndexOf(currentElite)] = individual;
            }
            MapElites.SetElite(explorationIndex, leniencyIndex, individual);
        }

        /// Print all the individuals of the MAP-Elites population.
        public void Debug()
        {
            for (int exploration = 0; exploration < ExplorationEliteCount; exploration++)
            {
                for (int leniency = 0; leniency < LeniencyEliteCount; leniency++)
                {
                    string log = "Elite ";
                    log += "CE" + SearchSpace.ExplorationRanges[exploration] + "-";
                    log += "LE" + SearchSpace.LeniencyRanges[leniency];
                    Console.WriteLine(log);
                    if (MapElites.GetElite(exploration, leniency) is null)
                    {
                        Console.WriteLine(LevelDebug.INDENT + "Empty");
                    }
                    else
                    {
                        MapElites.GetElite(exploration, leniency).Debug();
                    }
                    Console.WriteLine();
                }
            }
        }

        public int IndividualsBetterThan(float acceptableFitness)
        {
            var betterCounter = 0;
            for (var exploration = 0; exploration < ExplorationEliteCount; exploration++)
            {
                for (var leniency = 0; leniency < LeniencyEliteCount; leniency++)
                {
                    if ((MapElites.GetElite(exploration, leniency)?.Fitness.result ?? float.MaxValue) < acceptableFitness)
                    {
                        betterCounter++;
                    }
                }
            }
            return betterCounter;
        }

        public void UpdateBiomes()
        {
            BiomeMap.UpdateBiomes(MapElites);
        }

        public int GetAmountOfBiomesWithElites()
        {
            return BiomeMap.BiomesWithElites;
        }

        public int GetAmountOfBiomesWithElitesBetterThan(float fitness)
        {
            return BiomeMap.BiomesWithElitesBetterThan(fitness);
        }

        public List<Individual> GetBestEliteForEachBiome()
        {
            return BiomeMap.GetBestEliteForEachBiome();
        }

        public Individual GetRandomIndividualFromList()
        {
            return RandomSingleton.GetInstance().RandomElementFromList(EliteList);
        }
    }
}