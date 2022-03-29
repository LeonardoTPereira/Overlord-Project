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
            Debug.Log("Start creating enemies...");
            difficulty = difficultyLevels;
            float goal = GetDesiredDifficulty();
            Parameters prs = new Parameters(
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
            /*if (!AssetDatabase.IsValidFolder(filename))
            {
                Debug.Log("Creating new Folder");
                string guid = AssetDatabase.CreateFolder(foldername, subfoldername);
                filename = AssetDatabase.GUIDToAssetPath(guid) + "/";
            }*/
#endif
            var enemyList = new List<EnemySO>();
            var i = 0;
            foreach (Individual individual in generator.Solution.ToList())
            {
#if UNITY_EDITOR
                //AssetDatabase.DeleteAsset(filename + "Enemy" + i + ".asset");
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
                //AssetDatabase.CreateAsset(enemySo, filename + "Enemy" + i + ".asset");
#endif
                enemyList.Add(enemySo);

                i++;
            }
#if UNITY_EDITOR
            //AssetDatabase.Refresh();
#endif
            Debug.Log("The enemies were created!");

            return enemyList;
        }
    }
}