using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overlord.RulesGenerator.EnemyGeneration;

[CreateAssetMenu(fileName = "Platformer List of Enemy Weapons", menuName = "Overlord-Project/Rules-Generator/PlatformerEnemyWeaponListSO")]
public class PlatformerEnemyWeaponsSO : EnemyWeaponsSO<PlatformerEnemyWeaponsSO.WeaponTypeEnums>
{
    public enum WeaponTypeEnums
    {
        Shadow,
        InfectedAnt,
        FuriousAnt,
        GrayWolf,
        BlackWolf
    }

    public override List<Enum> GetRangedWeaponTypes()
    {
        return new List<Enum>
        {
            WeaponTypeEnums.Shadow
        };
    }
    public override List<Enum> GetMeleeWeaponTypes()
    {
        return new List<Enum>
        {
            WeaponTypeEnums.InfectedAnt,
            WeaponTypeEnums.FuriousAnt,
            WeaponTypeEnums.GrayWolf,
            WeaponTypeEnums.BlackWolf
        };
    }
}
