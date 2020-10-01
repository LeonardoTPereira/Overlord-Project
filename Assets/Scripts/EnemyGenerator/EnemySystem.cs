using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using EnemyGenerator;
using UnityEngine.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

#if UNITY_EDITOR
public class EnemySystem : ComponentSystem
{
    protected override void OnUpdate()
    {

    }

    //Returns the type of movement the enemy has
    /*public MovementType GetMovementType(MovementEnum moveTypeEnum)
    {
        switch (moveTypeEnum)
        {
            case MovementEnum.None:
                return EnemyMovement.NoMovement;
            case MovementEnum.Random:
                return EnemyMovement.MoveRandomly;
            case MovementEnum.Flee:
                return EnemyMovement.FleeFromPlayer;
            case MovementEnum.Follow:
                return EnemyMovement.FollowPlayer;
            default:
                Debug.Log("No Movement Attached to Enemy");
                return null;
        }
    }*/
}


//Job to calculate the fitness of the whole population
public class EASystem : JobComponentSystem
{

    [RequireComponentTag(typeof(Population))]
    [BurstCompile]
    struct CopyPopulationJob : IJobForEachWithEntity<EnemyComponent, WeaponComponent>
    {
        public NativeArray<EnemyComponent> enemyPopulationCopy;
        public NativeArray<WeaponComponent> weaponPopulationCopy;

        public void Execute(Entity entity, int index, [ReadOnly]ref EnemyComponent enemy, [ReadOnly]ref WeaponComponent weapon)
        {
            enemyPopulationCopy[index] = enemy;
            weaponPopulationCopy[index] = weapon;
        }
    }

    [RequireComponentTag(typeof(IntermediatePopulation))]
    [BurstCompile]
    struct CopyIntermediatePopulationJob : IJobForEachWithEntity<EnemyComponent, WeaponComponent>
    {
        public NativeArray<EnemyComponent> enemyPopulationCopy;
        public NativeArray<WeaponComponent> weaponPopulationCopy;
        [ReadOnly] public int popSize;

        public void Execute(Entity entity, int index, [ReadOnly]ref EnemyComponent enemy, [ReadOnly]ref WeaponComponent weapon)
        {
            enemyPopulationCopy[index - popSize] = enemy;
            weaponPopulationCopy[index - popSize] = weapon;
        }
    }

    [RequireComponentTag(typeof(Population))]
    [BurstCompile]
    struct ReplacePopulationJob : IJobForEachWithEntity<EnemyComponent, WeaponComponent>
    {
        public NativeArray<EnemyComponent> enemyPopulationCopy;
        public NativeArray<WeaponComponent> weaponPopulationCopy;

        public void Execute(Entity entity, int index, ref EnemyComponent enemy, ref WeaponComponent weapon)
        {
            enemy = enemyPopulationCopy[index];
            weapon = weaponPopulationCopy[index];
        }
    }

    [RequireComponentTag(typeof(Population))]
    [BurstCompile]
    struct GetFitnessJob : IJobForEachWithEntity<EnemyComponent>
    {
        public NativeArray<float> fitness;

        public void Execute(Entity entity, int index, [ReadOnly]ref EnemyComponent enemy)
        {
            fitness[index] = enemy.fitness;
        }
    }

    //The job that handles the fitness calculatios
    [RequireComponentTag(typeof(Population))]
    [BurstCompile]
    public struct FitnessJob : IJobForEach<EnemyComponent, WeaponComponent>
    {
        [ReadOnly]
        public NativeArray<int> projectileMultipliers;
        [ReadOnly]
        public NativeArray<float> movementMultipliers;
        [ReadOnly]
        public NativeArray<float> weaponMultipliers;
        [ReadOnly]
        public NativeArray<bool> weaponHasProjectile;
        public void Execute(ref EnemyComponent enemy, [ReadOnly] ref WeaponComponent weapon)
        {
            float damageMultiplier, movementMultiplier;
            int projectileMultiplier = 0;

            //Depending on each weapon, assign a damage 
            damageMultiplier = weaponMultipliers[enemy.weapon];

            //Depending on movement type, assign a movement multiplier
            movementMultiplier = movementMultipliers[enemy.movement];

            //If the weapon throws projectiles, assign a projectile multiplier
            if (weaponHasProjectile[enemy.weapon])
                projectileMultiplier = projectileMultipliers[weapon.projectile];

            enemy.fitness = enemy.damage * damageMultiplier + enemy.health + enemy.movementSpeed * movementMultiplier + 1 / enemy.restTime + enemy.activeTime + projectileMultiplier * (weapon.attackSpeed + weapon.projectileSpeed);
        }
    }

