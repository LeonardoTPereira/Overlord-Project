using System;
using Util;

namespace Overlord.RulesGenerator.EnemyGeneration
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
        public int MovementIndex { get; set; }
        public WeaponData Weapon { get; }
        public int WeaponIndex { get; set; }
        public float DifficultyLevel { get; set; }
        public float FitnessValue { get; set; }
        public int Generation { get; set; }

        /// Individual contructor.
        public Individual(
            EnemyData enemy,
            WeaponData weapon
        )
        {
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
            UnityEngine.Debug.Log("  He=" + Enemy.Status1);
            UnityEngine.Debug.Log("  St=" + Enemy.Status2);
            UnityEngine.Debug.Log("  AS=" + Enemy.Status3);
            UnityEngine.Debug.Log("  MT=" + Enemy.Movement);
            UnityEngine.Debug.Log("  MS=" + Enemy.Status4);
            UnityEngine.Debug.Log("  AT=" + Enemy.Status5);
            UnityEngine.Debug.Log("  RT=" + Enemy.Status6);
            UnityEngine.Debug.Log("  WT=" + Weapon.Weapon);
            UnityEngine.Debug.Log("  PS=" + Weapon.WeaponStatus1);
            UnityEngine.Debug.Log("");
        }

        /// Return a random individual. 
        /// The name of the variables (health, strength, etc.) are related to other games, 
        /// so you can consider them as generic attributes (stat1, stat2, etc.).
        public static Individual GetRandom(SearchSpaceConfig searchSpace)
        {
            // Create a random enemy
            var (min, max) = (searchSpace.Status1.Min, searchSpace.Status1.Max);
            var health = RandomSingleton.GetInstance().Next(min, max);
            (min, max) = (searchSpace.Status2.Min, searchSpace.Status2.Max);
            var strength = RandomSingleton.GetInstance().Next(min, max);
            var (minFloat, maxFloat) = (searchSpace.Status3.Min, searchSpace.Status3.Max);
            var attackSpeed = RandomSingleton.GetInstance().Next(minFloat, maxFloat);
            // Create a random weapon
            var weaponType = RandomSingleton.GetInstance().RandomElementFromList<Enum>(searchSpace.WeaponSet.GetAllWeaponTypes());
            var movementType = RandomSingleton.GetInstance().RandomElementFromList<Enum>(searchSpace.MovementSet.GetAllMovementTypes()); //List<Enum>
            (minFloat, maxFloat) = (searchSpace.Status4.Min, searchSpace.Status4.Max);
            var movementSpeed = RandomSingleton.GetInstance().Next(minFloat, maxFloat);
            (minFloat, maxFloat) = (searchSpace.Status5.Min, searchSpace.Status5.Max);
            var activeTime = RandomSingleton.GetInstance().Next(minFloat, maxFloat);
            (minFloat, maxFloat) = (searchSpace.Status6.Min, searchSpace.Status6.Max);
            var restTime = RandomSingleton.GetInstance().Next(minFloat, maxFloat);
            EnemyData e = new EnemyData(health, strength, attackSpeed, movementType, movementSpeed, activeTime, restTime);
            (minFloat, maxFloat) = (searchSpace.WeaponStatus1.Min, searchSpace.WeaponStatus1.Max);
            var projectileSpeed = RandomSingleton.GetInstance().Next(minFloat, maxFloat);
            WeaponData w = new WeaponData(weaponType, projectileSpeed);
            // Combine the genes to create a new individual
            Individual individual = new Individual(e, w);
            individual.MovementIndex = searchSpace.MovementSet.GetMappedIndex(movementType);
            individual.WeaponIndex = searchSpace.WeaponSet.GetMappedIndex(weaponType);
            individual.DifficultyLevel = -1;
            individual.Generation = -1;
            individual.FitnessValue = -1;
            return individual;
        }
    }

    /// This struct represents an enemy.
    [Serializable]
    public struct EnemyData
    {
        public float Status1 { get; set; }      // Old name: Health
        public float Status2 { get; set; }      // Old name: Strength
        public float Status3 { get; set; }      // Old name: AttackSpeed
        public Enum Movement { get; set; }      // Old name: MovementType
        public float Status4 { get; set; }      // Old name: MovementSpeed
        public float Status5 { get; set; }      // Old name: ActiveTime
        public float Status6 { get; set; }      // Old name: RestTime

        /// Enemy contructor.
        public EnemyData(
            float health,
            float strength,
            float attackSpeed,
            Enum movement,
            float movementSpeed,
            float activeTime,
            float restTime
        )
        {
            Status1 = health;
            Status2 = strength;
            Status3 = attackSpeed;
            Movement = movement;
            Status4 = movementSpeed;
            Status5 = activeTime;
            Status6 = restTime;
        }
    }

    /// This struc represents a weapon.
    [Serializable]
    public struct WeaponData
    {
        public Enum Weapon { get; set; }
        public float WeaponStatus1 { get; set; }    // Old name: ProjectileSpeed

        /// Weapon constructor.
        public WeaponData(
            Enum weapon,
            float projectileSpeed
        )
        {
            Weapon = weapon;
            WeaponStatus1 = projectileSpeed;
        }
    }
}