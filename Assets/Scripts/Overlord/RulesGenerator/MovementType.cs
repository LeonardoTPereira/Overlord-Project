using System;
using System.Collections.Generic;
using System.Linq;

namespace Overlord.RulesGenerator.EnemyGeneration
{
    public class EnemyMovementType
    {
        public enum MovementTypeEnums
        {
            Type1, // Enemy stays still.
            Type2, // Enemy performs random 2D movements.
            Type3, // Enemy follows the player.
            Type4, // Enemy flees from the player.
            Type5, // Enemy performs random horizontal or vertical movements.
            Type6, // Enemy follows the player horizontally or vertically.
            Type7, // Enemy flees from the player horizontally or vertically.
        }
        
        private int _movementIndex = -1;
        

        public int GetMovementIndex()
        {
            return _movementIndex;
        }

        public void SetMovementIndex(int movementIndex)
        {
            _movementIndex = movementIndex;
        }
        
        public static List<MovementTypeEnums> AllMovementTypes()
        {
            return Enum.GetValues(typeof(MovementTypeEnums))
                .Cast<MovementTypeEnums>()
                .ToList();
        }
    }

    public class TopdownMovementType : EnemyMovementType
    {
        public enum MovementTypeEnums
        {
            None, // Enemy stays still.
            Random, // Enemy performs random 2D movements.
            Follow, // Enemy follows the player.
            Flee, // Enemy flees from the player.
            Random1D, // Enemy performs random horizontal or vertical movements.
            Follow1D, // Enemy follows the player horizontally or vertically.
            Flee1D, // Enemy flees from the player horizontally or vertically.
        }

        public static List<MovementTypeEnums> GetHealerMovementList()
        {
            return new List<MovementTypeEnums> {
                MovementTypeEnums.Random,
                MovementTypeEnums.Random1D,
                MovementTypeEnums.Flee,
                MovementTypeEnums.Flee1D,
            };
        }
    }
    
    public class PlatformerMovementType : EnemyMovementType
    {
        public enum MovementTypeEnums
        {
            NoMovement,
            Patrol,
            CooldownPatrol,
            Jumper,
            Flee1D,
            Follow1D,
            JumperUp,
        }

    }

}