    //The job that handles the fitness calculatios
    [RequireComponentTag(typeof(IntermediatePopulation))]
    [BurstCompile]
    public struct NewPopFitnessJob : IJobForEach<EnemyComponent, WeaponComponent>
    {
        [ReadOnly]
        public NativeArray<int> projectileMultipliers;
        [ReadOnly]
        public NativeArray<float> movementMultipliers;
        [ReadOnly]
        public NativeArray<float> weaponMultipliers;
        [ReadOnly]
        public NativeArray<bool> weaponHasProjectile;
        public void Execute(ref EnemyComponent enemy, [ReadOnly] ref WeaponComponent weapon)
        {
            float damageMultiplier, movementMultiplier;
            int projectileMultiplier = 0;

            //Depending on each weapon, assign a damage 
            damageMultiplier = weaponMultipliers[enemy.weapon];

            //Depending on movement type, assign a movement multiplier
            movementMultiplier = movementMultipliers[enemy.movement];

            //If the weapon throws projectiles, assign a projectile multiplier
            if (weaponHasProjectile[enemy.weapon])
                projectileMultiplier = projectileMultipliers[weapon.projectile];

            enemy.fitness = enemy.damage * damageMultiplier + enemy.health + enemy.movementSpeed * movementMultiplier + 1 / enemy.restTime + enemy.activeTime + projectileMultiplier * (weapon.attackSpeed + weapon.projectileSpeed);
        }
    }

    [BurstCompile]
    public struct TournamentJob : IJobForEach<IntermediatePopulation>
    {
        [ReadOnly]
        public NativeArray<float> enemyPopulationFitness;
        [ReadOnly]
        public float desiredFitness;
        public Unity.Mathematics.Random random;


        public void Execute(ref IntermediatePopulation interPop)
        {
            int auxIdx1, auxIdx2;
            float auxFit1, auxFit2;

            auxIdx1 = random.NextInt(0, enemyPopulationFitness.Length);
            auxIdx2 = random.NextInt(0, enemyPopulationFitness.Length);

            auxFit1 = math.abs(enemyPopulationFitness[auxIdx1] - desiredFitness);
            auxFit2 = math.abs(enemyPopulationFitness[auxIdx2] - desiredFitness);

            if (auxFit1 < auxFit2)
            {
                interPop.parent1 = auxIdx1;
            }
            else
            {
                interPop.parent1 = auxIdx2;
            }
            auxIdx1 = random.NextInt(0, enemyPopulationFitness.Length);
            auxIdx2 = random.NextInt(0, enemyPopulationFitness.Length);

            auxFit1 = math.abs(enemyPopulationFitness[auxIdx1] - desiredFitness);
            auxFit2 = math.abs(enemyPopulationFitness[auxIdx2] - desiredFitness);

            if (auxFit1 < auxFit2)
            {
                interPop.parent2 = auxIdx1;
            }
            else
            {
                interPop.parent2 = auxIdx2;
            }

        }
    }

