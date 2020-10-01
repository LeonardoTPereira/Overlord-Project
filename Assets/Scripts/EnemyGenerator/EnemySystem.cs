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

public class EnemySystem : ComponentSystem
{
    public delegate Vector3 MovementType(Vector3 playerPos, Vector3 enemyPos);
    protected override void OnUpdate()
    {

        /*Entities.ForEach((ref Translation translation, ref EnemyComponent enemyComponent) => {
            Vector3 _playerPos = GameManagerTest.instance.GetPlayerPos();
            MovementType enemyMovement = GetMovementType(enemyComponent.movement);
            translation.Value += (float3)(enemyComponent.movementSpeed * enemyMovement(_playerPos, translation.Value) * Time.deltaTime);
        });*/

    }

    //Returns the type of movement the enemy has
    public MovementType GetMovementType(EnemyComponent.MovementEnum moveTypeEnum)
    {
        switch (moveTypeEnum)
        {
            case EnemyComponent.MovementEnum.None:
                return EnemyMovement.NoMovement;
            case EnemyComponent.MovementEnum.Random:
                return EnemyMovement.MoveRandomly;
            case EnemyComponent.MovementEnum.Flee:
                return EnemyMovement.FleeFromPlayer;
            case EnemyComponent.MovementEnum.Follow:
                return EnemyMovement.FollowPlayer;
            default:
                Debug.Log("No Movement Attached to Enemy");
                return null;
        }
    }
}


//Job to calculate the fitness of the whole population
public class EASystem : JobComponentSystem {
    //The job that handles the fitness calculatios
    [BurstCompile]
    public struct FitnessJob : IJobForEachWithEntity<EnemyComponent, WeaponComponent>
    {
        public void Execute(Entity entity, int index, ref EnemyComponent enemy, [ReadOnly] ref WeaponComponent weapon)
        {
            float damageMultiplier, movementMultiplier;
            int projectileMultiplier = 0;
            
            //Depending on each weapon, assign a damage multiplier
            switch (enemy.weapon)
            {
                case EnemyComponent.WeaponEnum.None:
                    damageMultiplier = 1.0f;
                    break;
                case EnemyComponent.WeaponEnum.Sword:
                    damageMultiplier = 1.1f;
                    break;
                case EnemyComponent.WeaponEnum.Bow:
                    damageMultiplier = 1.1f;
                    break;
                case EnemyComponent.WeaponEnum.Bomb:
                    damageMultiplier = 1.2f;
                    break;
                case EnemyComponent.WeaponEnum.Shield:
                    damageMultiplier = 1.1f;
                    break;
                default:
                    damageMultiplier = 1.0f;
                    break;
            }

            //Depending on movement type, assign a movement multiplier
            switch (enemy.movement)
            {
                case EnemyComponent.MovementEnum.None:
                    movementMultiplier = 0.0f;
                    break;
                case EnemyComponent.MovementEnum.Random:
                    movementMultiplier = 1.1f;
                    break;
                case EnemyComponent.MovementEnum.Flee:
                    movementMultiplier = 1.2f;
                    break;
                case EnemyComponent.MovementEnum.Follow:
                    movementMultiplier = 1.3f;
                    break;
                default:
                    movementMultiplier = 1.0f;
                    break;
            }

            //If the weapon throws projectiles, assign a projectile multiplier
            if (enemy.weapon != EnemyComponent.WeaponEnum.None)
            {
                switch (weapon.projectile)
                {
                    case WeaponComponent.ProjectileEnum.None:
                        projectileMultiplier = 0;
                        break;
                    case WeaponComponent.ProjectileEnum.Arrow:
                        projectileMultiplier = 1;
                        break;
                    default:
                        projectileMultiplier = 0;
                        break;
                }
            }

            enemy.fitness = enemy.damage * damageMultiplier + enemy.health + enemy.movementSpeed * movementMultiplier + 1 / enemy.restTime + enemy.activeTime + projectileMultiplier * ((1 / weapon.attackSpeed) + weapon.projectileSpeed);
        }
    }

    [BurstCompile]
    public struct TournamentJob : IJobForEachWithEntity<EnemyComponent, WeaponComponent>
    {
        public void Execute(Entity entity, int index, ref EnemyComponent enemy, [ReadOnly] ref WeaponComponent weapon)
        {
            
            //Tournament logic goes here.
        }
    }

    //The EA main loop
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float startTime = Time.realtimeSinceStartup;
        FitnessJob job = new FitnessJob
        { };
        //return job.Schedule(this, inputDeps);
        JobHandle handle = job.Schedule(this, inputDeps);
        handle.Complete();
        Debug.Log(Time.realtimeSinceStartup-startTime);
        return handle;
    }
}