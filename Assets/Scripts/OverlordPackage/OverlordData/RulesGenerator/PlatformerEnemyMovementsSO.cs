using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Overlord.RulesGenerator.EnemyGeneration;

[CreateAssetMenu(fileName = "Platformer List of Enemy Movements", menuName = "Overlord-Project/Rules-Generator/PlatformerEnemyMovementListSO")]
public class PlatformerEnemyMovementsSO : EnemyMovementsSO<PlatformerEnemyMovementsSO.MovementTypeEnums>
{
    public enum MovementTypeEnums
    {
        NoMovement,
        Patrol,
        CooldownPatrol,
        Jumper,
        Flee1D,
        Follow1D,
        JumperUp
    }
    /*
    public override List<Enum> GetHealerMovementList()
    {
        // Colocar movimentos de healer aqui, se existirem
        return new List<Enum>();
    }
    */
}