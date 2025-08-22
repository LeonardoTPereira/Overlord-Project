using MyBox;
using System;
using ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;
using Overlord.GenerationController.Facade;

namespace Game.EnemyGenerator
{
    public class EnemyGeneratorManager : MonoBehaviour
    {
        [SerializeField] private SearchSpaceConfig _searchSpaceConfig;

        public bool ActivateManualDifficulty;
        [ConditionalField(nameof(ActivateManualDifficulty))] public DifficultyLevels difficulties;
        private DifficultyLevels _difficulty;

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

        private float GetDifficultyFactor()
        {
            switch (_difficulty)
            {
                case DifficultyLevels.VeryEasy:
                    return EnemyUtil.veryEasyDifficultyFactor;
                case DifficultyLevels.Easy:
                    return EnemyUtil.easyDifficultyFactor;
                case DifficultyLevels.Medium:
                    return EnemyUtil.mediumDifficultyFactor;
                case DifficultyLevels.Hard:
                    return EnemyUtil.hardDifficultyFactor;
                case DifficultyLevels.VeryHard:
                    return EnemyUtil.veryHardDifficultyFactor;
                default:
                    Debug.LogWarning("Difficulty not set, defaulting to Medium.");
                    return EnemyUtil.mediumDifficultyFactor;
            }
        }

        public List<EnemySO> GetEnemyList(DifficultyLevels difficultyLevels)
        {
            SetGeneticAlgorithmSettings(difficultyLevels);
            EvolveEnemies();
            EnemySOFactory enemyFactory = new EnemySOFactory(_searchSpaceConfig.MovementSet, _searchSpaceConfig.WeaponSet);
            return enemyFactory.GetEnemiesSOFromSolution(_generator.Solution.ToList());
        }
        
        private void EvolveEnemies()
        {
            _generator = new EnemyGenerator(_geneticSettings, _searchSpaceConfig);
            _generator.Evolve();
        }

        private void SetGeneticAlgorithmSettings(DifficultyLevels difficultyLevels)
        {
            _difficulty = difficultyLevels;
            SetNumberOfMovementsAndWeapons();
            //TODO Mudar depois para tipo genérico, ou criar uma classe EnemyGeneratorManager para cada tipo de jogo
            _geneticSettings.difficulty = GetDifficultyFactor();
        }
        
        private void SetNumberOfMovementsAndWeapons()
        {
            _geneticSettings.numberOfMovements = _searchSpaceConfig.MovementSet.GetEnemyMovementCount();
            _geneticSettings.numberOfWeapons = _searchSpaceConfig.WeaponSet.GetEnemyWeaponCount();
        }
    }
}