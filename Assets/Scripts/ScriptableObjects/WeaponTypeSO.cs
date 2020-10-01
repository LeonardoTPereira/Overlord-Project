using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyGenerator
{
    [CreateAssetMenu]
    public class WeaponTypeSO : ScriptableObject
    {
        public float multiplier;
        public bool hasProjectile;
        public ProjectileTypeSO projectile;
        public GameObject weaponPrefab;
    }
}