using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Overlord.RulesGenerator.EnemyGeneration;

[CreateAssetMenu(fileName = "GENERIC List of Movements", menuName = "Overlord-Project/Rules-Generator/GenericEnemyMovementListSO")]
public class GenericEnemyMovementsSO : EnemyMovementsSO<GenericEnemyMovementsSO.MovementTypeEnums>
{   
    public enum MovementTypeEnums
    {
        Type1, Type2, Type3, Type4, Type5, Type6, Type7
    }
}
