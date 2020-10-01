using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Rendering;
using EnemyGenerator;

public class GameManagerTest : MonoBehaviour
{
    //singleton
    public static GameManagerTest instance = null;

    //The player the enemies will follow
    [SerializeField] private GameObject player = null;

    //Mesh and material if we will create the sprites of the enemies with them.
    //TODO check the video where the CodeMonkey guy uses a spritesheet animation
    //Maybe we will ignore ECS for the gameplay itself as it is overly complicated and we will have few enemyes per room.
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;

    //Prefabs of the weapons
    //TODO create them in unity
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private GameObject bowPrefab;
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private GameObject bombPrefab;

    //Prefabs of the projectiles
    //TODO create them in unity
    [SerializeField] private GameObject projectilePrefab;

    //Array of entities for the population of enemies
    public NativeArray<Entity> enemyPopulationArray;
    //Array of entities for the intermediate population of enemies
    public NativeArray<Entity> intermediateEnemyPopulationArray;
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

    private void Start()
    {
        //We must have an entity manager in our current world to create and handle the entities
        EntityManager entityManager = World.Active.EntityManager;

        //An entity archetype is kind of a struct of entities
        //This one is for the population of enemies itself, having a "population" tag to differentiate it from the intermediate population
        //Also it has the enemy and its weapon component and, 
        //as we may use this entity for the game (or not, and just handle the gameplay with the monobehaviour stuff), it also has a translation
        EntityArchetype enemyArchetype = entityManager.CreateArchetype(
            typeof(Population),
            typeof(EnemyComponent),
            typeof(Translation),
            typeof(WeaponComponent)
        );

        //The entity archetype for the intermediate population. As the translation is not used in the EA evolution, it does not contain one
        EntityArchetype intermediateEnemyArchetype = entityManager.CreateArchetype(
            typeof(IntermediatePopulation),
            typeof(EnemyComponent),
            typeof(WeaponComponent)
        );

        //Instantiate "Population Size" individuals for both populations using a native array
        enemyPopulationArray = new NativeArray<Entity>(EnemyUtil.popSize, Allocator.Temp);
        intermediateEnemyPopulationArray = new NativeArray<Entity>(EnemyUtil.popSize, Allocator.Temp);
        //Create the entities themselves in the memory
        entityManager.CreateEntity(enemyArchetype, enemyPopulationArray);
        entityManager.CreateEntity(intermediateEnemyArchetype, intermediateEnemyPopulationArray);

        //Initialize the initial population of enemies with random values
        for (int i = 0; i < enemyPopulationArray.Length; i++)
        {
            Entity entity = enemyPopulationArray[i];

            entityManager.SetComponentData(entity,
                new WeaponComponent
                {
                    projectile = (WeaponComponent.ProjectileEnum)UnityEngine.Random.Range(0, (int)WeaponComponent.ProjectileEnum.COUNT),
                    attackSpeed = UnityEngine.Random.Range(1, 11),
                    projectileSpeed = UnityEngine.Random.Range(1, 11)
                }
            );

            entityManager.SetComponentData(entity,
                new EnemyComponent
                {
                    health = UnityEngine.Random.Range(1, 11),
                    damage = UnityEngine.Random.Range(1, 11),
                    movementSpeed = UnityEngine.Random.Range(1, 11),
                    activeTime = UnityEngine.Random.Range(1, 11),
                    restTime = UnityEngine.Random.Range(1, 11),
                    weapon = (EnemyComponent.WeaponEnum)UnityEngine.Random.Range(0, (int)EnemyComponent.WeaponEnum.COUNT),
                    movement = (EnemyComponent.MovementEnum)UnityEngine.Random.Range(0, (int)EnemyComponent.MovementEnum.COUNT),
                    fitness = Mathf.Infinity
                }
            );
            //We are not using this right now...
            //TODO: decide if we will use the translation from the ECS or the MonoBehavior, that is easier to handle
            entityManager.SetComponentData(entity,
                new Translation
                {
                    Value = new float3(UnityEngine.Random.Range(-8, 8f), UnityEngine.Random.Range(-5, 5f), 0)
                }
            );

            /*entityManager.SetSharedComponentData(entity, new RenderMesh
            {
                mesh = mesh,
                material = material,
            });*/
        }

        //Kill the temporary arrays to free memory
        enemyPopulationArray.Dispose();
        intermediateEnemyPopulationArray.Dispose();

    }

    public void Update()
    {
        Debug.Log("RoundX");
    }

    //Returns the current position of the player
    public Vector3 GetPlayerPos()
    {
        if(player != null)
            return player.transform.position;
        return new Vector3(0,0,0);
    }
}
