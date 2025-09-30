using System;
using System.Collections.Generic;

namespace Game.EnemyGenerator
{
    /// <summary>
    /// Holds the most relevant data of the evolutionary process.
    /// </summary>
    [Serializable]
    public class GeneticAlgorithmData
    {
        public EnemyGeneratorGeneticAlgorithmSettings geneticAlgorithmSettings;
        public double duration;
        public List<Individual> initialPopulation;
        public List<Individual> intermediatePopulation;
        public List<Individual> finalPopulation;
    }
}