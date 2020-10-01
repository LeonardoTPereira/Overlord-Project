using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Rendering;
using EnemyGenerator;
using UnityEditor;
using System.IO;

public class GameManagerTest : MonoBehaviour
{
    public enum DifficultyEnum
    {
        easy,
        medium,
        hard,
        veryhard
    };

    //singleton
    public static GameManagerTest instance = null;
#if UNITY_EDITOR
    //Array of entities for the population of enemies
    public NativeArray<Entity> enemyPopulationArray;
    //Array of entities for the intermediate population of enemies
    public NativeArray<Entity> intermediateEnemyPopulationArray;

    public NativeArray<float> fitnessArray;
#endif
    public int generationCounter;

    public float startTime;

    public DifficultyEnum difficulty = DifficultyEnum.easy;

    public bool enemyGenerated, enemyPrinted, enemyReady, enemySorted;

#if UNITY_EDITOR
    public NativeArray<EnemyComponent> enemyPop;
    public NativeArray<EnemyComponent> bestEnemyPop;
    public NativeArray<WeaponComponent> weaponPop;
    public NativeArray<WeaponComponent> bestWeaponPop;
#endif
    public int bestIdx;

    public ProjectileTypeRuntimeSetSO projectileSet;
    public MovementTypeRuntimeSetSO movementSet;
    public BehaviorTypeRuntimeSetSO behaviorSet;
    public WeaponTypeRuntimeSetSO weaponSet;
#if UNITY_EDITOR
    public NativeArray<int> projectileMultipliers;
    public NativeArray<float> movementMultipliers;
    //TODO put them into the EA
    public NativeArray<float> behaviorMultipliers;
    public NativeArray<float> weaponMultipliers;
    public NativeArray<bool> weaponHasProjectile;
#endif

    public float timeToConverge, timeAfterSort;
    public float bestFitness, averageNBestFitness, averageFitness;
    public float stdDevNBestFitness, stdDevFitness;

    EntityArchetype enemyArchetype, intermediateEnemyArchetype;
    EntityManager entityManager;
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
        startTime = Time.realtimeSinceStartup;
#if UNITY_EDITOR
        //Instantiate "Population Size" individuals for both populations using a native array
        enemyPopulationArray = new NativeArray<Entity>(EnemyUtil.popSize, Allocator.Persistent);
        intermediateEnemyPopulationArray = new NativeArray<Entity>(EnemyUtil.popSize, Allocator.Persistent);
        fitnessArray = new NativeArray<float>(EnemyUtil.popSize, Allocator.Persistent);
        enemyPop = new NativeArray<EnemyComponent>(EnemyUtil.popSize, Allocator.Persistent);
        weaponPop = new NativeArray<WeaponComponent>(EnemyUtil.popSize, Allocator.Persistent);
        bestEnemyPop = new NativeArray<EnemyComponent>(EnemyUtil.nBestEnemies, Allocator.Persistent);
        bestWeaponPop = new NativeArray<WeaponComponent>(EnemyUtil.nBestEnemies, Allocator.Persistent);
#endif
        generationCounter = 0;

