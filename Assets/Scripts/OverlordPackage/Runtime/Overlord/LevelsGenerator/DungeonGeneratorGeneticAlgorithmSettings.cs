using MyBox;
using Overlord.UI;
using UnityEngine;

namespace Overlord.LevelGenerator.EvolutionaryAlgorithm
{
    [CreateAssetMenu(fileName = "GeneticAlgorithmSettings", menuName = "Overlord-Project/Levels-Generator/GeneticAlgorithmSettings")]
    public class DungeonGeneratorGeneticAlgorithmSettings: ScriptableObject
    {
        [field: SerializeField] public int TotalRunsOfEA { get; set; }
        [field: SerializeField, Range(1, 240)] public int Time { get; set; }
        [field: SerializeField, Range(1, 100)] public int Population { get; set; }
        [field: SerializeField, Range(0, 100)] public int Mutation { get; set; }
        [field: SerializeField, Range(1, 10)] public int Competitors { get; set; }
        [field: SerializeField, Range(1, 200)] public int MinimumElite { get; set; }
        [field: SerializeField, Range(0.0f, 10.0f)] public float AcceptableFitness { get; set; }
    }
}