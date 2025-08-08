using System;
using System.Collections.Generic;
using Overlord.RulesGenerator.EnemyGeneration;

namespace Game.EnemyGenerator
{
    /// This enum defines the types of weapons an enemy may have.
    [Serializable()]
    public enum WeaponType
    {
        Barehand,    // Enemy attacks the player with barehands (Melee).
        Sword,       // Enemy uses a short sword to damage the player (Melee).
        Bow,         // Enemy shots projectiles towards the player (Range).
        BombThrower, // Enemy shots bombs towards the player (Range).
        Shield,      // Enemy uses a shield to defend itself (Defense).
        CureSpell,   // Enemy uses magic to cure other enemies (Defense).
    }


    /// This class defines the search space of each attribute of enemies.
    ///
    /// The prefix `r` in the attributes' names of this class stands for `range
    /// of`, e.g., the rHealth means the range of health.
    ///
    /// Why the search space is represented by a class instead of a struct?
    /// Because the search space must be a singleton.
    public class SearchSpace
    {
        public (int, int) rHealth { get; }
        public (int, int) rStrength { get; }
        public (float, float) rAttackSpeed { get; }
        public List<EnemyMovementType> rMovementType { get; }
        public (float, float) rMovementSpeed { get; }
        public (float, float) rActiveTime { get; }
        public (float, float) rRestTime { get; }
        public WeaponType[] rWeaponType { get; }
        public (float, float) rProjectileSpeed { get; }

        /// Search Space constructor.
        private SearchSpace(
            (int, int) _rHealth,
            (int, int) _rStrength,
            (float, float) _rAttackSpeed,
            List<EnemyMovementType> _rMovementType,
            (float, float) _rMovementSpeed,
            (float, float) _rActiveTime,
            (float, float) _rRestTime,
            WeaponType[] _rWeaponType,
            (float, float) _rProjectileSpeed
        )
        {
            rHealth = _rHealth;
            rStrength = _rStrength;
            rAttackSpeed = _rAttackSpeed;
            rMovementType = _rMovementType;
            rMovementSpeed = _rMovementSpeed;
            rActiveTime = _rActiveTime;
            rRestTime = _rRestTime;
            rWeaponType = _rWeaponType;
            rProjectileSpeed = _rProjectileSpeed;
        }

        /// This variable holds the single instance of the Search Space.
        private static SearchSpace instance = null;

        /// Return the single instance of the Search Space.
        public static SearchSpace Instance
        {
            get
            {
                if (instance is null)
                {
                    instance = new SearchSpace(
                        (1, 6),                         // Health
                        (1, 4),                         // Strength
                        (0.75f, 4f),                    // Attack Speed
                        SearchSpace.GetAllMovementTypes(), // Movement Types
                        (0.8f, 3.2f),                   // Movement Speed
                        (1.5f, 10f),                    // Active Time
                        (0.3f, 1.5f),                   // Rest Time
                        SearchSpace.AllWeaponTypes(),   // Weapon Types
                        (1f, 4f)                        // Projectile Speed
                    );
                }
                return instance;
            }
        }


        /// Return the array of all movement types.
        public static List<EnemyMovementType> GetAllMovementTypes()
        {
            return Enum.GetValues(typeof(MovementType)).Cast<MovementType>().ToList();
            //return (MovementType[])Enum.GetValues(typeof(MovementType));
        }

        /// Return the list of all movement types.
        ///
        /// The healer ideally searches for other enemies and avoids the
        /// player, besides these movements in melee enemies do not present
        /// a clear risk to the player.
        public static List<MovementType> HealerMovementList()
        {
            return new List<MovementType> {
                MovementType.Random,
                MovementType.Random1D,
                MovementType.Flee,
                MovementType.Flee1D,
            };
        }


        /// Return the array of all weapon types.
        public static WeaponType[] AllWeaponTypes()
        {
            return (WeaponType[])Enum.GetValues(typeof(WeaponType));
        }

        /// Return the list of ranged weapon types.
        public static List<WeaponType> RangedWeaponList()
        {
            return new List<WeaponType> {
                WeaponType.Bow,
                WeaponType.BombThrower,
            };
        }

        /// Return the list of melee weapon types.
        public static List<WeaponType> MeleeWeaponList()
        {
            return new List<WeaponType> {
                WeaponType.Barehand,
                WeaponType.Sword,
                WeaponType.Shield,
            };
        }
    }
}