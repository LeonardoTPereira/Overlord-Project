using System.Collections.Generic;
using System.IO;
using Game.Events;
using Game.NarrativeGenerator;
using ScriptableObjects;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Util;
using MyBox;

namespace Game.EnemyGenerator
{
    public class EnemyGeneratorManager : MonoBehaviour
    {
#if UNITY_EDITOR
        [field: Foldout("Scriptable Objects")]
        [field: Header("Enemy Components")]
#endif
        public EnemyComponentsSO EnemyComponents { get; }

        [field: SerializeField] public bool IsEnable { get; } = false;

        /// Evolutionary parameters
        private static readonly int MAX_GENERATIONS = 300;

        private static readonly int INITIAL_POPULATION_SIZE = 35;
        private static readonly int INTERMEDIATE_POPULATION_SIZE = 100;
        private static readonly int MUATION_RATE = 20;
        private static readonly int GENE_MUTATION_RATE = 40;
        private static readonly int NUMBER_OF_COMPETITORS = 3;

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
            Debug.Log("Start creating enemies...");
            difficulty = difficultyLevels;
            float goal = GetDesiredDifficulty();
            Parameters prs = new Parameters(
                MAX_GENERATIONS, // Number of generations
                INITIAL_POPULATION_SIZE, // Initial population size
                INTERMEDIATE_POPULATION_SIZE, // Intermediate population size
                MUATION_RATE, // Mutation chance
                GENE_MUTATION_RATE, // Mutation chance of a single gene
                NUMBER_OF_COMPETITORS, // Number of tournament competitors
                goal // Aimed difficulty of enemies
            );
            generator = new EnemyGenerator(prs);
            generator.Evolve();
            return CreateSOBestEnemies();
        }

        public List<EnemySO> CreateSOBestEnemies()
        {
            string foldername = "Assets/Resources/Enemies";
            string subfoldername;
            string filename;
            switch (difficulty)
            {
                case DifficultyLevels.VeryEasy:
                    subfoldername = "VeryEasy";
                    break;
                case DifficultyLevels.Easy:
                    subfoldername = "Easy";
                    break;
                case DifficultyLevels.Medium:
                    subfoldername = "Medium";
                    break;
                case DifficultyLevels.Hard:
                    subfoldername = "Hard";
                    break;
                case DifficultyLevels.VeryHard:
                    subfoldername = "VeryHard";
                    break;
                default:
                    subfoldername = "Unknown";
                    Debug.LogError("Difficulty to Create Enemies not Chosen");
                    break;
            }

            filename = foldername + "/" + subfoldername + "/";

#if UNITY_EDITOR
            if (!AssetDatabase.IsValidFolder(filename))
            {
                Debug.Log("Creating new Folder");
                string guid = AssetDatabase.CreateFolder(foldername, subfoldername);
                filename = AssetDatabase.GUIDToAssetPath(guid) + "/";
            }
#endif
            var enemyList = new List<EnemySO>();
            var i = 0;
            foreach (Individual individual in generator.Solution.ToList())
            {
#if UNITY_EDITOR
                AssetDatabase.DeleteAsset(filename + "Enemy" + i + ".asset");
#endif
                int weaponIndex = (int) individual.Weapon.Weapon;
                int movementIndex = (int) individual.Enemy.Movement;
                int behaviorIndex = 0; // Behaviors are not implemented yet

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
#if UNITY_EDITOR
                AssetDatabase.CreateAsset(enemySo, filename + "Enemy" + i + ".asset");
#endif
                enemyList.Add(enemySo);

                i++;
            }
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
            Debug.Log("The enemies were created!");

            return enemyList;
        }
    }
}