using System;
using System.Collections.Generic;
using UnityEngine;
using Overlord.RulesGenerator.EnemyGeneration;

[CreateAssetMenu(fileName = "Topdown List of Enemy Weapons", menuName = "Overlord-Project/Rules-Generator/Weapons/TopdownEnemyWeaponListSO")]
public class TopdownEnemyWeaponsSO : EnemyWeaponsSO<TopdownEnemyWeaponsSO.WeaponTypeEnums>
{
    public enum WeaponTypeEnums
    {
        Barehand,    // Enemy attacks the player with barehands (Melee).
        Sword,       // Enemy uses a short sword to damage the player (Melee).
        Bow,         // Enemy shots projectiles towards the player (Range).
        BombThrower, // Enemy shots bombs towards the player (Range).
        Shield,      // Enemy uses a shield to defend itself (Defense).
        CureSpell,   // Enemy uses magic to cure other enemies (Defense).
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

    public List<Enum> GetHealerWeaponTypes()
    {
        return new List<Enum>
        {
            WeaponTypeEnums.CureSpell,
        };
    }

    public bool IsHealerWeapon(Enum weapon)
    {
        return weapon.Equals(WeaponTypeEnums.CureSpell);
    }
}
