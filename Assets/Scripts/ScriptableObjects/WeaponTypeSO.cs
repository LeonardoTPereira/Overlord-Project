using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu] [Serializable]
    public class WeaponTypeSO : ScriptableObject
    {
        public float multiplier;
        public bool hasProjectile;
        [SerializeField]
        public ProjectileTypeSO projectile;
        [SerializeField]
        public GameObject weaponPrefab;
    }
}