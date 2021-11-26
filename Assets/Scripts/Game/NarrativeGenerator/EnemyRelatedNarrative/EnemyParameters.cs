using System.Collections.Generic;
using System.Text;
using Game.NarrativeGenerator.Quests;
using UnityEngine;

namespace Game.NarrativeGenerator
{
    [CreateAssetMenu(menuName = "NarrativeComponents/Enemies")]
    public class EnemyParameters : ScriptableObject
    {
        private int nEnemies;
        private SortedList<float, float> percentageByDifficulty;

        public int NEnemies { get => nEnemies; set => nEnemies = value; }
        public SortedList<float, float> PercentageByDifficulty { get => percentageByDifficulty; set => percentageByDifficulty = value; }

        public EnemyParameters()
        {
            NEnemies = 0;
            percentageByDifficulty = new SortedList<float, float>();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (KeyValuePair<float, float> kvp in percentageByDifficulty)
            {
                stringBuilder.Append($"Difficulty = {kvp.Key}, Percentage = {kvp.Value}\n");
            }
            return stringBuilder.ToString();
        }
    }
}
