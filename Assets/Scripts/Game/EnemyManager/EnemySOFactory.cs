using ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.EnemyGenerator
{
    public class EnemySOFactory
    {
        private readonly MovementTypeRuntimeSetSO _movementSet;
        private readonly WeaponTypeRuntimeSetSO _weaponSet;

        public EnemySOFactory(MovementTypeRuntimeSetSO movementSet, WeaponTypeRuntimeSetSO weaponSet)
        {
            _movementSet = movementSet ?? throw new ArgumentNullException(nameof(movementSet));
            _weaponSet = weaponSet ?? throw new ArgumentNullException(nameof(weaponSet));
        }

        public List<EnemySO> GetEnemiesSOFromSolution(IEnumerable<Individual> solution)
        {
            var enemyList = new List<EnemySO>();

            foreach (var individual in solution)
            {
                int weaponIndex = Convert.ToInt32(individual.Weapon.Weapon);
                int movementIndex = Convert.ToInt32(individual.Enemy.Movement);
                //int behaviorIndex = 0; // ainda não implementado
                //if (behaviorIndex < 0 || behaviorIndex >= _behaviorSet.Items.Count) continue;

                ValidateIndices(weaponIndex, movementIndex);

                enemyList.Add(IndividualEnemySO(individual));
            }

            return enemyList;
        }

        private void ValidateIndices(int weaponIndex, int movementIndex)
        {
            if (weaponIndex < 0 || weaponIndex >= _weaponSet.Items.Count)
            {
                throw new IndexOutOfRangeException($"Weapon index {weaponIndex} is out of range.");
            }
            if (movementIndex < 0 || movementIndex >= _movementSet.Items.Count)
            {
                throw new IndexOutOfRangeException($"Movement index {movementIndex} is out of range.");
            }
        }

        private EnemySO IndividualEnemySO(Individual individual)
        {
            EnemySO enemySo = ScriptableObject.CreateInstance<EnemySO>();

            enemySo.Init(
                individual.Enemy.Health,
                individual.Enemy.Strength,
                individual.Enemy.MovementSpeed,
                individual.Enemy.ActiveTime,
                individual.Enemy.RestTime,
                _weaponSet.Items[Convert.ToInt32(individual.Weapon.Weapon)],
                _movementSet.Items[Convert.ToInt32(individual.Enemy.Movement)],
                null, // Behavior not implemented yet
                individual.FitnessValue,
                individual.Enemy.AttackSpeed,
                individual.Weapon.ProjectileSpeed
            );

            return enemySo;
        }
    }
}