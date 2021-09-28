using System.Collections.Generic;
using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests.QuestTerminals
{
    public class KillQuestSO : QuestSO
    {
        Dictionary<EnemySO, int> EnemiesToKillByType;
        
        public void AddItem(EnemySO enemy, int amount)
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
