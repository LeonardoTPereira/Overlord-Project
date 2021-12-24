using Util;

namespace Game.EnemyGenerator
{
    /// This class holds the mutation operator.
    public static class Mutation
    {
        /// Reproduce a new individual by mutating a parent.
        public static Individual Apply( Individual parent,  int chance) {
            var individual = parent.Clone();
            // Apply mutation on enemy attributes
            var enemy = individual.Enemy;
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                var (min, max) = SearchSpace.Instance.rHealth;
                enemy.Health = RandomSingleton.GetInstance().Next(min, max+1);
            }
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                var (min, max) = SearchSpace.Instance.rStrength;
                enemy.Strength = RandomSingleton.GetInstance().Next(min, max+1);
            }
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                var (min, max) = SearchSpace.Instance.rAttackSpeed;
                enemy.AttackSpeed = RandomSingleton.GetInstance().Next(min, max+1);
            }
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                enemy.Movement = RandomSingleton.GetInstance().RandomElementFromArray(SearchSpace.Instance.rMovementType);
            }
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                var (min, max) = SearchSpace.Instance.rMovementSpeed;
                enemy.MovementSpeed = RandomSingleton.GetInstance().Next(min, max+1);
            }
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                var (min, max) = SearchSpace.Instance.rActiveTime;
                enemy.ActiveTime = RandomSingleton.GetInstance().Next(min, max + 1);
            }
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                var (min, max) = SearchSpace.Instance.rRestTime;
                enemy.RestTime = RandomSingleton.GetInstance().Next(min, max+1);
            }
            // Apply mutation on weapon attributes
            var weapon = individual.Weapon;
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                weapon.Weapon = RandomSingleton.GetInstance().RandomElementFromArray(SearchSpace.Instance.rWeaponType);
            }
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                var (min, max) = SearchSpace.Instance.rProjectileSpeed;
                weapon.ProjectileSpeed = RandomSingleton.GetInstance().Next(min, max+1);
            }
            return individual;
        }
    }
}