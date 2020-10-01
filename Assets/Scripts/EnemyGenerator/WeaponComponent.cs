using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

#if UNITY_EDITOR
namespace EnemyGenerator
{
    public struct WeaponComponent : IComponentData
    {
        public int projectile;
        public float attackSpeed;
        public float projectileSpeed;
    }
    public enum ProjectileEnum
    {
        None,
        Arrow,
        Bomb,
        COUNT
    }

}
#endif