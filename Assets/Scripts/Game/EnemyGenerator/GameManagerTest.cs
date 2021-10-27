using System.IO;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace EnemyGenerator
{
    public class GameManagerTest : MonoBehaviour
    {
        public enum DifficultyEnum
        {
            VeryEasy,
            Easy,
            Medium,
            Hard,
            VeryHard
        };

        private static readonly int MAX_GENERATIONS = 300;
        private static readonly int INITIAL_POPULATION_SIZE = 35;
        private static readonly int INTERMEDIATE_POPULATION_SIZE = 100;
        private static readonly int MUATION_RATE = 20;
        private static readonly int GENE_MUTATION_RATE = 40;
        private static readonly int NUMBER_OF_COMPETITORS = 3;

        //singleton
        public static GameManagerTest instance = null;

        private EnemyGenerator generator;

        public DifficultyEnum difficulty = DifficultyEnum.Easy;

        void Awake()
        {
            //Singleton
            if (instance == null)
            {
                instance = this;
                AwakeInit();
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void AwakeInit()
        {
            switch (difficulty)
            {
                case DifficultyEnum.VeryEasy:
                    EnemyUtil.desiredFitness = EnemyUtil.veryEasyFitness;
                    break;
                case DifficultyEnum.Easy:
                    EnemyUtil.desiredFitness = EnemyUtil.easyFitness;
                    break;
                case DifficultyEnum.Medium:
                    EnemyUtil.desiredFitness = EnemyUtil.mediumFitness;
                    break;
                case DifficultyEnum.Hard:
                    EnemyUtil.desiredFitness = EnemyUtil.hardFitness;
                    break;
                case DifficultyEnum.VeryHard:
                    EnemyUtil.desiredFitness = EnemyUtil.veryHardFitness;
                    break;
                default:
                    EnemyUtil.desiredFitness = EnemyUtil.mediumFitness;
                    break;
            }
        }

        void Start()
        {
            Debug.Log("Evolve");
            // Evolve
            Parameters prs = new Parameters(
                (new System.Random()).Next(), // Random seed
                MAX_GENERATIONS, // Number of generations
                INITIAL_POPULATION_SIZE, // Initial population size
                INTERMEDIATE_POPULATION_SIZE, // Intermediate population size
                MUATION_RATE, // Mutation chance
                GENE_MUTATION_RATE, // Mutation chance of a single gene
                NUMBER_OF_COMPETITORS, // Number of tournament competitors
                EnemyUtil.desiredFitness // Aimed difficulty of enemies
            );
            generator = new EnemyGenerator(prs);
            generator.Evolve();
            CreateSOBestEnemies();
        }

#if UNITY_EDITOR
        public void CreateSOBestEnemies()
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
            if (!AssetDatabase.IsValidFolder(filename))
            {
                Debug.Log("Creating new Folder");
                AssetDatabase.CreateFolder(foldername, subfoldername);
            }

            int i = 0;
            foreach (Individual individual in generator.Solution.ToList())
            {
                AssetDatabase.DeleteAsset(filename + "Enemy" + i + ".asset");

                EnemySO enemySO = ScriptableObject.CreateInstance<EnemySO>();
                enemySO.Init(
                    individual.enemy.health,
                    individual.enemy.strength,
                    individual.enemy.movementSpeed,
                    individual.enemy.activeTime,
                    individual.enemy.restTime,
                    (int) individual.weapon.weaponType,
                    (int) individual.enemy.movementType,
                    0, // Behaviors are not implemented yet
                    individual.fitness,
                    individual.enemy.attackSpeed,
                    individual.weapon.projectileSpeed
                );

                AssetDatabase.CreateAsset(enemySO, filename + "Enemy" + i + ".asset");

                i++;
            }
        }
#endif
    }
}