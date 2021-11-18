using UnityEngine;
using System.Collections.Generic;
using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests.QuestTerminals
{
    public class KillQuestSO : QuestSO
    {
        public Dictionary<WeaponTypeSO, int> EnemiesToKillByType { get; set; }
        public Dictionary<float, int> EnemiesToKillByFitness { get; set; }
        
        public override void Init()
        {
            base.Init();
            EnemiesToKillByType = new Dictionary<WeaponTypeSO, int>();
            EnemiesToKillByFitness = new Dictionary<float, int>();
        }

        public void Init(string questName, bool endsStoryLine, QuestSO previous, Dictionary<WeaponTypeSO, int> enemiesByType)
        {
            base.Init(questName, endsStoryLine, previous);
            EnemiesToKillByType = enemiesByType;
        }
        public void Init(string questName, bool endsStoryLine, QuestSO previous, Dictionary<float, int> enemiesByFitness)
        {
            base.Init(questName, endsStoryLine, previous);
            EnemiesToKillByFitness = enemiesByFitness;
        }
        
        public void AddEnemy(WeaponTypeSO enemy, int amount)
        {
            if (EnemiesToKillByType.TryGetValue(enemy, out var currentAmount))
            {
                EnemiesToKillByType[enemy] = currentAmount + amount;
            }
            else
            {
                EnemiesToKillByType.Add(enemy, amount);
            }
        }
        
        public void AddEnemy(float enemyFitness, int amount)
        {
            if (EnemiesToKillByFitness.TryGetValue(enemyFitness, out var currentAmount))
            {
                EnemiesToKillByFitness[enemyFitness] = currentAmount + amount;
            }
            else
            {
                EnemiesToKillByFitness.Add(enemyFitness, amount);
            }
        }
    }
}
