﻿using System.Collections.Generic;
using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestTerminals;
using ScriptableObjects;
using UnityEngine;

namespace Game.NarrativeGenerator
{
    [Serializeable]
    public class QuestEnemiesParametersByDifficultyParameters : QuestEnemiesParameters
    {
        private SortedList<float, int> totalByFitness;
        
        public QuestEnemiesParametersByDifficultyParameters()
        {
            NEnemies = 0;
            totalByFitness = new SortedList<float, int>();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (KeyValuePair<float, int> kvp in totalByFitness)
            {
                stringBuilder.Append($"Enemy = {kvp.Key}, total = {kvp.Value}\n");
            }
            return stringBuilder.ToString();
        }

        public void CalculateMonsterFromQuests(QuestLine quests)
        {
            foreach (var quest in quests.graph)
            {
                AddEnemiesWhenEnemyQuest(quest);
            }
        }
        
        private void AddEnemiesWhenEnemyQuest(QuestSO quest)
        {
            if (quest.IsKillQuest())
            {
                AddEnemies((KillQuestSO) quest);
            }
            else if (quest.IsDropQuest())
            {
                AddEnemies((DropQuestSO) quest);
            }
        }

        private void AddEnemies(KillQuestSO quest)
        {
            foreach (var enemyAmountPair in quest.EnemiesToKillByFitness)
            {
                AddEnemiesToDictionary(enemyAmountPair);
            }
        }
        
        /*
         * TODO the enemies on drop quests could be the same from the killEnemies quest. We can try to check overlaps
         * and avoid creating more from these quests if possible
         */
        private void AddEnemies(DropQuestSO quest)
        {
            foreach (var dropItemData in quest.ItemDataByEnemyFitness)
            {
                AddEnemiesFromPairToDictionary(dropItemData);
            }
        }

        private void AddEnemiesFromPairToDictionary(KeyValuePair<ItemSO, Dictionary<float, int>> dropItemData)
        {
            foreach (var enemyData in dropItemData.Value)
            {
                AddEnemiesToDictionary(enemyData);
            }
        }

        private void AddEnemiesToDictionary(KeyValuePair<float, int> enemyData)
        {
            int newEnemies = enemyData.Value;
            NEnemies += newEnemies;
            if (totalByFitness.TryGetValue(enemyData.Key, out var enemiesForItem))
            {
                totalByFitness[enemyData.Key] = enemiesForItem + newEnemies;
            }
            else
            {
                totalByFitness.Add(enemyData.Key, newEnemies);
            }
        }
    }
}