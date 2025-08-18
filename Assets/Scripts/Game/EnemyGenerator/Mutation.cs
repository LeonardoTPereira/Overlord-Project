using Util;
using System;

namespace Game.EnemyGenerator
{
    /// This class holds the mutation operator.
    public static class Mutation
    {
        /// Reproduce a new individual by mutating a parent.
        public static Individual Apply(Individual parent, int chance, SearchSpaceConfig searchSpace)
        {
            var individual = parent.Clone();
            // Apply mutation on enemy attributes
            var enemy = individual.Enemy;
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                var (min, max) = (searchSpace.Status1.Min, searchSpace.Status1.Max);
                enemy.Health = RandomSingleton.GetInstance().Next((int)min, (int)max + 1);
            }
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                var (min, max) = (searchSpace.Status2.Min, searchSpace.Status2.Max);
                enemy.Strength = RandomSingleton.GetInstance().Next((int)min, (int)max + 1);
            }
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                var (min, max) = (searchSpace.Status3.Min, searchSpace.Status3.Max);
                enemy.AttackSpeed = RandomSingleton.GetInstance().Next(min, max + 1);
            }
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                enemy.Movement = RandomSingleton.GetInstance().RandomElementFromList<Enum>(searchSpace.MovementSet.GetAllMovementTypes());
            }
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                var (min, max) = (searchSpace.Status4.Min, searchSpace.Status4.Max);
                enemy.MovementSpeed = RandomSingleton.GetInstance().Next(min, max + 1);
            }
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                var (min, max) = (searchSpace.Status5.Min, searchSpace.Status5.Max);
                enemy.ActiveTime = RandomSingleton.GetInstance().Next(min, max + 1);
            }
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                var (min, max) = (searchSpace.Status6.Min, searchSpace.Status6.Max);
                enemy.RestTime = RandomSingleton.GetInstance().Next(min, max + 1);
            }
            // Apply mutation on weapon attributes
            var weapon = individual.Weapon;
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                weapon.Weapon = RandomSingleton.GetInstance().RandomElementFromArray(SearchSpace.Instance.rWeaponType);
            }
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                var (min, max) = (searchSpace.WeaponStatus1.Min, searchSpace.WeaponStatus1.Max);
                weapon.ProjectileSpeed = RandomSingleton.GetInstance().Next(min, max + 1);
            }
            return individual;
        }
    }
}