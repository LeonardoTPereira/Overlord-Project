using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Overlord.RulesGenerator.EnemyGeneration
{
    public class EnemyWeaponsSO<TEnum> : EnemyWeaponsSOInterface where TEnum : Enum
    {
        [SerializeField] public List<TEnum> _enemyWeapons;

        public override string GetWeaponName(int index)
        {
            if (index < 0 || index >= Enum.GetValues(typeof(TEnum)).Length)
                return string.Empty;

            return ((TEnum)(object)index).ToString();
        }

        public override Enum GetEnemyWeaponByIndex(int index)
        {
            if (index < 0 || index >= _enemyWeapons.Count)
                throw new IndexOutOfRangeException($"Movement index {index} is out of range.");
            return _enemyWeapons[index];
        }

        public override List<Enum> GetAllWeaponTypes()
        {
            //return ((TEnum[])Enum.GetValues(typeof(TEnum))).Cast<Enum>().ToList();
            return _enemyWeapons.Cast<Enum>().ToList();
        }

        public override int GetEnemyWeaponCount()
        {
            return _enemyWeapons.Count;
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
}