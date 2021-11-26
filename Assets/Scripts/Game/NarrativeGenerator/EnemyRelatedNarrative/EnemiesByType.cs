using System;
using System.Collections.Generic;
using Game.Maestro;
using Game.NarrativeGenerator.Quests;
using MyBox;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.NarrativeGenerator.EnemyRelatedNarrative
{
    [Serializable]
    public class EnemiesByType
    {
        [SerializeField]
        private WeaponTypeAmountDictionary enemiesByType;
        public WeaponTypeAmountDictionary EnemiesByTypeDictionary
        {
            get => enemiesByType;
            set => enemiesByType = value;
        }

        public EnemiesByType()
        {
            EnemiesByTypeDictionary = new WeaponTypeAmountDictionary();
        }

        public EnemiesByType(EnemiesByType original)
        {
            EnemiesByTypeDictionary = new WeaponTypeAmountDictionary();
            foreach (var weaponTypeAmountPair in original.EnemiesByTypeDictionary)
            {
                EnemiesByTypeDictionary.Add(weaponTypeAmountPair.Key, weaponTypeAmountPair.Value);
            }
        }

        public KeyValuePair<WeaponTypeSO, int> GetRandom()
        {
            return EnemiesByTypeDictionary.GetRandom();
        }
    }
}