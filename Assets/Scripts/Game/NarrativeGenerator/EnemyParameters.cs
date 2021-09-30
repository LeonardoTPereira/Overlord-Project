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
            StringBuilder stringBuilder = new();
            foreach (KeyValuePair<float, float> kvp in percentageByDifficulty)
            {
                stringBuilder.Append($"Difficulty = {kvp.Key}, Percentage = {kvp.Value}\n");
            }
            return stringBuilder.ToString();
        }

        public void ConversorMonster(EnemyParameters enemyParameters, QuestList quests)
        {
            for (int i = 0; i < quests.graph.Count; i++)
            {
                if (quests.graph[i].Tipo == 2 || quests.graph[i].Tipo == 5)
                {

                    enemyParameters.PercentageByDifficulty.TryGetValue(quests.graph[i].N1)
                    enemyParameters.PercentageType1 += quests.graph[i].N1; //significados de n1, n2, n3 e tipo no script "Quest", favor verificar
                    enemyParameters.PercentageType2 += quests.graph[i].N2;
                    enemyParameters.PercentageType3 += quests.graph[i].N3;

                    if (enemyParameters.PercentageType1 >= 5) enemyParameters.FitnessType1 += Random.Range(10, 21);
                    else enemyParameters.FitnessType1 += Random.Range(1, 10);

                    if (enemyParameters.PercentageType2 >= 5) enemyParameters.FitnessType2 += Random.Range(5, 21);
                    else enemyParameters.FitnessType2 += Random.Range(1, 5);

                    if (enemyParameters.PercentageType3 >= 5) enemyParameters.FitnessType3 += Random.Range(1, 10);
                    else enemyParameters.FitnessType3 += Random.Range(10, 21);

                    enemyParameters.NEnemies += 2;
                }
            }
        }
    }
}
