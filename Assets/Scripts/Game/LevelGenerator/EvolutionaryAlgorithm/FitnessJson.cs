using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    public class FitnessJson
    {
        private const string filePath = @"E:\Doutorado\Resultados\NewFitnessResults\Results.json";
        public struct FitnessData
        {
            public int leniencyIndex;
            public int explorationIndex;
            public int generation;
            public float distance;
            public float usage;
            public float sparsity;
            public float enemyStandardDeviation;
            public float result;

            public FitnessData(int leniencyIndex, int explorationIndex, int generation, 
                float distance, float usage, float sparsity, float enemyStandardDeviation, float result)
            {
                this.leniencyIndex = leniencyIndex;
                this.explorationIndex = explorationIndex;
                this.generation = generation;
                this.distance = distance;
                this.usage = usage;
                this.sparsity = sparsity;
                this.enemyStandardDeviation = enemyStandardDeviation;
                this.result = result;
            }
        }

        private readonly List<FitnessData> data;

        public FitnessJson()
        {
            data = new List<FitnessData>();
        }

        public void AddFitness(Individual individual, int generation, int explorationIndex, int leniencyIndex)
        {
            data.Add(new FitnessData(leniencyIndex, explorationIndex, generation, 
                individual.Fitness.Distance, individual.Fitness.Usage, individual.Fitness.EnemySparsity, 
                individual.Fitness.EnemyStandardDeviation, individual.Fitness.Result));
        }

        public void SaveJson()
        {
            var serializer = new JsonSerializer();
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }
            using (var sw = File.AppendText(filePath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, data);
            }
        }
    }
}