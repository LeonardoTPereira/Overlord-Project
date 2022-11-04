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
        [field: SerializeField] public String EnemyTypeName { get; set; }
        
        public bool IsHealer()
        {
            return EnemyTypeName == "Healer";
        }

        public bool IsRanger()
        {
            return hasProjectile;
        }

        public bool IsMelee()
        {
            return !IsRanger() && !IsHealer();
        }
        
        public bool IsSword()
        {
            return EnemyTypeName == "Sword";
        }
    }
}