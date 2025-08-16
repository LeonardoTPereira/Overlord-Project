using MyBox;
using System;
using ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;
using Overlord.GenerationController.Facade;
using Overlord.RulesGenerator.EnemyGeneration;

namespace Game.EnemyGenerator
{
    public class EnemyGeneratorManager : MonoBehaviour
    {
        [field: Foldout("Enemy Components")]
        [SerializeField] private MovementTypeRuntimeSetSO _movementSet;
        [field: Foldout("Enemy Components")]
        [SerializeField] private WeaponTypeRuntimeSetSO _weaponSet;
        //[SerializeField] private BehaviorTypeRuntimeSetSO BehaviorSet;

        public bool ActivateManualDifficulty;
        [ConditionalField(nameof(ActivateManualDifficulty))] public DifficultyLevels difficulties;
        
        [SerializeField] private EnemyGeneratorGeneticAlgorithmSettings geneticSettings;

        private EnemyGenerator _generator;

        private DifficultyLevels _difficulty;

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
            _rulesFacade.SetEnemyMovementType(new TopdownMovementType());
            if (ActivateManualDifficulty)
            {
                GetEnemyList(difficulties);
            }
        }

        private float GetDesiredDifficulty()
        {
            switch (_difficulty)
            {
                case DifficultyLevels.VeryEasy:
                    return EnemyUtil.veryEasyDifficulty;
                case DifficultyLevels.Easy:
                    return EnemyUtil.easyDifficulty;
                case DifficultyLevels.Medium:
                    return EnemyUtil.mediumDifficulty;
                case DifficultyLevels.Hard:
                    return EnemyUtil.hardDifficulty;
                case DifficultyLevels.VeryHard:
                    return EnemyUtil.veryHardDifficulty;
                default:
                    Debug.LogWarning("Difficulty not set, defaulting to Medium.");
                    return EnemyUtil.mediumDifficulty;
            }
        }

        public List<EnemySO> GetEnemyList(DifficultyLevels difficultyLevels)
        {
            SetGeneticAlgorithmSettings(difficultyLevels);
            EvolveEnemies();
            EnemySOFactory enemyFactory = new EnemySOFactory(_movementSet, _weaponSet);
            return enemyFactory.GetEnemiesSOFromSolution(_generator.Solution.ToList());
        }
        
        private void EvolveEnemies()
        {
            _generator = new EnemyGenerator(geneticSettings);
            _generator.Evolve();
        }

        private void SetGeneticAlgorithmSettings(DifficultyLevels difficultyLevels)
        {
            _difficulty = difficultyLevels;
            SetNumberOfMovementsAndWeapons();
            //TODO Mudar depois para tipo genérico, ou criar uma classe EnemyGeneratorManager para cada tipo de jogo
            geneticSettings.movementType = new TopdownMovementType();
            geneticSettings.difficulty = GetDesiredDifficulty();
        }
        
        private void SetNumberOfMovementsAndWeapons()
        {
            geneticSettings.numberOfMovements = _movementSet.Items.Count;
            geneticSettings.numberOfWeapons = _weaponSet.Items.Count;
        }
    }
}