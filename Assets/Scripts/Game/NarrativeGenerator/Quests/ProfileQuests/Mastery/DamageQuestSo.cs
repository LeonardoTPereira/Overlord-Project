using System;
using ScriptableObjects;
using Util;
using UnityEngine;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class DamageQuestSo : MasteryQuestSo
    {
        [field: SerializeField] private DamageQuestData DamageData { get; set; }
        public override string SymbolType => Constants.DAMAGE_QUEST;

        public override void Init()
        {
            base.Init();
            DamageData = new DamageQuestData();
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, WeaponTypeSO enemyToDamage, int damage)
        {
            base.Init(questName, endsStoryLine, previous);
            DamageData = new DamageQuestData(damage, enemyToDamage);
        }

        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            var damageQuest = copiedQuest as DamageQuestSo;
            if (damageQuest != null)
            {
                DamageData = new DamageQuestData(damageQuest.DamageData);
            }
            else
            {
                throw new ArgumentException(
                    $"Expected argument of type {typeof(DamageQuestSo)}, got type {copiedQuest.GetType()}");
            }
        }
        
        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<DamageQuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            return !IsCompleted 
                   && DamageData.Enemy == (questElement as DamageQuestData)?.Enemy;        
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            if (questElement is not DamageQuestData damageData) return;
            DamageData.Damage -= damageData.Damage;
            if (DamageData.Damage <= 0)
            {
                IsCompleted = true;
            }
        }
    }
}