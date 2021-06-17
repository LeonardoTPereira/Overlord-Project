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

}
#endif