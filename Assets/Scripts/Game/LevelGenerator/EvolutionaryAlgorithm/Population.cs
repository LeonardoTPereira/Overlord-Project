using System.Collections.Generic;
using Game.Events;
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
        public int LinearityEliteCount { get; }
        public int TotalElites { get; private set; }
        public List<Individual> EliteList { get; set; }
        public BiomeMap BiomeMap { get; set; }
        public MapElites MapElites { get; set; }
        protected readonly FitnessJson FitnessJson;
        protected readonly FitnessPlot Plotter;

        /// Population constructor.
        public Population(int explorationSize, int leniencySize, int linearitySize, FitnessPlot plotter = null)
        {
            LeniencyEliteCount = leniencySize;
            ExplorationEliteCount = explorationSize;
            LinearityEliteCount = linearitySize;
            MapElites = new MapElites(ExplorationEliteCount, LeniencyEliteCount, LinearityEliteCount);
            EliteList = new List<Individual>();
            TotalElites = 0;
            BiomeMap = new BiomeMap();
            FitnessJson = new FitnessJson();
            Plotter = plotter;
        }

        //TODO Check bug where, apparently, no individual can be created in this loop
        //Maybe they always go out of elite range, somehow (shouldn't!)
        public void PlaceIndividual(Individual individual) {
            var explorationIndex = SearchSpace.GetCoefficientOfExplorationIndex(individual.exploration);
            var leniencyIndex = SearchSpace.GetLeniencyIndex(individual.leniency);
            var linearityIndex = SearchSpace.GetLinearityIndex(individual.linearity);
            
            if (!MapElites.IsCellInMapRange(explorationIndex, leniencyIndex, linearityIndex)) return;
            
            var currentElite = MapElites.GetElite(explorationIndex, leniencyIndex, linearityIndex);
            if (currentElite == null)
            {
                TotalElites++;
                EliteList.Add(individual);
            }
            else
            {
                if (currentElite.IsBetterThan(individual)) return;
                EliteList[EliteList.IndexOf(currentElite)] = individual;
            }
            MapElites.SetElite(explorationIndex, leniencyIndex, linearityIndex, individual);
        }

        /// Print all the individuals of the MAP-Elites population.
        public void Debug()
        {
            for (int exploration = 0; exploration < ExplorationEliteCount; exploration++)
            {
                for (int leniency = 0; leniency < LeniencyEliteCount; leniency++)
                {
                    for (int linearity = 0; linearity < LinearityEliteCount; linearity++)
                    {
                        string log = "Elite ";
                        log += "CE" + SearchSpace.ExplorationRanges[exploration] + "-";
                        log += "LE" + SearchSpace.LeniencyRanges[leniency] + "-";
                        log += "LC" + SearchSpace.LinearityRanges[linearity] + "-";
                        UnityEngine.Debug.Log(log);
                        if (MapElites.GetElite(exploration, leniency, linearity) is null)
                        {
                            UnityEngine.Debug.Log(LevelDebug.INDENT + "Empty");
                        }
                        else
                        {
                            MapElites.GetElite(exploration, leniency, linearity).Debug();
                        }
                    }
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
                    for (var linearity = 0; linearity < LinearityEliteCount; linearity++)
                    {
                        if ((MapElites.GetElite(exploration, leniency, linearity)?.Fitness.Result ?? float.MaxValue) < acceptableFitness)
                        {
                            betterCounter++;
                        }
                    }
                }
            }
            return betterCounter;
        }

        public void UpdateBiomes(int generation)
        {
            foreach (var elite in EliteList)
            {
                FitnessJson?.AddFitness(elite, generation, SearchSpace.GetCoefficientOfExplorationIndex(elite.exploration), SearchSpace.GetLeniencyIndex(elite.leniency), SearchSpace.GetLinearityIndex(elite.linearity));
                /*else
                {
                    UnityEngine.Debug.LogWarning("No Json Component Linked. Will Not Save Fitness Data to Json");
                }*/

                if (Plotter != null)
                {
                    Plotter.UpdateFitnessPlotData(elite, generation);
                }
                /*else
                {
                    UnityEngine.Debug.LogWarning("No Plot Component Linked. Will Not Plot Fitness Data to Animation");
                }*/
            }
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

        public void SaveJson()
        {
            if (FitnessJson != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(FitnessJson.SaveJson);
            }
            else
            {
                UnityEngine.Debug.LogWarning("No Json Component Linked. Will Not Save Fitness Data to Json");
            }

            if (Plotter != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(Plotter.AddAnimationCurves);
            }
            else
            {
                UnityEngine.Debug.LogWarning("No Plot Component Linked. Will Not Plot Fitness Data to Animation");
            }
        }
    }
}