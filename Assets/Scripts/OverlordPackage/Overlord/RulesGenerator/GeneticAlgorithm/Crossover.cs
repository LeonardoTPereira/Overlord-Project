using System;
using UnityEngine;
using Util;
#if UNITY_EDITOR
using static Codice.Client.Common.Connection.AskCredentialsToUser;
#endif

namespace Overlord.RulesGenerator.EnemyGeneration
{
    /// This class holds the crossover operator.
    public static class Crossover
    {
        /// Perform a custom BLX-Alpha crossover composed of two stages.
        ///
        /// The first stage applies a fixed-point crossover dividing the genes
        /// enemy and weapon. The second stage calculates the usual BLX-alpha
        /// of all numerical attributes. However, if the weapons of both
        /// individuals are different, the BLX-alpha for the projectile speed
        /// is not performed.
        public static Individual[] Apply(
            Individual parent1,
            Individual parent2, SearchSpaceConfig searchSpace
        )
        {
            // Initialize the two new individuals performing a fixed 1-point
            // crossover (crossing enemy and weapon genes)
            Individual[] children = new Individual[2];
            children[0] = new Individual(parent1.Enemy, parent2.Weapon);
            children[1] = new Individual(parent2.Enemy, parent1.Weapon);
            
            float alpha = (float)RandomSingleton.GetInstance().Random.NextDouble();

            // Apply BLX-alpha to enemy attributes
            ApplyBLXAlphaToEnemy(children[0].Enemy, children[1].Enemy, searchSpace, alpha);

            // Apply BLX-alpha to weapon attributes only if both weapons are of same type
            if (parent1.Weapon.Weapon == parent2.Weapon.Weapon)
            {
                ApplyBLXAlphaToWeapons(children[0].Weapon, children[1].Weapon, searchSpace, alpha);
            }

            return children;
        }

        private static void ApplyBLXAlphaToEnemy(EnemyData enemy1, EnemyData enemy2, SearchSpaceConfig searchSpace, float alpha)
        {
            (enemy1.Status1, enemy2.Status1) = BLXAlpha(enemy1.Status1, enemy2.Status1, (searchSpace.Status1.Min, searchSpace.Status1.Max), alpha);
            (enemy1.Status2, enemy2.Status2) = BLXAlpha(enemy1.Status2, enemy2.Status2, (searchSpace.Status2.Min, searchSpace.Status2.Max), alpha);
            (enemy1.Status3, enemy2.Status3) = BLXAlpha(enemy1.Status3, enemy2.Status3, (searchSpace.Status3.Min, searchSpace.Status3.Max), alpha);
            (enemy1.Status4, enemy2.Status4) = BLXAlpha(enemy1.Status4, enemy2.Status4, (searchSpace.Status4.Min, searchSpace.Status4.Max), alpha);
            (enemy1.Status5, enemy2.Status5) = BLXAlpha(enemy1.Status5, enemy2.Status5, (searchSpace.Status5.Min, searchSpace.Status5.Max), alpha);
            (enemy1.Status6, enemy2.Status6) = BLXAlpha(enemy1.Status6, enemy2.Status6, (searchSpace.Status6.Min, searchSpace.Status6.Max), alpha);
        }

        private static void ApplyBLXAlphaToWeapons(WeaponData weapon1, WeaponData weapon2, SearchSpaceConfig searchSpace, float alpha)
        {
            (weapon1.WeaponStatus1, weapon2.WeaponStatus1) =
                BLXAlpha(weapon1.WeaponStatus1, weapon2.WeaponStatus1,
                         (searchSpace.WeaponStatus1.Min, searchSpace.WeaponStatus1.Max),
                         alpha);
        }

        /// Return a tuple of two values calculated by the BLX-alpha.
        static (T, T) BLXAlpha<T>(
            T _v1,
            T _v2,
            (T min, T max) _bounds,
            float _alpha
            )
        {
            // Convert the entered values to float
            Type ft = typeof(float);
            float fv1 = (float)Convert.ChangeType(_v1, ft);
            float fv2 = (float)Convert.ChangeType(_v2, ft);
            float fa = (float)Convert.ChangeType(_bounds.min, ft);
            float fb = (float)Convert.ChangeType(_bounds.max, ft);
            // Identify the maximum and minimum values
            float max = Mathf.Max(fv1, fv2);
            float min = Mathf.Min(fv1, fv2);
            // Calculate the crossover values
            float max_alpha = max + _alpha;
            float min_alpha = min - _alpha;
            float c1 = RandomSingleton.GetInstance().Next(min_alpha, max_alpha);
            float c2 = RandomSingleton.GetInstance().Next(min_alpha, max_alpha);
            // If the values extrapolate the attribute's range of values, then
            // truncate the result to the closest value
            float a = Mathf.Max(Mathf.Min(c1, fb), fa);
            float b = Mathf.Max(Mathf.Min(c2, fb), fa);
            // Convert and return the crossover result to type `T`
            Type pt = typeof(T);
            return (
                (T)Convert.ChangeType(a, pt),
                (T)Convert.ChangeType(b, pt)
            );
        }
    }
}