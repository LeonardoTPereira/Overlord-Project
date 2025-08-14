using System;
using System.Collections.Generic;
using System.Linq;

namespace Overlord.RulesGenerator.EnemyGeneration
{
    public abstract class EnemyMovementType
    {
        private int _movementIndex = -1;

        public enum MovementTypeEnums
        {
            Type1,Type2, Type3, Type4, Type5, Type6, Type7
        }

        public int GetMovementIndex()
        {
            return _movementIndex;
        }

        public void SetMovementIndex(int movementIndex)
        {
            _movementIndex = movementIndex;
        }

        public string GetMovementName(int movementIndex)
        {
            if (movementIndex < 0 || movementIndex >= Enum.GetValues(typeof(MovementTypeEnums)).Length)
                return string.Empty;

            return ((MovementTypeEnums) movementIndex).ToString();
        }

        public virtual List<Enum> GetAllMovementTypes()
        {
            return new List<Enum>();
        }

        public virtual List<Enum> GetHealerMovementList()
        {
            return new List<Enum>();
        }
    }

    public class TopdownMovementType : EnemyMovementType
    {
        public new enum MovementTypeEnums
        {
            None,       // Enemy stays still
            Random,     // Random 2D movements
            Follow,     // Follows the player
            Flee,       // Flees from the player
            Random1D,   // Random horizontal/vertical
            Follow1D,   // Follows horizontally/vertically
            Flee1D      // Flees horizontally/vertically
        }

        public override List<Enum> GetAllMovementTypes()
        {
            return Enum.GetValues(typeof(MovementTypeEnums))
                       .Cast<Enum>()
                       .ToList();
        }

        /// By Breno:
        /// Return the list of all movement types. 
        ///
        /// The healer ideally searches for other enemies and avoids the
        /// player, besides these movements in melee enemies do not present
        /// a clear risk to the player.
        public override List<Enum> GetHealerMovementList()
        {
            return new List<Enum>
            {
                MovementTypeEnums.Random,
                MovementTypeEnums.Random1D,
                MovementTypeEnums.Flee,
                MovementTypeEnums.Flee1D
            };
        }
    }

    public class PlatformerMovementType : EnemyMovementType
    {
        public new enum MovementTypeEnums
        {
            NoMovement,
            Patrol,
            CooldownPatrol,
            Jumper,
            Flee1D,
            Follow1D,
            JumperUp
        }

        public override List<Enum> GetAllMovementTypes()
        {
            return Enum.GetValues(typeof(MovementTypeEnums))
                       .Cast<Enum>()
                       .ToList();
        }

        public override List<Enum> GetHealerMovementList()
        {
            // Colocar movimentos de healer aqui, se existirem
            return new List<Enum>();
        }
    }
}