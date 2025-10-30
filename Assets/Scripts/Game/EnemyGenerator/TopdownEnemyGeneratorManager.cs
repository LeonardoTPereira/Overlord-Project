using MyBox;
using ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;
using Overlord.GenerationController.Facade;
using System.Linq;

namespace Overlord.RulesGenerator.EnemyGeneration
{
    public class TopdownEnemyGeneratorManager : EnemyGeneratorManager
    {
        public static TopdownEnemyGeneratorManager Instance { get; private set; } = null;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public List<EnemySO> GetEnemySOList(DifficultyLevels difficultyLevels)
        {
            List<Individual> enemies = GetEnemies(difficultyLevels);
            EnemySOFactory enemyFactory = new EnemySOFactory(_searchSpaceConfig.MovementSet, _searchSpaceConfig.WeaponSet);
            return enemyFactory.GetEnemiesSOFromSolution(enemies);
        }

        public override void SetGeneticAlgorithmSettings(DifficultyLevels difficultyLevels)
        {
            base.SetGeneticAlgorithmSettings(difficultyLevels);
            _fitnessFunction = new TopdownGame.Overlord.Inheritance.RulesGenerator.TopdownFitness();
        }
    }
}
