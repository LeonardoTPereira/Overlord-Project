using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Overlord.RulesGenerator.EnemyGeneration
{
    public class EnemyWeaponsSO<TEnum> : EnemyWeaponsSOInterface where TEnum : Enum
    {
        [SerializeField] public List<TEnum> _enemyWeapons;
        private Dictionary<Enum, int> _weaponIndexMap;

        public override int GetMappedIndex(Enum weapon)
        {
            if (_weaponIndexMap == null)
            {
                _weaponIndexMap = new Dictionary<Enum, int>();
                for (int i = 0; i < _enemyWeapons.Count; i++)
                {
                    _weaponIndexMap[_enemyWeapons[i]] = i;
                }
            }

            if (_weaponIndexMap.TryGetValue(weapon, out int index))
            {
                return index;
            }

            throw new ArgumentException($"Arma '{weapon}' não encontrado na lista de armas do inimigo.");
        }

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