        enemyGenerated = false;
        enemyPrinted = false;
        enemyReady = false;
        enemySorted = false;
#if UNITY_EDITOR
        projectileMultipliers = new NativeArray<int>(projectileSet.Items.Count, Allocator.Persistent);
        movementMultipliers = new NativeArray<float>(movementSet.Items.Count, Allocator.Persistent);
        behaviorMultipliers = new NativeArray<float>(behaviorSet.Items.Count, Allocator.Persistent);
        weaponMultipliers = new NativeArray<float>(weaponSet.Items.Count, Allocator.Persistent);
        weaponHasProjectile = new NativeArray<bool>(weaponSet.Items.Count, Allocator.Persistent);
#endif
        switch (difficulty)
        {
            case DifficultyEnum.easy:
                EnemyUtil.desiredFitness = EnemyUtil.easyFitness;
                break;
            case DifficultyEnum.medium:
                EnemyUtil.desiredFitness = EnemyUtil.mediumFitness;
                break;
            case DifficultyEnum.hard:
                EnemyUtil.desiredFitness = EnemyUtil.hardFitness;
                break;
            case DifficultyEnum.veryhard:
                EnemyUtil.desiredFitness = EnemyUtil.veryHardFitness;
                break;
            default:
                EnemyUtil.desiredFitness = EnemyUtil.mediumFitness;
                break;
        }
    }

    private void Start()
    {
        StartInit();
    }

    public void StartInit()
    {
#if UNITY_EDITOR
        for (int i = (projectileSet.Items.Count - 1); i >= 0; i--)
            projectileMultipliers[i] = projectileSet.Items[i].multiplier;

        for (int i = (movementSet.Items.Count - 1); i >= 0; i--)
            movementMultipliers[i] = movementSet.Items[i].multiplier;

        for (int i = (behaviorSet.Items.Count - 1); i >= 0; i--)
            behaviorMultipliers[i] = behaviorSet.Items[i].multiplier;

        for (int i = (weaponSet.Items.Count - 1); i >= 0; i--)
            weaponMultipliers[i] = weaponSet.Items[i].multiplier;

        for (int i = (weaponSet.Items.Count - 1); i >= 0; i--)
            weaponHasProjectile[i] = weaponSet.Items[i].hasProjectile;

        //We must have an entity manager in our current world to create and handle the entities
        entityManager = World.Active.EntityManager;
#endif
        //An entity archetype is kind of a struct of entities
        //This one is for the population of enemies itself, having a "population" tag to differentiate it from the intermediate population
        //Also it has the enemy and its weapon component and, 
        //as we may use this entity for the game (or not, and just handle the gameplay with the monobehaviour stuff), it also has a translation
#if UNITY_EDITOR
        enemyArchetype = entityManager.CreateArchetype(
            typeof(Population),
            typeof(EnemyComponent),
            typeof(WeaponComponent)
        );

        //The entity archetype for the intermediate population. As the translation is not used in the EA evolution, it does not contain one
        intermediateEnemyArchetype = entityManager.CreateArchetype(
            typeof(IntermediatePopulation),
            typeof(EnemyComponent),
            typeof(WeaponComponent)
        );


#endif




        //Create the entities themselves in the memory
#if UNITY_EDITOR

        entityManager.CreateEntity(enemyArchetype, enemyPopulationArray);
        entityManager.CreateEntity(intermediateEnemyArchetype, intermediateEnemyPopulationArray);
        //Initialize the initial population of enemies with random values
        for (int i = 0; i < enemyPopulationArray.Length; i++)
        {
            Entity entity = enemyPopulationArray[i];

            entityManager.SetComponentData(entity,
                new WeaponComponent
                {
                    //projectile = (WeaponComponent.ProjectileEnum)UnityEngine.Random.Range(0, (int)WeaponComponent.ProjectileEnum.COUNT),
                    projectile = UnityEngine.Random.Range(0, projectileMultipliers.Length),
                    attackSpeed = UnityEngine.Random.Range(EnemyUtil.minAtkSpeed, EnemyUtil.maxAtkSpeed),
                    projectileSpeed = UnityEngine.Random.Range(EnemyUtil.minProjectileSpeed, EnemyUtil.maxProjectileSpeed)
                }
            );

            entityManager.SetComponentData(entity,
                new EnemyComponent
                {
                    health = UnityEngine.Random.Range(EnemyUtil.minHealth, EnemyUtil.maxHealth),
                    damage = UnityEngine.Random.Range(EnemyUtil.minDamage, EnemyUtil.maxDamage),
                    movementSpeed = UnityEngine.Random.Range(EnemyUtil.minMoveSpeed, EnemyUtil.maxMoveSpeed),
                    activeTime = UnityEngine.Random.Range(EnemyUtil.minActivetime, EnemyUtil.maxActiveTime),
                    restTime = UnityEngine.Random.Range(EnemyUtil.minResttime, EnemyUtil.maxRestTime),
                    weapon = UnityEngine.Random.Range(0, weaponMultipliers.Length),
                    movement = UnityEngine.Random.Range(0, movementMultipliers.Length),
                    behavior = UnityEngine.Random.Range(0, behaviorMultipliers.Length),
                    fitness = Mathf.Infinity
                }
            );
        }
#endif
        //GameManager.instance.createEnemy = true;
        //Kill the temporary arrays to free memory
        //enemyPopulationArray.Dispose();
        //intermediateEnemyPopulationArray.Dispose();
    }

    public void Update()
    {
        if (enemyReady & enemySorted)
        {
            timeAfterSort = Time.realtimeSinceStartup - startTime;
            if (!enemyPrinted)
            {
                
                /*Debug.Log("Fitness: " + enemyPop[bestIdx].fitness);
                Debug.Log("Health: " + enemyPop[bestIdx].health);
                Debug.Log("damage: " + enemyPop[bestIdx].damage);
                Debug.Log("activetime: " + enemyPop[bestIdx].activeTime);
                Debug.Log("movement: " + enemyPop[bestIdx].movement);
                Debug.Log("movementspeed: " + enemyPop[bestIdx].movementSpeed);
                Debug.Log("resttime: " + enemyPop[bestIdx].restTime);*/
                enemyPrinted = true;
#if UNITY_EDITOR
                SaveTests();
                //REMOVE THE COMMENT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                CreateSOBestEnemies();
                GameManager.instance.createEnemy = false;

                entityManager.DestroyEntity(enemyPopulationArray);
                entityManager.DestroyEntity(intermediateEnemyPopulationArray);
                enemyPopulationArray.Dispose();
                intermediateEnemyPopulationArray.Dispose();
                fitnessArray.Dispose();
#endif
#if UNITY_EDITOR
                enemyPop.Dispose();
                weaponPop.Dispose();
                bestEnemyPop.Dispose();
                bestWeaponPop.Dispose();

                projectileMultipliers.Dispose();
                movementMultipliers.Dispose();
                behaviorMultipliers.Dispose();
                weaponMultipliers.Dispose();
                weaponHasProjectile.Dispose();
                GameManager.instance.Rezero();
#endif
            }
        }
    }

    protected void OnApplicationQuit()
    {

    }

