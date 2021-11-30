using System.Collections.Generic;
using System.IO;
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
        [Foldout("Scriptable Objects"), Header("Enemy Components")]
#endif
        public EnemyComponentsSO enemyComponents;
        [SerializeField]
        public bool isEnable = false;

        /// Evolutionary parameters
        private static readonly int MAX_GENERATIONS = 300;
        private static readonly int INITIAL_POPULATION_SIZE = 35;
        private static readonly int INTERMEDIATE_POPULATION_SIZE = 100;
        private static readonly int MUATION_RATE = 20;
        private static readonly int GENE_MUTATION_RATE = 40;
        private static readonly int NUMBER_OF_COMPETITORS = 3;

        /// Singleton
        public static EnemyGeneratorManager instance = null;

        private EnemyGenerator generator;

        private DifficultyEnum difficulty;

        public static event CreateEAEnemyEvent createEAEnemyEventHandler;

        void Awake()
        {
            //Singleton
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void Start()
        {
            if (isEnable)
            {
                EvolveEnemies(DifficultyEnum.Easy);
            }
        }
        
        private float GetDesiredDifficulty()
        {
            switch (difficulty)
            {
                case DifficultyEnum.VeryEasy:
                    return EnemyUtil.veryEasyDifficulty;
                case DifficultyEnum.Easy:
                    return EnemyUtil.easyDifficulty;
                case DifficultyEnum.Medium:
                    return EnemyUtil.mediumDifficulty;
                case DifficultyEnum.Hard:
                    return EnemyUtil.hardDifficulty;
                case DifficultyEnum.VeryHard:
                    return EnemyUtil.veryHardDifficulty;
                default:
                    return EnemyUtil.mediumDifficulty;
            }
        }

        public List<EnemySO> EvolveEnemies(DifficultyEnum difficultyEnum)
        {
            Debug.Log("Start creating enemies...");
            difficulty = difficultyEnum;
            float goal = GetDesiredDifficulty();
            Parameters prs = new Parameters(
                (new System.Random()).Next(), // Random seed
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
                case DifficultyEnum.VeryEasy:
                    subfoldername = "VeryEasy";
                    break;
                case DifficultyEnum.Easy:
                    subfoldername = "Easy";
                    break;
                case DifficultyEnum.Medium:
                    subfoldername = "Medium";
                    break;
                case DifficultyEnum.Hard:
                    subfoldername = "Hard";
                    break;
                case DifficultyEnum.VeryHard:
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
                int weaponIndex = (int) individual.weapon.weaponType;
                int movementIndex = (int) individual.enemy.movementType;
                int behaviorIndex = 0; // Behaviors are not implemented yet

                EnemySO enemySo = ScriptableObject.CreateInstance<EnemySO>();
                enemySo.Init(
                    individual.enemy.health,
                    individual.enemy.strength,
                    individual.enemy.movementSpeed,
                    individual.enemy.activeTime,
                    individual.enemy.restTime,
                    enemyComponents.weaponSet.Items[weaponIndex],
                    enemyComponents.movementSet.Items[movementIndex],
                    enemyComponents.behaviorSet.Items[behaviorIndex],
                    individual.fitness,
                    individual.enemy.attackSpeed,
                    individual.weapon.projectileSpeed
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