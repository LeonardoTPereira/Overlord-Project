using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyWeaponsSOInterface : ScriptableObject
{
    public abstract int GetMappedIndex(Enum weapon);
    public abstract string GetWeaponName(int index);
    public abstract Enum GetEnemyWeaponByIndex(int index);
    public abstract int GetEnemyWeaponCount();
    public abstract List<Enum> GetAllWeaponTypes();
    public abstract List<Enum> GetRangedWeaponTypes();
    public abstract List<Enum> GetMeleeWeaponTypes();
}