    [BurstCompile]
    struct CrossoverJob : IJobForEach<EnemyComponent, WeaponComponent, IntermediatePopulation>
    {
        [ReadOnly]
        public NativeArray<EnemyComponent> enemyPopulationCopy;
        [ReadOnly]
        public NativeArray<WeaponComponent> weaponPopulationCopy;
        public Unity.Mathematics.Random random;
        public void Execute(ref EnemyComponent enemy, ref WeaponComponent weapon, [ReadOnly] ref IntermediatePopulation interPop)
        {
            int mainParent, secParent;
            if (random.NextInt(0, 100) < EnemyUtil.crossChance)
            {
                enemy.health = (enemyPopulationCopy[interPop.parent1].health + enemyPopulationCopy[interPop.parent2].health) / 2;
                enemy.damage = (enemyPopulationCopy[interPop.parent1].damage + enemyPopulationCopy[interPop.parent2].damage) / 2;
                enemy.movementSpeed = (enemyPopulationCopy[interPop.parent1].movementSpeed + enemyPopulationCopy[interPop.parent2].movementSpeed) / 2;
                enemy.activeTime = (enemyPopulationCopy[interPop.parent1].activeTime + enemyPopulationCopy[interPop.parent2].activeTime) / 2;
                enemy.restTime = (enemyPopulationCopy[interPop.parent1].restTime + enemyPopulationCopy[interPop.parent2].restTime) / 2;

                if (random.NextBool())
                {
                    mainParent = interPop.parent1;
                    secParent = interPop.parent2;
                }
                else
                {
                    mainParent = interPop.parent2;
                    secParent = interPop.parent1;
                }
                enemy.weapon = enemyPopulationCopy[mainParent].weapon;
                weapon.projectile = weaponPopulationCopy[mainParent].projectile;
                if (enemyPopulationCopy[mainParent].weapon == enemyPopulationCopy[secParent].weapon)
                {
                    weapon.attackSpeed = (weaponPopulationCopy[mainParent].attackSpeed + weaponPopulationCopy[secParent].attackSpeed) / 2;
                    weapon.projectileSpeed = (weaponPopulationCopy[mainParent].projectileSpeed + weaponPopulationCopy[secParent].projectileSpeed) / 2;
                }
                else
                {
                    weapon.attackSpeed = weaponPopulationCopy[mainParent].attackSpeed;
                    weapon.projectileSpeed = weaponPopulationCopy[mainParent].projectileSpeed;
                }

                if (random.NextBool())
                {
                    mainParent = interPop.parent1;
                    secParent = interPop.parent2;
                }
                else
                {
                    mainParent = interPop.parent2;
                    secParent = interPop.parent1;
                }
                enemy.movement = enemyPopulationCopy[mainParent].movement;
                //TODO think about the movement speed averaging according to the movement type
            }
            else
            {
                if (random.NextBool())
                {
                    mainParent = interPop.parent1;
                    secParent = interPop.parent2;
                }
                else
                {
                    mainParent = interPop.parent2;
                    secParent = interPop.parent1;
                }
                enemy.health = enemyPopulationCopy[mainParent].health;
                enemy.damage = enemyPopulationCopy[mainParent].damage;
                enemy.movementSpeed = enemyPopulationCopy[mainParent].movementSpeed;
                enemy.activeTime = enemyPopulationCopy[mainParent].activeTime;
                enemy.restTime = enemyPopulationCopy[mainParent].restTime;

                enemy.weapon = enemyPopulationCopy[mainParent].weapon;
                weapon.projectile = weaponPopulationCopy[mainParent].projectile;
                weapon.attackSpeed = weaponPopulationCopy[mainParent].attackSpeed;
                weapon.projectileSpeed = weaponPopulationCopy[mainParent].projectileSpeed;

                enemy.movement = enemyPopulationCopy[mainParent].movement;
            }
        }
    }

    [BurstCompile]
    struct MutationJob : IJobForEach<EnemyComponent, WeaponComponent, IntermediatePopulation>
    {
        public Unity.Mathematics.Random random;
        public int projectileLength, weaponLength, movementLength;
        public void Execute(ref EnemyComponent enemy, ref WeaponComponent weapon, [ReadOnly] ref IntermediatePopulation interPop)
        {
            if (random.NextInt(0, 100) < EnemyUtil.mutChance)
                weapon.projectile = random.NextInt(0, projectileLength);
            //weapon.projectile = (WeaponComponent.ProjectileEnum)random.NextInt(0, (int)WeaponComponent.ProjectileEnum.COUNT);
            if (random.NextInt(0, 100) < EnemyUtil.mutChance)
                weapon.attackSpeed = random.NextFloat(EnemyUtil.minAtkSpeed, EnemyUtil.maxAtkSpeed);
            if (random.NextInt(0, 100) < EnemyUtil.mutChance)
                weapon.projectileSpeed = random.NextFloat(EnemyUtil.minProjectileSpeed, EnemyUtil.maxProjectileSpeed);
            if (random.NextInt(0, 100) < EnemyUtil.mutChance)
                enemy.health = random.NextInt(EnemyUtil.minHealth, EnemyUtil.maxHealth);
            if (random.NextInt(0, 100) < EnemyUtil.mutChance)
                enemy.damage = random.NextInt(EnemyUtil.minDamage, EnemyUtil.maxDamage);
            if (random.NextInt(0, 100) < EnemyUtil.mutChance)
                enemy.movementSpeed = random.NextFloat(EnemyUtil.minMoveSpeed, EnemyUtil.maxMoveSpeed);
            if (random.NextInt(0, 100) < EnemyUtil.mutChance)
                enemy.activeTime = random.NextFloat(EnemyUtil.minActivetime, EnemyUtil.maxActiveTime);
            if (random.NextInt(0, 100) < EnemyUtil.mutChance)
                enemy.restTime = random.NextFloat(EnemyUtil.minResttime, EnemyUtil.maxRestTime);
            if (random.NextInt(0, 100) < EnemyUtil.mutChance)
                enemy.weapon = random.NextInt(0, weaponLength);
            if (random.NextInt(0, 100) < EnemyUtil.mutChance)
                enemy.movement = random.NextInt(0, movementLength);
        }
    }


