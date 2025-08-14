using Overlord.GenerationController.Facade;
using Overlord.RulesGenerator.EnemyGeneration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
        public List<Enum> rMovementType { get; }
        public (float, float) rMovementSpeed { get; }
        public (float, float) rActiveTime { get; }
        public (float, float) rRestTime { get; }
        public WeaponType[] rWeaponType { get; }
        public (float, float) rProjectileSpeed { get; }

        private EnemyMovementType _movementType;

        /// Search Space constructor.
        private SearchSpace(
            (int, int) _rHealth,
            (int, int) _rStrength,
            (float, float) _rAttackSpeed,
            List<Enum> _rMovementType,
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

        private static SearchSpace instance = null;
        private RulesGeneratorFacade _rulesFacade = RulesGeneratorFacade.Instance;

        /// Return the single instance of the Search Space.
        public static SearchSpace Instance
        {
            get
            {
                if (instance is null)
                {
                    List<Enum> listOfMovementsEnum = RulesGeneratorFacade.Instance.GetEnemyMovementType().GetAllMovementTypes();
                    //SearchSpace.PrintEnumList(listOfMovementsEnum);
                    
                    instance = new SearchSpace(
                        (1, 6),                         // Health
                        (1, 4),                         // Strength
                        (0.75f, 4f),                    // Attack Speed
                        listOfMovementsEnum,            // Movement Types
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
            // Join all enum names into one string separated by commas
            string joined = string.Join(", ", enumList.Select(e => e.ToString()));
            UnityEngine.Debug.Log("Enum list contents: " + joined);
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