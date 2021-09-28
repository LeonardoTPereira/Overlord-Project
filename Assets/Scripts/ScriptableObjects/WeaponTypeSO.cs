using UnityEngine;

namespace ScriptableObjects
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