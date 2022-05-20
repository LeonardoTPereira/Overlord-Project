using System;
using Util;

namespace Game.EnemyGenerator
{
    /// This class represents an individual.
    ///
    /// Individuals are composed of an enemy, a weapon, their fitness value,
    /// their difficulty degree, and the generation when they were created.
    /// These attributes are the most common variables from enemies in
    /// different games.
    ///
    /// Why individuals are represented by a class instead of a struct? When
    /// using MAP-Elites some slots may be empty, then the `null` option makes
    /// easier to manage the MAP-Elites population.
    public class Individual
    {
        public EnemyData Enemy { get; }
        public WeaponData Weapon { get; }
        public float DifficultyLevel { get; set; }
        public float FitnessValue { get; set; }
        public int Generation { get; set; }

        /// Individual contructor.
        public Individual(
            EnemyData enemy,
            WeaponData weapon
        ) {
            Enemy = enemy;
            Weapon = weapon;
        }

        /// Return a clone of the individual.
        ///
        /// We create a new individual by passing `enemy` and `weapon` in the
        /// Individual constructor. Since both are structs, we can copy them by
        /// value instead of doing a deep copy.
        public Individual Clone()
        {
            Individual individual = new Individual(Enemy, Weapon);
            individual.DifficultyLevel = DifficultyLevel;
            individual.FitnessValue = FitnessValue;
            individual.Generation = Generation;
            return individual;
        }

        /// Print the individual attributes.
        public void Debug()
        {
            UnityEngine.Debug.Log("  G=" + Generation);
            UnityEngine.Debug.Log("  F=" + FitnessValue);
            UnityEngine.Debug.Log("  D=" + DifficultyLevel);
            UnityEngine.Debug.Log("  He=" + Enemy.Health);
            UnityEngine.Debug.Log("  St=" + Enemy.Strength);
            UnityEngine.Debug.Log("  AS=" + Enemy.AttackSpeed);
            UnityEngine.Debug.Log("  MT=" + Enemy.Movement);
            UnityEngine.Debug.Log("  MS=" + Enemy.MovementSpeed);
            UnityEngine.Debug.Log("  AT=" + Enemy.ActiveTime);
            UnityEngine.Debug.Log("  RT=" + Enemy.RestTime);
            UnityEngine.Debug.Log("  WT=" + Weapon.Weapon);
            UnityEngine.Debug.Log("  PS=" + Weapon.ProjectileSpeed);
            UnityEngine.Debug.Log("");
        }

        /// Return a random individual.
        public static Individual GetRandom() {
            SearchSpace ss = SearchSpace.Instance;
            // Create a random enemy
            var (min, max) = ss.rHealth;
            var health = RandomSingleton.GetInstance().Next(min, max + 1);
            (min, max) = ss.rStrength;
            var strength = RandomSingleton.GetInstance().Next(min, max + 1);
            var (minFloat, maxFloat) = ss.rAttackSpeed;
            var attackSpeed = RandomSingleton.GetInstance().Next(minFloat, maxFloat);
            var movementType = RandomSingleton.GetInstance().RandomElementFromArray(ss.rMovementType);
            (minFloat, maxFloat) = ss.rMovementSpeed;
            var movementSpeed = RandomSingleton.GetInstance().Next(minFloat, maxFloat);
            (minFloat, maxFloat) = ss.rActiveTime;
            var activeTime = RandomSingleton.GetInstance().Next(minFloat, maxFloat);
            (minFloat, maxFloat) = ss.rRestTime;
            var restTime = RandomSingleton.GetInstance().Next(minFloat, maxFloat);
            EnemyData e = new EnemyData(health, strength, attackSpeed, movementType, movementSpeed, activeTime, restTime);
            // Create a random weapon
            var weaponType = RandomSingleton.GetInstance().RandomElementFromArray(ss.rWeaponType);
            (minFloat, maxFloat) = ss.rProjectileSpeed;
            var projectileSpeed = RandomSingleton.GetInstance().Next(minFloat, maxFloat);
            WeaponData w = new WeaponData(weaponType, projectileSpeed);
            // Combine the genes to create a new individual
            Individual individual = new Individual(e, w);
            individual.DifficultyLevel = Common.UNKNOWN;
            individual.Generation = Common.UNKNOWN;
            individual.FitnessValue = Common.UNKNOWN;
            return individual;
        }
    }

    /// This struct represents an enemy.
    [Serializable]
    public struct EnemyData
    {
        public int Health { get; set; }
        public int Strength { get; set; }
        public float AttackSpeed { get; set; }
        public MovementType Movement { get; set; }
        public float MovementSpeed { get; set; }
        public float ActiveTime { get; set; }
        public float RestTime { get; set; }

        /// Enemy contructor.
        public EnemyData(
            int health,
            int strength,
            float attackSpeed,
            MovementType movement,
            float movementSpeed,
            float activeTime,
            float restTime
        ) {
            Health = health;
            Strength = strength;
            AttackSpeed = attackSpeed;
            Movement = movement;
            MovementSpeed = movementSpeed;
            ActiveTime = activeTime;
            RestTime = restTime;
        }
    }

    /// This struc represents a weapon.
    [Serializable]
    public struct WeaponData
    {
        public WeaponType Weapon { get; set; }
        public float ProjectileSpeed { get; set; }

        /// Weapon constructor.
        public WeaponData(
            WeaponType weapon,
            float projectileSpeed
        ) {
            Weapon = weapon;
            ProjectileSpeed = projectileSpeed;
        }
    }
}