#if UNITY_EDITOR
    public void SaveTests()
    {
        bestFitness = enemyPop[0].fitness;
        averageNBestFitness = AverageFitness(EnemyUtil.nBestEnemies);
        averageFitness = AverageFitness(EnemyUtil.popSize);
        stdDevNBestFitness = StdDevFitness(EnemyUtil.nBestEnemies, averageNBestFitness);
        stdDevFitness = StdDevFitness(EnemyUtil.popSize, averageFitness);

        //Debug.Log(Directory.GetCurrentDirectory());
        string filename = difficulty.ToString() + "-" + EnemyUtil.popSize.ToString() + "-" + EnemyUtil.maxGenerations.ToString();


        using (StreamWriter outputFile = File.AppendText(filename + "Test.csv"))
        {
            outputFile.Write(timeToConverge + ",");
            outputFile.Write(timeAfterSort + ",");
            outputFile.Write(bestFitness + ",");
            outputFile.Write(averageNBestFitness + ",");
            outputFile.Write(averageFitness + ",");
            outputFile.Write(stdDevNBestFitness + ",");
            outputFile.Write(stdDevFitness + "\n");
        }

    }

    public float AverageFitness(int n)
    {
        float averageTotal = 0, average;
        int j, nLoops = 0, counter;
        for (int i = 0; i < n;)
        {
            average = 0;
            counter = 0;
            for (j = 0; (j < 100) && (i < n); j++, i++)
            {
                average += enemyPop[i].fitness;
                counter++;
            }
            nLoops++;
            average = average / counter;
            averageTotal += average;
        }
        return (averageTotal / nLoops);
    }

    public float StdDevFitness(int n, float average)
    {
        float stdDevFitness = 0;
        for (int i = 0; i < n; ++i)
        {
            stdDevFitness += math.pow(enemyPop[i].fitness - average, 2);
        }
        return math.sqrt(stdDevFitness / n);
    }
#endif

#if UNITY_EDITOR
    public void CreateSOBestEnemies()
    {
        EnemySO[] bestEnemies = new EnemySO[EnemyUtil.nBestEnemies];
        int shift = 0, i = 0;
        string foldername = "Assets/Resources/Enemies";
        string subfoldername;
        string filename;
        switch (difficulty)
        {
            case DifficultyEnum.easy:
                subfoldername = "Easy";
                break;
            case DifficultyEnum.medium:
                subfoldername = "Medium";
                break;
            case DifficultyEnum.hard:
                subfoldername = "Hard";
                break;
            case DifficultyEnum.veryhard:
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

        while (i < EnemyUtil.nBestEnemies)
        {
            if (IsEnemyDifferent(i + shift))
            {
                bestEnemies[i] = ScriptableObject.CreateInstance<EnemySO>();
#if UNITY_EDITOR
                AssetDatabase.DeleteAsset(filename + "Enemy" + i + ".asset");
#endif
                bestEnemies[i].Init(enemyPop[i + shift].health, enemyPop[i + shift].damage, enemyPop[i + shift].movementSpeed, enemyPop[i + shift].activeTime,
                    enemyPop[i + shift].restTime, enemyPop[i + shift].weapon, enemyPop[i + shift].movement, enemyPop[i + shift].behavior, enemyPop[i + shift].fitness,
                    weaponPop[i + shift].attackSpeed, weaponPop[i + shift].projectileSpeed);
#if UNITY_EDITOR
                AssetDatabase.CreateAsset(bestEnemies[i], filename + "Enemy" + i + ".asset");
#endif
                ++i;
            }
            else
            {
                ++shift;
            }
        }

    }

    private bool IsEnemyDifferent(int index)
    {
        if (enemyPop[index].health == enemyPop[index + 1].health)
            if (enemyPop[index].damage == enemyPop[index + 1].damage)
                if (math.abs(enemyPop[index].movementSpeed - enemyPop[index + 1].movementSpeed) < 0.001f)
                    if (math.abs(enemyPop[index].activeTime - enemyPop[index + 1].activeTime) < 0.001f)
                        if (math.abs(enemyPop[index].restTime - enemyPop[index + 1].restTime) < 0.001f)
                            if (enemyPop[index].weapon == enemyPop[index + 1].weapon)
                                if (enemyPop[index].movement == enemyPop[index + 1].movement)
                                    if (enemyPop[index].behavior == enemyPop[index + 1].behavior)
                                        if (math.abs(weaponPop[index].attackSpeed - weaponPop[index + 1].attackSpeed) < 0.001f)
                                            if (math.abs(weaponPop[index].projectileSpeed - weaponPop[index + 1].projectileSpeed) < 0.001f)
                                                return false;
        return true;
    }
#endif
}
