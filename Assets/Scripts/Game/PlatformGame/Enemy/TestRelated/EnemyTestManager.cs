using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects;
using Util;

namespace PlatformGame.Enemy
{
    public class EnemyTestManager : MonoBehaviour
    {
        public static EnemyTestManager Instance;

        private void Awake()
        {
            if (Instance != null)
                Destroy(this);
            Instance = this;
        }

        public enum MovementEnum
        {
            None,
            Random,
            Follow,
            Flee,
            Random1D,
            Follow1D,
            Flee1D,
            Count
        }

        public bool IsTestActive = false;
        public int Health;
        public int Damage;
        public float MovementSpeed;
        public float ActiveTime;
        public float RestTime;
        public float AttackSpeed;
        public float ProjectileSpeed;
        public Enums.MovementEnum Movement;

        public EnemySO GenerateTestPlataformEnemySO()
        {
            EnemySO enemySo = new EnemySO();
            MovementTypeSO moveTypeSo = new MovementTypeSO();

            enemySo.movement = moveTypeSo;

            enemySo.health = Health;
            enemySo.damage = Damage;
            enemySo.movementSpeed = MovementSpeed;
            enemySo.activeTime = ActiveTime;
            enemySo.restTime = RestTime;
            enemySo.attackSpeed = AttackSpeed;
            enemySo.projectileSpeed = ProjectileSpeed;
            enemySo.movement.enemyMovementIndex = Movement;

            return enemySo;
        }

    }
}