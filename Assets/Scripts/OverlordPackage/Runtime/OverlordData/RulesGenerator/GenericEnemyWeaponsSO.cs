using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overlord.RulesGenerator.EnemyGeneration;

[CreateAssetMenu(fileName = "Generic List of Enemy Weapons", menuName = "Overlord-Project/Rules-Generator/GenericEnemyWeaponListSO")]
public class GenericEnemyWeaponsSO : EnemyWeaponsSO<GenericEnemyWeaponsSO.WeaponTypeEnums>
{
    public enum WeaponTypeEnums
    {
        Type1, Type2, Type3, Type4, Type5, Type6
    }

    public override List<Enum> GetRangedWeaponTypes()
    {
        return new List<Enum>();
    }

    public override List<Enum> GetMeleeWeaponTypes()
    {
        return new List<Enum>();
    }
}
