using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using MyBox;

namespace Game.EnemyGenerator
{
    public class EnemyGeneratorManager : MonoBehaviour
    {
#if UNITY_EDITOR
        [field: Foldout("Scriptable Objects")]
        [field: Header("Enemy Components")]
#endif
        [field: SerializeField] public EnemyComponentsSO EnemyComponents { get; set; }

        [field: SerializeField] public bool IsEnable { get; set; } = false;

        /// Evolutionary parameters
        [SerializeField] private int maxGenerations = 500;
        [SerializeField] private int initialPopulationSize = 35;
        [SerializeField] private int intermediatePopulationSize = 100;
        [SerializeField] private int mutationRate = 20;
        [SerializeField] private int geneMutationRate = 30;
        [SerializeField] private int numberOfCompetitors = 2;
        [SerializeField] private int numberOfDesiredElitesPerEnemy = 3;
        [SerializeField] private float minimumAcceptableFitnessPerEnemy = 0.5f;

        /// Singleton
        public static EnemyGeneratorManager Instance { get; set; } = null;

        private EnemyGenerator generator;

        private DifficultyLevels difficulty;

        private void Awake()
        {
            //Singleton
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void Start()
        {
            if (IsEnable)
            {
                EvolveEnemies(DifficultyLevels.Easy);
            }
        }

        private float GetDesiredDifficulty()
        {
            switch (difficulty)
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

        public List<EnemySO> EvolveEnemies(DifficultyLevels difficultyLevels)
        {
            difficulty = difficultyLevels;
            var goal = GetDesiredDifficulty();
            var prs = new Parameters(
                maxGenerations, // Number of generations
                initialPopulationSize, // Initial population size
                intermediatePopulationSize, // Intermediate population size
                mutationRate, // Mutation chance
                geneMutationRate, // Mutation chance of a single gene
                numberOfCompetitors, // Number of tournament competitors
                numberOfDesiredElitesPerEnemy,
                minimumAcceptableFitnessPerEnemy,
                goal // Aimed difficulty of enemies
            );
            generator = new EnemyGenerator(prs);
            generator.Evolve();
            return CreateSoBestEnemies();
        }

        private List<EnemySO> CreateSoBestEnemies()
        {
            var enemyList = new List<EnemySO>();
            foreach (var individual in generator.Solution.ToList())
            {
                var weaponIndex = (int) individual.Weapon.Weapon;
                var movementIndex = (int) individual.Enemy.Movement;
                var behaviorIndex = 0; // Behaviors are not implemented yet

                EnemySO enemySo = ScriptableObject.CreateInstance<EnemySO>();
                enemySo.Init(
                    individual.Enemy.Health,
                    individual.Enemy.Strength,
                    individual.Enemy.MovementSpeed,
                    individual.Enemy.ActiveTime,
                    individual.Enemy.RestTime,
                    EnemyComponents.weaponSet.Items[weaponIndex],
                    EnemyComponents.movementSet.Items[movementIndex],
                    EnemyComponents.behaviorSet.Items[behaviorIndex],
                    individual.FitnessValue,
                    individual.Enemy.AttackSpeed,
                    individual.Weapon.ProjectileSpeed
                );
                enemyList.Add(enemySo);
            }
            return enemyList;
        }
    }
}