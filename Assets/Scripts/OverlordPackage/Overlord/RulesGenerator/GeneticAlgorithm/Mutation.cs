using Util;
using System;
#if UNITY_EDITOR
using Codice.Client.Common;
#endif

namespace Overlord.RulesGenerator.EnemyGeneration
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
                enemy.Status1 = RandomSingleton.GetInstance().Next((int)min, (int)max + 1);
            }
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                var (min, max) = (searchSpace.Status2.Min, searchSpace.Status2.Max);
                enemy.Status2 = RandomSingleton.GetInstance().Next((int)min, (int)max + 1);
            }
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                var (min, max) = (searchSpace.Status3.Min, searchSpace.Status3.Max);
                enemy.Status3 = RandomSingleton.GetInstance().Next(min, max + 1);
            }
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                enemy.Movement = RandomSingleton.GetInstance().RandomElementFromList<Enum>(searchSpace.MovementSet.GetAllMovementTypes());
            }
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                var (min, max) = (searchSpace.Status4.Min, searchSpace.Status4.Max);
                enemy.Status4 = RandomSingleton.GetInstance().Next(min, max + 1);
            }
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                var (min, max) = (searchSpace.Status5.Min, searchSpace.Status5.Max);
                enemy.Status5 = RandomSingleton.GetInstance().Next(min, max + 1);
            }
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                var (min, max) = (searchSpace.Status6.Min, searchSpace.Status6.Max);
                enemy.Status6 = RandomSingleton.GetInstance().Next(min, max + 1);
            }
            // Apply mutation on weapon attributes
            var weapon = individual.Weapon;
            var test = RandomSingleton.GetInstance().RandomPercent();
            if (chance > test)
            {                
            }
            if (chance > test)
            {
                weapon.Weapon = RandomSingleton.GetInstance().RandomElementFromList<Enum>(searchSpace.WeaponSet.GetAllWeaponTypes());
            }
            if (chance > RandomSingleton.GetInstance().RandomPercent())
            {
                var (min, max) = (searchSpace.WeaponStatus1.Min, searchSpace.WeaponStatus1.Max);
                weapon.WeaponStatus1 = RandomSingleton.GetInstance().Next(min, max + 1);
            }
            return individual;
        }
    }
}