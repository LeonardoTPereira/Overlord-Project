using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Overlord.RulesGenerator.EnemyGeneration;

[CreateAssetMenu(fileName = "GENERIC List of Movements", menuName = "Overlord-Project/Enemy/GenericEnemyMovementListSO")]
public class GenericEnemyMovementsSO : EnemyMovementsSO<GenericEnemyMovementsSO.MovementTypeEnums>
{    public enum MovementTypeEnums
    {
        Type1, Type2, Type3, Type4, Type5, Type6, Type7
    }
    /*
    public override List<Enum> GetAllMovementTypes()
    {
        return ((GenericEnemyMovementsSO.MovementTypeEnums[])Enum.GetValues(typeof(GenericEnemyMovementsSO.MovementTypeEnums))).Cast<Enum>().ToList();
    }

    public override List<Enum> GetHealerMovementList()
    {
        return new List<Enum>();
    }
    */
}
