using UnityEngine;
using Util;

namespace ScriptableObjects
{
    public delegate void ProjectileBehavior(Vector2 shootDirection);

    [CreateAssetMenu]
    public class ProjectileTypeSO : ScriptableObject
    {
        public int multiplier;
        public GameObject projectilePrefab;
        //These data are for player's projectiles only (for now)
        public int damage;
        public float atkSpeed, moveSpeed;
        public Enums.PlayerProjectileEnum projectileBehaviorIndex;
        public string ProjectileName (bool IsInPortuguese)=> IsInPortuguese ? ptProjectileName : enProjectileName;
        public string Description (bool IsInPortuguese) => IsInPortuguese ? ptDescription : enDescription;
        public string ptProjectileName, ptDescription;
        public string enProjectileName, enDescription;
        public Color color;

        public void Copy(ProjectileTypeSO projectileTypeSo)
        {
            color = projectileTypeSo.color;
            projectilePrefab = projectileTypeSo.projectilePrefab;
            damage = projectileTypeSo.damage;
            multiplier = projectileTypeSo.multiplier;
            atkSpeed = projectileTypeSo.atkSpeed;
            moveSpeed = projectileTypeSo.moveSpeed;
            projectileBehaviorIndex = projectileTypeSo.projectileBehaviorIndex;
            enDescription = projectileTypeSo.enDescription;
            enProjectileName = projectileTypeSo.enProjectileName;
            ptDescription = projectileTypeSo.ptDescription;
            ptProjectileName = projectileTypeSo.ptProjectileName;
        }
    }
}