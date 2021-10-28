using UnityEngine;
using System.Collections.Generic;
using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests.QuestTerminals
{
    public class KillQuestSO : QuestSO
    {
        public Dictionary<EnemySO, int> EnemiesToKillByType { get; set; }
        
        public override void Init()
        {
            base.Init();
            EnemiesToKillByType = new Dictionary<EnemySO, int>();
        }

        public void Init(string questName, bool endsStoryLine, QuestSO previous, Dictionary<EnemySO, int> enemiesByType)
        {
            base.Init(questName, endsStoryLine, previous);
            EnemiesToKillByType = enemiesByType;
        }
        
        public void AddEnemy(EnemySO enemy, int amount)
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
    }
}
