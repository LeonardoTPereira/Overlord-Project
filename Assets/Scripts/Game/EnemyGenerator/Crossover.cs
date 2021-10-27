using System;
using UnityEngine;

namespace EnemyGenerator
{
    /// This class holds the crossover operator.
    public class Crossover
    {
        /// Perform a custom BLX-Alpha crossover composed of two stages.
        ///
        /// The first stage applies a fixed-point crossover dividing the genes
        /// enemy and weapon. The second stage calculates the usual BLX-alpha
        /// of all numerical attributes. However, if the weapons of both
        /// individuals are different, the BLX-alpha for the projectile speed
        /// is not performed.
        public static Individual[] Apply(
            Individual _parent1,
            Individual _parent2,
            ref System.Random _rand
        ) {
            // Create aliases for the parents' genes
            Enemy p1e = _parent1.enemy;
            Weapon p1w = _parent1.weapon;
            Enemy p2e = _parent2.enemy;
            Weapon p2w = _parent2.weapon;
            // Initialize the two new individuals performing a fixed 1-point
            // crossover (crossing enemy and weapon genes)
            Individual[] children = new Individual[2];
            children[0] = new Individual(p1e, p2w);
            children[1] = new Individual(p2e, p1w);
            // Apply BLX-alpha on enemy attributes
            float alpha = Common.RandomFloat((0f, 1f), ref _rand);
            (children[0].enemy.health,
             children[1].enemy.health) =
                BLXAlpha(
                    children[0].enemy.health,
                    children[1].enemy.health,
                    SearchSpace.Instance.rHealth,
                    alpha,
                    ref _rand
                );
            (children[0].enemy.strength,
             children[1].enemy.strength) =
                BLXAlpha(
                    children[0].enemy.strength,
                    children[1].enemy.strength,
                    SearchSpace.Instance.rStrength,
                    alpha,
                    ref _rand
                );
            (children[0].enemy.attackSpeed,
             children[1].enemy.attackSpeed) =
                BLXAlpha(
                    children[0].enemy.attackSpeed,
                    children[1].enemy.attackSpeed,
                    SearchSpace.Instance.rAttackSpeed,
                    alpha,
                    ref _rand
                );
            (children[0].enemy.movementSpeed,
             children[1].enemy.movementSpeed) =
                BLXAlpha(
                    children[0].enemy.movementSpeed,
                    children[1].enemy.movementSpeed,
                    SearchSpace.Instance.rMovementSpeed,
                    alpha,
                    ref _rand
                );
            (children[0].enemy.activeTime,
             children[1].enemy.activeTime) =
                BLXAlpha(
                    children[0].enemy.activeTime,
                    children[1].enemy.activeTime,
                    SearchSpace.Instance.rActiveTime,
                    alpha,
                    ref _rand
                );
            (children[0].enemy.restTime,
             children[1].enemy.restTime) =
                BLXAlpha(
                    children[0].enemy.restTime,
                    children[1].enemy.restTime,
                    SearchSpace.Instance.rRestTime,
                    alpha,
                    ref _rand
                );
            // If both weapons are of the same type, then apply BLX-alpha on
            // weapon attributes, otherwise, skip it
            if (p1w.weaponType == p2w.weaponType)
            {
                (children[0].weapon.projectileSpeed,
                 children[1].weapon.projectileSpeed) =
                    BLXAlpha(
                        children[0].weapon.projectileSpeed,
                        children[1].weapon.projectileSpeed,
                        SearchSpace.Instance.rProjectileSpeed,
                        alpha,
                        ref _rand
                    );
            }
            return children;
        }

        /// Return a tuple of two values calculated by the BLX-alpha.
        static (T, T) BLXAlpha<T>(
            T _v1,
            T _v2,
            (T min, T max) _bounds,
            float _alpha,
            ref System.Random _rand
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
            (float, float) range = (max_alpha, min_alpha);
            float c1 = Common.RandomFloat(range, ref _rand);
            float c2 = Common.RandomFloat(range, ref _rand);
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