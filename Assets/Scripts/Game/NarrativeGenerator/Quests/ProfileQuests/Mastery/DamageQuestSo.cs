using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;
using System.Linq;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class DamageQuestSo : MasteryQuestSo
    {
        [field: SerializeField]
        public EnemiesByType EnemiesToDamageByType { get; set; }
        public Dictionary<float, int> EnemiesToDamageByFitness { get; set; }
        public override string symbolType {
            get { return Constants.DAMAGE_QUEST; }
        }

        public override void Init()
        {
            base.Init();
            EnemiesToDamageByType = new EnemiesByType ();
            EnemiesToDamageByFitness = new Dictionary<float, int>();
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, EnemiesByType  enemiesByType )
        {
            base.Init(questName, endsStoryLine, previous);
            EnemiesToDamageByType = enemiesByType;
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, Dictionary<float, int> enemiesByFitness )
        {
            base.Init(questName, endsStoryLine, previous);
            EnemiesToDamageByFitness = enemiesByFitness;
        }
        
        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            EnemiesToDamageByType = new EnemiesByType ();
            foreach (var enemyByType in (copiedQuest as DamageQuestSo).EnemiesToDamageByType.EnemiesByTypeDictionary)
            {
                EnemiesToDamageByType.EnemiesByTypeDictionary.Add(enemyByType.Key, enemyByType.Value);
            }
        }
        
        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<DamageQuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }
        
        public void AddEnemy(WeaponTypeSO enemy, int amount)
        {
            if (EnemiesToDamageByType.EnemiesByTypeDictionary.TryGetValue(enemy, out var currentAmount))
            {
                EnemiesToDamageByType.EnemiesByTypeDictionary[enemy] = currentAmount + amount;
            }
            else
            {
                EnemiesToDamageByType.EnemiesByTypeDictionary.Add(enemy, amount);
            }
        }
        
        public void AddEnemy(float enemyFitness, int amount)
        {
            if (EnemiesToDamageByFitness.TryGetValue(enemyFitness, out var currentAmount))
            {
                EnemiesToDamageByFitness[enemyFitness] = currentAmount + amount;
            }
            else
            {
                EnemiesToDamageByFitness.Add(enemyFitness, amount);
            }
        }

        public void SubtractDamage(WeaponTypeSO weaponTypeSo, int damage )
        {
            EnemiesToDamageByType.EnemiesByTypeDictionary[weaponTypeSo] -= damage;
        }

        public bool CheckIfCompleted()
        {
            return EnemiesToDamageByType.EnemiesByTypeDictionary.All(enemyDamage => enemyDamage.Value <= 0);;
        }

        public bool HasEnemyToDamage (WeaponTypeSO weaponTypeSo)
        {
            if (!EnemiesToDamageByType.EnemiesByTypeDictionary.TryGetValue(weaponTypeSo, out var damageLeft)) return false;
            return damageLeft > 0;
        }
    }
}