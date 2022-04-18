using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameManager;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using Game.NarrativeGenerator.Quests;
using MyBox;
using ScriptableObjects;
using Unity.Mathematics;
using UnityEngine;
using Util;

namespace Game.LevelManager
{
    public static class ItemDispenser
    {
        private static int _remainingItems;

        public static void DistributeItemsInDungeon(Map map, QuestLine questLine)
        {
            var itemsInQuestByType = new ItemsAmount(questLine.ItemParametersForQuestLine.ItemsByType);
            _remainingItems = questLine.ItemParametersForQuestLine.TotalItems;
            var totalTreasureRooms = map.NTreasureRooms;
            var itemsPerRoom = 0;
            if (totalTreasureRooms > 0)
            {
                itemsPerRoom = _remainingItems/totalTreasureRooms;
                _remainingItems %= totalTreasureRooms;
            }
            foreach (var dungeonPart in map.DungeonPartByCoordinates)
            {
                if (dungeonPart.Value is DungeonRoom dungeonRoom && !dungeonRoom.IsStartRoom() && dungeonRoom.HasItemPreference)
                {
                    dungeonRoom.Items = SelectItemsForRoom(itemsInQuestByType, itemsPerRoom);
                }
            }

            if (_remainingItems <= 0) return;
            
            foreach (var dungeonPart in map.DungeonPartByCoordinates)
            {
                if (dungeonPart.Value is DungeonRoom dungeonRoom && !dungeonRoom.IsStartRoom())
                {
                    dungeonRoom.Items = SelectItemsForRoom(itemsInQuestByType, itemsPerRoom);
                }
            }
        }

        private static ItemsAmount SelectItemsForRoom(ItemsAmount itemsInQuestByType, int itemsPerRoom)
        {
            var itemsByType = new ItemsAmount();
            var selectedItems = 0;
            var itemsInRoom = itemsPerRoom + _remainingItems;
            _remainingItems = 0;
            while (selectedItems < itemsInRoom)
            {
                var selectedType = itemsInQuestByType.GetRandom();
                var maxPossibleNewEnemies = math.min(selectedType.Value, itemsInRoom - selectedItems);
                var newItems = RandomSingleton.GetInstance().Random.Next(1, maxPossibleNewEnemies);
                
                AddItemsInRoom(itemsByType, selectedType.Key, newItems);
                UpdateRemainingItemsInQuest(itemsInQuestByType, selectedType.Key, newItems);
                selectedItems += newItems;
            }
            return itemsByType;
        }

        private static void AddItemsInRoom(ItemsAmount itemsAmount, ItemSo selectedType, int newEnemies)
        {
            if (itemsAmount.ItemAmountBySo.TryGetValue(selectedType, out var enemiesForItem))
            {
                itemsAmount.ItemAmountBySo[selectedType] = enemiesForItem + newEnemies;
            }
            else
            {
                itemsAmount.ItemAmountBySo.Add(selectedType, newEnemies);
            }
        }

        private static void UpdateRemainingItemsInQuest(ItemsAmount enemiesInQuestByType, 
            ItemSo selectedType, int newEnemies)
        {
            if (enemiesInQuestByType.ItemAmountBySo.Count == 0)
                throw new ArgumentException("Enemies in Quest cannot be an empty collection.", nameof(enemiesInQuestByType));
            enemiesInQuestByType.ItemAmountBySo[selectedType] -= newEnemies;
            if (enemiesInQuestByType.ItemAmountBySo[selectedType] <= 0)
            {
                enemiesInQuestByType.ItemAmountBySo.Remove(selectedType);
            }
        }
    }
}