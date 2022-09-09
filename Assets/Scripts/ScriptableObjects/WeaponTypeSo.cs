using System;
using System.Text;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu] [Serializable]
    public class WeaponTypeSo : ScriptableObject
    {
        [field: SerializeField] public float FitnessMultiplier { get; set; }
        [field: SerializeField] public bool HasProjectile { get; set; }
        [field: SerializeField] public ProjectileTypeSO Projectile { get; set; }

        [field: SerializeField] public GameObject WeaponPrefab { get; set; }
        [field: SerializeField] public string EnemyTypeName { get; set; }
        
        public bool IsHealer()
        {
            return EnemyTypeName == "Healer";
        }

        public bool IsRanger()
        {
            return HasProjectile;
        }

        public bool IsMelee()
        {
            return !IsRanger() && !IsHealer();
        }
        
        public bool IsSword()
        {
            return EnemyTypeName == "Sword";
        }

        public object GetEnemySpriteString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"<sprite=\"Enemies\" name=\"{EnemyTypeName}\">");
            return stringBuilder.ToString();
        }
    }
}