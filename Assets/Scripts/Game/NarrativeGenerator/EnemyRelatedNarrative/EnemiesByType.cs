using System;
using System.Collections.Generic;
using System.Linq;
using Game.EnemyManager;
using Game.GameManager;
using Game.NarrativeGenerator.Quests;
using MyBox;
using ScriptableObjects;
using UnityEngine;

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

        public int Count()
        {
            if (EnemiesByTypeDictionary == null)
            {
                return 0;
            }
            return EnemiesByTypeDictionary.Count;
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

        public KeyValuePair<WeaponTypeSO, QuestIdList> GetRandom()
        {
            return EnemiesByTypeDictionary.GetRandom();
        }
        
        public void AddNEnemiesFromType(KeyValuePair<WeaponTypeSO, QuestIdList> selectedType, int newEnemies)
        {
            var weaponType = selectedType.Key;
            if (!EnemiesByTypeDictionary.ContainsKey(weaponType))
            {
                EnemiesByTypeDictionary.Add(weaponType, new QuestIdList());
            }
            for (var i = 0; i < newEnemies; i++)
            {
                EnemiesByTypeDictionary[weaponType].Add(selectedType.Value.QuestIds.First());
                selectedType.Value.QuestIds.RemoveAt(0);
            }
        }
        
        public void RemoveCurrentTypeIfEmpty(WeaponTypeSO selectedType)
        {
            if (EnemiesByTypeDictionary.Count == 0)
                throw new ArgumentException($"Enemies in Quest cannot be an empty collection. " +
                                            $"{nameof(EnemiesByTypeDictionary)}");
            if (EnemiesByTypeDictionary[selectedType].QuestIds.Count <= 0)
            {
                EnemiesByTypeDictionary.Remove(selectedType);
            }
        }

        public EnemyByAmountDictionary GetEnemiesForRoom()
        {
            var enemiesBySo = new EnemyByAmountDictionary();
            foreach (var enemyType in EnemiesByTypeDictionary)
            {
                foreach (var questId in enemyType.Value.QuestIds)
                {
                    var selectedEnemy = EnemyLoader.GetRandomEnemyOfType(enemyType.Key);
                    if (!enemiesBySo.ContainsKey(selectedEnemy))
                    {
                        var questIdList = new QuestIdList();
                        enemiesBySo.Add(selectedEnemy, questIdList);
                    }
                    enemiesBySo[selectedEnemy].Add(questId);
                }
            }
            return enemiesBySo;
        }
        
        public bool TryAddHealer(ref EnemiesByType enemies)
        {
            foreach( var enemy in enemies.EnemiesByTypeDictionary)
            {
                if (!enemy.Key.IsHealer() || enemy.Value.QuestIds.Count <= 0) continue;
                AddNEnemiesFromType(enemy, 1);
                enemies.RemoveCurrentTypeIfEmpty(enemy.Key);
                return true;
            }
            return false;
        }
        
        public bool TryAddAttacker(ref EnemiesByType enemies)
        {
            foreach( var enemy in enemies.EnemiesByTypeDictionary)
            {
                if (!enemy.Key.IsRanger() && !enemy.Key.IsMelee()) continue;
                if (enemy.Value.QuestIds.Count <= 0) continue;
                AddNEnemiesFromType(enemy, 1);
                return true;
            }
            return false;
        }

        public void RemoveEnemyWithId(WeaponTypeSO weaponTypeSo, int questId)
        {
            EnemiesByTypeDictionary[weaponTypeSo].QuestIds.Remove(questId);
        }
    }
}