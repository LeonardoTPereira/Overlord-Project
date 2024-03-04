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
        [Range(1, 6)]
        public int Health;
        [Range(1, 4)]
        public int Damage;
        [Range(.8f, 3.2f)]
        public float MovementSpeed;
        [Range(1.5f, 10f)]
        public float ActiveTime;
        [Range(.3f, 1.5f)]
        public float RestTime;
        [Range(.75f, 4f)]
        public float AttackSpeed;
        [Range(1f, 4f)]
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