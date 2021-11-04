using System;

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
        public Enemy enemy;
        public Weapon weapon;
        public float difficulty;
        public float fitness;
        public int generation;

        /// Individual contructor.
        public Individual(
            Enemy _enemy,
            Weapon _weapon
        ) {
            enemy = _enemy;
            weapon = _weapon;
        }

        /// Return a clone of the individual.
        ///
        /// We create a new individual by passing `enemy` and `weapon` in the
        /// Individual constructor. Since both are structs, we can copy them by
        /// value instead of doing a deep copy.
        public Individual Clone()
        {
            Individual individual = new Individual(enemy, weapon);
            individual.difficulty = difficulty;
            individual.fitness = fitness;
            individual.generation = generation;
            return individual;
        }

        /// Print the individual attributes.
        public void Debug()
        {
            Console.WriteLine("  G=" + generation);
            Console.WriteLine("  F=" + fitness);
            Console.WriteLine("  D=" + difficulty);
            Console.WriteLine("  He=" + enemy.health);
            Console.WriteLine("  St=" + enemy.strength);
            Console.WriteLine("  AS=" + enemy.attackSpeed);
            Console.WriteLine("  MT=" + enemy.movementType);
            Console.WriteLine("  MS=" + enemy.movementSpeed);
            Console.WriteLine("  AT=" + enemy.activeTime);
            Console.WriteLine("  RT=" + enemy.restTime);
            Console.WriteLine("  WT=" + weapon.weaponType);
            Console.WriteLine("  PS=" + weapon.projectileSpeed);
            Console.WriteLine();
        }

        /// Return a random individual.
        public static Individual GetRandom(
            ref Random _rand
        ) {
            SearchSpace ss = SearchSpace.Instance;
            // Create a random enemy
            Enemy e = new Enemy(
                Common.RandomInt(ss.rHealth, ref _rand),
                Common.RandomInt(ss.rStrength, ref _rand),
                Common.RandomFloat(ss.rAttackSpeed, ref _rand),
                Common.RandomElementFromArray(ss.rMovementType, ref _rand),
                Common.RandomFloat(ss.rMovementSpeed, ref _rand),
                Common.RandomFloat(ss.rActiveTime, ref _rand),
                Common.RandomFloat(ss.rRestTime, ref _rand)
            );
            // Create a random weapon
            Weapon w = new Weapon(
                Common.RandomElementFromArray(ss.rWeaponType, ref _rand),
                Common.RandomFloat(ss.rProjectileSpeed, ref _rand)
            );
            // Combine the genes to create a new individual
            Individual individual = new Individual(e, w);
            individual.difficulty = Common.UNKNOWN;
            individual.generation = Common.UNKNOWN;
            individual.fitness = Common.UNKNOWN;
            return individual;
        }
    }

    /// This struct represents an enemy.
    [Serializable]
    public struct Enemy
    {
        public int health;
        public int strength;
        public float attackSpeed;
        public MovementType movementType;
        public float movementSpeed;
        public float activeTime;
        public float restTime;

        /// Enemy contructor.
        public Enemy(
            int _health,
            int _strength,
            float _attackSpeed,
            MovementType _movementType,
            float _movementSpeed,
            float _activeTime,
            float _restTime
        ) {
            health = _health;
            strength = _strength;
            attackSpeed = _attackSpeed;
            movementType = _movementType;
            movementSpeed = _movementSpeed;
            activeTime = _activeTime;
            restTime = _restTime;
        }
    }

    /// This struc represents a weapon.
    [Serializable]
    public struct Weapon
    {
        public WeaponType weaponType;
        public float projectileSpeed;

        /// Weapon constructor.
        public Weapon(
            WeaponType _weaponType,
            float _projectileSpeed
        ) {
            weaponType = _weaponType;
            projectileSpeed = _projectileSpeed;
        }
    }
}