using MyBox;
using Overlord.GenerationController.Facade;
using ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Overlord.RulesGenerator.EnemyGeneration
{
    public class EnemyGeneratorManager : MonoBehaviour
    {
        [DisplayInspector]
        public SearchSpaceConfig _searchSpaceConfig;

        public bool ActivateManualDifficulty;
        [ConditionalField(nameof(ActivateManualDifficulty))] public DifficultyLevels difficulties;

        [SerializeField] private EnemyGeneratorGeneticAlgorithmSettings _geneticSettings;

        private EnemyGenerator _generator;
        private RulesGeneratorFacade _rulesFacade;
        protected IEnemyFitness _fitnessFunction;

        public static EnemyGeneratorManager Instance { get; private set; } = null;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void Start()
        {
            _rulesFacade = RulesGeneratorFacade.Instance;
            _rulesFacade.SetEnemyMovementType(_searchSpaceConfig.MovementSet);
            if (ActivateManualDifficulty)
            {
                GetEnemies(difficulties);
            }
        }

        public List<Individual> GetEnemies(DifficultyLevels difficultyLevels)
        {
            SetGeneticAlgorithmSettings(difficultyLevels);
            EvolveEnemies();
            return _generator.Solution.ToList();
        }

        public virtual List<EnemySO> GetEnemySOList(DifficultyLevels difficultyLevels)
        {
            List<Individual> enemies = GetEnemies(difficultyLevels);
            EnemySOFactory enemyFactory = new EnemySOFactory(_searchSpaceConfig.MovementSet, _searchSpaceConfig.WeaponSet);
            return enemyFactory.GetEnemiesSOFromSolution(enemies);
        }

        public virtual void SetGeneticAlgorithmSettings(DifficultyLevels difficultyLevels)
        {
            _geneticSettings.numberOfMovements = _searchSpaceConfig.MovementSet.GetEnemyMovementCount();
            _geneticSettings.numberOfWeapons = _searchSpaceConfig.WeaponSet.GetEnemyWeaponCount();
            _geneticSettings.difficulty = EnemyDifficultyFactor.GetDifficultyFactor(difficultyLevels);
            _fitnessFunction = new GenericEnemyFitness();
        }

        private void EvolveEnemies()
        {
            _generator = new EnemyGenerator(_geneticSettings, _searchSpaceConfig, _fitnessFunction);
            _generator.Evolve();
        }
    }
}