    [RequireComponentTag(typeof(Population))]
    [BurstCompile]
    struct FindBestJob : IJobForEachWithEntity<EnemyComponent, WeaponComponent>
    {
        public float bestFitness;
        public int bestIndex;
        [ReadOnly] public float desiredFitness;
        public void Execute(Entity entity, int index, [ReadOnly]ref EnemyComponent enemy, [ReadOnly]ref WeaponComponent weapon)
        {
            float auxFitness = math.abs(enemy.fitness - desiredFitness);
            if (auxFitness < bestFitness)
            {
                bestFitness = auxFitness;
                bestIndex = index;
            }
        }
    }

    [BurstCompile]
    struct EmptyJob : IJob
    {
        public void Execute()
        {
        }
    }
    //The EA main loop
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        JobHandle handle;
        GetFitnessJob getFitnessJob;
        if (GameManager.instance.createEnemy)
        {
            //Debug.Log("Creating Enemies");
            if (GameManagerTest.instance.generationCounter < EnemyUtil.maxGenerations)
            {
                if (GameManagerTest.instance.generationCounter == 0)
                {

                    FitnessJob fitJob = new FitnessJob
                    {
                        projectileMultipliers = GameManagerTest.instance.projectileMultipliers,
                        movementMultipliers = GameManagerTest.instance.movementMultipliers,
                        weaponMultipliers = GameManagerTest.instance.weaponMultipliers,
                        weaponHasProjectile = GameManagerTest.instance.weaponHasProjectile
                    };
                    //return job.Schedule(this, inputDeps);
                    handle = fitJob.Schedule(this, inputDeps);
                    handle.Complete();


                    getFitnessJob = new GetFitnessJob
                    {
                        fitness = GameManagerTest.instance.fitnessArray
                    };

                    handle = getFitnessJob.Schedule(this, inputDeps);
                    handle.Complete();

                    CopyPopulationJob copyPopulation = new CopyPopulationJob
                    {
                        enemyPopulationCopy = GameManagerTest.instance.enemyPop,
                        weaponPopulationCopy = GameManagerTest.instance.weaponPop
                    };


                    handle = copyPopulation.Schedule(this, inputDeps);

                    handle.Complete();

                }

                var random = new Unity.Mathematics.Random((uint)UnityEngine.Random.Range(1, 100000));

                TournamentJob tournJob = new TournamentJob()
                {
                    enemyPopulationFitness = GameManagerTest.instance.fitnessArray,
                    desiredFitness = EnemyUtil.desiredFitness,
                    random = random
                };

                handle = tournJob.Schedule(this, inputDeps);

                handle.Complete();

                //Debug.Log("Elite Gen - " + GameManagerTest.instance.generationCounter);
                //Elitism
                /*for (int i = 0; i < EnemyUtil.nBestEnemies; ++i)
                {
                    GameManagerTest.instance.bestEnemyPop[i] = GameManagerTest.instance.enemyPop[i];
                    GameManagerTest.instance.bestWeaponPop[i] = GameManagerTest.instance.weaponPop[i];
                    //Debug.Log("Best - " + i + " Fitness - " + GameManagerTest.instance.bestEnemyPop[i].fitness);
                }*/

                random = new Unity.Mathematics.Random((uint)UnityEngine.Random.Range(1, 100000));

                CrossoverJob crossJob = new CrossoverJob
                {
                    random = random,
                    enemyPopulationCopy = GameManagerTest.instance.enemyPop,
                    weaponPopulationCopy = GameManagerTest.instance.weaponPop
                };

                handle = crossJob.Schedule(this, inputDeps);

                handle.Complete();

                random = new Unity.Mathematics.Random((uint)UnityEngine.Random.Range(1, 100000));

                MutationJob mutJob = new MutationJob
                {
                    projectileLength = GameManagerTest.instance.projectileMultipliers.Length,
                    weaponLength = GameManagerTest.instance.weaponMultipliers.Length,
                    movementLength = GameManagerTest.instance.movementMultipliers.Length,
                    random = random,
                };

                handle = mutJob.Schedule(this, inputDeps);

                handle.Complete();

                NewPopFitnessJob newfitJob = new NewPopFitnessJob
                {
                    projectileMultipliers = GameManagerTest.instance.projectileMultipliers,
                    movementMultipliers = GameManagerTest.instance.movementMultipliers,
                    weaponMultipliers = GameManagerTest.instance.weaponMultipliers,
                    weaponHasProjectile = GameManagerTest.instance.weaponHasProjectile
                };
                //return job.Schedule(this, inputDeps);
                handle = newfitJob.Schedule(this, inputDeps);

                handle.Complete();

                CopyIntermediatePopulationJob copyIntermediatePopJob = new CopyIntermediatePopulationJob
                {
                    enemyPopulationCopy = GameManagerTest.instance.enemyPop,
                    weaponPopulationCopy = GameManagerTest.instance.weaponPop
                };

                handle = copyIntermediatePopJob.Schedule(this, inputDeps);

                handle.Complete();

                //Elitism
                /*for (int i = 0; i < EnemyUtil.nBestEnemies; ++i)
                {
                    GameManagerTest.instance.enemyPop[i] = GameManagerTest.instance.bestEnemyPop[i];
                    GameManagerTest.instance.weaponPop[i] = GameManagerTest.instance.bestWeaponPop[i];
                }*/


                /*TimSortSystem.SortEnemiesJob sortJob = new TimSortSystem.SortEnemiesJob
                {
                    enemies = GameManagerTest.instance.enemyPop,
                    weapons = GameManagerTest.instance.weaponPop,
                    random = random
                };
                //return job.Schedule(this, inputDeps);

                handle = sortJob.Schedule(inputDeps);

                handle.Complete();*/

                ReplacePopulationJob replacePopulation = new ReplacePopulationJob
                {
                    enemyPopulationCopy = GameManagerTest.instance.enemyPop,
                    weaponPopulationCopy = GameManagerTest.instance.weaponPop
                };

                handle = replacePopulation.Schedule(this, inputDeps);

                handle.Complete();

                getFitnessJob = new GetFitnessJob
                {
                    fitness = GameManagerTest.instance.fitnessArray
                };

                handle = getFitnessJob.Schedule(this, inputDeps);

                handle.Complete();

                GameManagerTest.instance.generationCounter++;
                return handle;
            }
            else if (GameManagerTest.instance.generationCounter == EnemyUtil.maxGenerations)
            {
                float bestFitness = Mathf.Infinity;
                FindBestJob findBest = new FindBestJob
                {
                    bestIndex = GameManagerTest.instance.bestIdx,
                    bestFitness = bestFitness,
                    desiredFitness = EnemyUtil.desiredFitness
                };

                handle = findBest.Schedule(this, inputDeps);
                GameManagerTest.instance.timeToConverge = Time.realtimeSinceStartup - GameManagerTest.instance.startTime;
                //Debug.Log(Time.realtimeSinceStartup - GameManagerTest.instance.startTime);
                GameManagerTest.instance.enemyGenerated = true;

                handle.Complete();

                return handle;
            }
        }
        EmptyJob emptyJob = new EmptyJob
        { };
        handle = emptyJob.Schedule();
        return handle;

    }
}
#endif
#if UNITY_EDITOR
public class SignalEAEnding : ComponentSystem
{

    protected override void OnUpdate()
    {
        if (GameManager.instance.createEnemy)
        {
            if ((GameManagerTest.instance.generationCounter == EnemyUtil.maxGenerations) && GameManagerTest.instance.enemyGenerated)
            {
                //Debug.Log("This different update!");

                GameManagerTest.instance.generationCounter++;
                GameManagerTest.instance.enemyReady = true;
            }
        }
    }
}
#endif
