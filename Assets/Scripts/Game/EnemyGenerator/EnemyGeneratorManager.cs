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

        [field: SerializeField] public bool IsEnable { get; set; } = false;

        [SerializeField]
        private EnemyGeneratorGeneticAlgorithmSettings geneticSettings;

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
            if (IsEnable)
            {
                GetEnemyList(DifficultyLevels.Easy);
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
                    return EnemyUtil.mediumDifficulty;
            }
        }

        public List<EnemySO> GetEnemyList(DifficultyLevels difficultyLevels)
        {
            SetGeneticAlgorithmSettings(difficultyLevels);
            EvolveEnemies();
            return CreateSoBestEnemies();
        }
        
        private void EvolveEnemies()
        {
            _generator = new EnemyGenerator(geneticSettings);
            _generator.Evolve();
        }

        private List<EnemySO> CreateSoBestEnemies()
        {
            var enemyList = new List<EnemySO>();
            foreach (var individual in _generator.Solution.ToList())
            {
                var weaponIndex = Convert.ToInt32(individual.Weapon.Weapon);
                var movementIndex = Convert.ToInt32(individual.Enemy.Movement);
                //var behaviorIndex = 0; // Behaviors are not implemented yet

                EnemySO enemySo = ScriptableObject.CreateInstance<EnemySO>();
                
                enemySo.Init(
                    individual.Enemy.Health,
                    individual.Enemy.Strength,
                    individual.Enemy.MovementSpeed,
                    individual.Enemy.ActiveTime,
                    individual.Enemy.RestTime,
                    _weaponSet.Items[weaponIndex],
                    _movementSet.Items[movementIndex],
                    null,                                               // NOT IMPLEMENTED YET
                    individual.FitnessValue,
                    individual.Enemy.AttackSpeed,
                    individual.Weapon.ProjectileSpeed
                );
                enemyList.Add(enemySo);
            }
            return enemyList;
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