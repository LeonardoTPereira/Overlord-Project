using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Overlord.RulesGenerator.EnemyGeneration;

[CreateAssetMenu(fileName = "Topdown List of Movements", menuName = "Overlord-Project/Rules-Generator/TopdownEnemyMovementListSO")]
public class TopdownEnemyMovementsSO : EnemyMovementsSO<TopdownEnemyMovementsSO.MovementTypeEnums>
{
    public enum MovementTypeEnums
    {
        None,       // Enemy stays still
        Random,     // Random 2D movements
        Follow,     // Follows the player
        Flee,       // Flees from the player
        Random1D,   // Random horizontal/vertical
        Follow1D,   // Follows horizontally/vertically
        Flee1D      // Flees horizontally/vertically
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