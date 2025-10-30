using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlord.RulesGenerator.EnemyGeneration
{
    public class GenericEnemyFitness : IEnemyFitness
    {
        SearchSpaceConfig _searchSpace;

        public void SetSearchSpace(SearchSpaceConfig searchSpace)
        {
            _searchSpace = searchSpace;
        }

        public void Calculate(ref Individual individual, float goal)
        {
            float fitnessFactor = CalculateFitnessFactor(individual);
            individual.FitnessValue = Math.Abs(goal - fitnessFactor);
        }

        public bool IsBest(Individual _i1, Individual _i2)
        {
            return _i1.FitnessValue > _i2.FitnessValue;
        }

        private float CalculateFitnessFactor(Individual individual)
        {
            float fitnessFactor = 0.0f;
            float WeaponFactor = CalculateWeaponFactor(individual);
            float PowerFactor = CalculatePowerFactor(individual);
            float LazyFactor = CalculateLazynessFactor(individual);

            fitnessFactor = WeaponFactor + PowerFactor - LazyFactor;
            return fitnessFactor;
        }

        private float CalculateWeaponFactor(Individual individual)
        {
            float weaponFactor = 0.0f;
            weaponFactor += individual.Weapon.WeaponStatus1 * 0.5f;
            return weaponFactor;
        }

        private float CalculatePowerFactor(Individual individual)
        {
            float powerFactor = 0.0f;
            powerFactor += individual.Enemy.Status1 * 0.6f;
            powerFactor += individual.Enemy.Status2 * 0.4f;
            powerFactor += individual.Enemy.Status3 * 0.3f;
            return powerFactor;
        }

        private float CalculateLazynessFactor(Individual individual)
        {
            float lazyFactor = 0.0f;
            lazyFactor += individual.Enemy.Status4 * 0.7f;
            lazyFactor += individual.Enemy.Status5 * 0.3f;
            lazyFactor += individual.Enemy.Status6 * 0.2f;
            return lazyFactor;
        }
    }
}