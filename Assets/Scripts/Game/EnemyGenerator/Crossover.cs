using System;
using UnityEngine;
using Util;

namespace Game.EnemyGenerator
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
            Individual parent2
        ) {
            // Create aliases for the parents' genes
            var parent1Enemy = parent1.Enemy;
            var parent1Weapon = parent1.Weapon;
            var parent2Enemy = parent2.Enemy;
            var parent2Weapon = parent2.Weapon;
            // Initialize the two new individuals performing a fixed 1-point
            // crossover (crossing enemy and weapon genes)
            Individual[] children = new Individual[2];
            children[0] = new Individual(parent1Enemy, parent2Weapon);
            children[1] = new Individual(parent2Enemy, parent1Weapon);
            // Apply BLX-alpha on enemy attributes
            float alpha = (float)RandomSingleton.GetInstance().Random.NextDouble();
            var enemy1 = children[0].Enemy;
            var enemy2 = children[1].Enemy;
            (enemy1.Health,
             enemy2.Health) =
                BLXAlpha(
                    enemy1.Health,
                    enemy2.Health,
                    SearchSpace.Instance.rHealth,
                    alpha
                    );
            (enemy1.Strength,
             enemy2.Strength) =
                BLXAlpha(enemy1.Strength,
                    enemy2.Strength,
                    SearchSpace.Instance.rStrength,
                    alpha
                    );
            (enemy1.AttackSpeed,
             enemy2.AttackSpeed) =
                BLXAlpha(
                    enemy1.AttackSpeed,
                    enemy2.AttackSpeed,
                    SearchSpace.Instance.rAttackSpeed,
                    alpha
                    );
            (enemy1.MovementSpeed,
             enemy2.MovementSpeed) =
                BLXAlpha(
                    enemy1.MovementSpeed,
                    enemy2.MovementSpeed,
                    SearchSpace.Instance.rMovementSpeed,
                    alpha
                    );
            (enemy1.ActiveTime,
             enemy2.ActiveTime) =
                BLXAlpha(
                    enemy1.ActiveTime,
                    enemy2.ActiveTime,
                    SearchSpace.Instance.rActiveTime,
                    alpha
                    );
            (enemy1.RestTime,
             enemy2.RestTime) =
                BLXAlpha(
                    enemy1.RestTime,
                    enemy2.RestTime,
                    SearchSpace.Instance.rRestTime,
                    alpha
                    );
            // If both weapons are of the same type, then apply BLX-alpha on
            // weapon attributes, otherwise, skip it
            if (parent1Weapon.Weapon == parent2Weapon.Weapon)
            {
                var weapon1 = children[0].Weapon;
                var weapon2 = children[1].Weapon;
                (weapon1.ProjectileSpeed,
                        weapon2.ProjectileSpeed) =
                    BLXAlpha(
                        weapon1.ProjectileSpeed,
                        weapon2.ProjectileSpeed,
                        SearchSpace.Instance.rProjectileSpeed,
                        alpha
                        );
            }
            return children;
        }

        /// Return a tuple of two values calculated by the BLX-alpha.
        static (T, T) BLXAlpha<T>(
            T _v1,
            T _v2,
            (T min, T max) _bounds,
            float _alpha
            ) {
            // Convert the entered values to float
            Type ft = typeof(float);
            float fv1 = (float) Convert.ChangeType(_v1, ft);
            float fv2 = (float) Convert.ChangeType(_v2, ft);
            float fa = (float) Convert.ChangeType(_bounds.min, ft);
            float fb = (float) Convert.ChangeType(_bounds.max, ft);
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
                (T) Convert.ChangeType(a, pt),
                (T) Convert.ChangeType(b, pt)
            );
        }
    }
}