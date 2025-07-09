using MyBox;
using ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

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

        [SerializeField]
        private EnemyGeneratorGeneticAlgorithmSettings geneticAlgorithmSettings;

        private EnemyGenerator generator;

        private DifficultyLevels difficulty;
        
        public static EnemyGeneratorManager Instance { get; set; } = null;

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
                GetEnemyList(DifficultyLevels.Easy);
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

        public List<EnemySO> GetEnemyList(DifficultyLevels difficultyLevels)
        {
            difficulty = difficultyLevels;
            geneticAlgorithmSettings.difficulty = GetDesiredDifficulty();
            EvolveEnemies();
            return CreateSoBestEnemies();
        }
        
        private void EvolveEnemies()
        {
            generator = new EnemyGenerator(geneticAlgorithmSettings);
            generator.Evolve();
        }

        private List<EnemySO> CreateSoBestEnemies()
        {
            var enemyList = new List<EnemySO>();
            foreach (var individual in generator.Solution.ToList())
            {
                var weaponIndex = (int)individual.Weapon.Weapon;
                var movementIndex = (int)individual.Enemy.Movement;
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