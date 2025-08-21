using System;
using System.Collections.Generic;
using UnityEngine;
using Overlord.RulesGenerator.EnemyGeneration;

[CreateAssetMenu(fileName = "Topdown List of Enemy Weapons", menuName = "Overlord-Project/Rules-Generator/Weapons/TopdownEnemyWeaponListSO")]
public class TopdownEnemyWeaponsSO : EnemyWeaponsSO<TopdownEnemyWeaponsSO.WeaponTypeEnums>
{
    public enum WeaponTypeEnums
    {
        None,       // No weapon
        Bow,        // Ranged weapon
        BombThrower,// Ranged weapon
        Barehand,   // Melee weapon
        Sword,      // Melee weapon
        Shield      // Melee weapon
    }
    public override List<Enum> GetRangedWeaponTypes()
    {
        return new List<Enum>
        {
            WeaponTypeEnums.Bow,
            WeaponTypeEnums.BombThrower,
        };
    }
    public override List<Enum> GetMeleeWeaponTypes()
    {
        return new List<Enum>
        {
            WeaponTypeEnums.Barehand,
            WeaponTypeEnums.Sword,
            WeaponTypeEnums.Shield,
        };
    }
}
