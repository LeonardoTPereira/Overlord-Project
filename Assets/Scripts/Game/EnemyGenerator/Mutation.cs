using System;

namespace Game.EnemyGenerator
{
    /// This class holds the mutation operator.
    public class Mutation
    {
        /// Reproduce a new individual by mutating a parent.
        public static Individual Apply(
            Individual _parent,
            int _chance,
            ref Random _rand
        ) {
            Individual individual = _parent.Clone();
            // Apply mutation on enemy attributes
            if (_chance > Common.RandomPercent(ref _rand))
            {
                individual.enemy.health = Common.RandomInt(
                    SearchSpace.Instance.rHealth, ref _rand
                );
            }
            if (_chance > Common.RandomPercent(ref _rand))
            {
                individual.enemy.strength = Common.RandomInt(
                    SearchSpace.Instance.rStrength, ref _rand
                );
            }
            if (_chance > Common.RandomPercent(ref _rand))
            {
                individual.enemy.attackSpeed = Common.RandomFloat(
                    SearchSpace.Instance.rAttackSpeed, ref _rand
                );
            }
            if (_chance > Common.RandomPercent(ref _rand))
            {
                individual.enemy.movementType = Common.RandomElementFromArray(
                    SearchSpace.Instance.rMovementType, ref _rand
                );
            }
            if (_chance > Common.RandomPercent(ref _rand))
            {
                individual.enemy.movementSpeed = Common.RandomFloat(
                    SearchSpace.Instance.rMovementSpeed, ref _rand
                );
            }
            if (_chance > Common.RandomPercent(ref _rand))
            {
                individual.enemy.activeTime = Common.RandomFloat(
                    SearchSpace.Instance.rActiveTime, ref _rand
                );
            }
            if (_chance > Common.RandomPercent(ref _rand))
            {
                individual.enemy.restTime = Common.RandomFloat(
                    SearchSpace.Instance.rRestTime, ref _rand
                );
            }
            // Apply mutation on weapon attributes
            if (_chance > Common.RandomPercent(ref _rand))
            {
                individual.weapon.weaponType = Common.RandomElementFromArray(
                    SearchSpace.Instance.rWeaponType, ref _rand
                );
            }
            if (_chance > Common.RandomPercent(ref _rand))
            {
                individual.weapon.projectileSpeed = Common.RandomFloat(
                    SearchSpace.Instance.rProjectileSpeed, ref _rand
                );
            }
            return individual;
        }
    }
}