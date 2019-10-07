using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace EnemyGenerator
{
    public struct WeaponComponent : IComponentData
    {
        public ProjectileEnum projectile;
        public float attackSpeed;
        public float projectileSpeed;

        public enum ProjectileEnum
        {
            None,
            Arrow,
            Bomb,
            COUNT
        }
    }

    
}