using MyBox;
using ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;
using Overlord.GenerationController.Facade;

namespace Game.EnemyGenerator
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
                GetEnemyList(difficulties);
            }
        }

        public List<EnemySO> GetEnemyList(DifficultyLevels difficultyLevels)
        {
            SetGeneticAlgorithmSettings(difficultyLevels);
            EvolveEnemies();
            EnemySOFactory enemyFactory = new EnemySOFactory(_searchSpaceConfig.MovementSet, _searchSpaceConfig.WeaponSet);
            return enemyFactory.GetEnemiesSOFromSolution(_generator.Solution.ToList());
        }

        private void SetGeneticAlgorithmSettings(DifficultyLevels difficultyLevels)
        {
            _geneticSettings.numberOfMovements = _searchSpaceConfig.MovementSet.GetEnemyMovementCount();
            _geneticSettings.numberOfWeapons = _searchSpaceConfig.WeaponSet.GetEnemyWeaponCount();
            _geneticSettings.difficulty = EnemyDifficultyFactor.GetDifficultyFactor(difficultyLevels);
        }
        
        private void EvolveEnemies()
        {
            _generator = new EnemyGenerator(_geneticSettings, _searchSpaceConfig);
            _generator.Evolve();
        }
    }
}
