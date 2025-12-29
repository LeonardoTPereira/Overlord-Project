using MyBox;
using ScriptableObjects;
using System.Collections.Generic;
using Overlord.RulesGenerator.EnemyGeneration;

namespace Topdown.Overlord.RulesGenerator.EnemyGeneration
{
    public class TopdownEnemyGeneratorManager : EnemyGeneratorManager
    {
        public static new TopdownEnemyGeneratorManager Instance { get; private set; } = null;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public override void SetGeneticAlgorithmSettings(DifficultyLevels difficultyLevels)
        {
            base.SetGeneticAlgorithmSettings(difficultyLevels);
            _fitnessFunction = new TopdownGame.Overlord.Inheritance.RulesGenerator.TopdownFitness();
        }
    }
}
