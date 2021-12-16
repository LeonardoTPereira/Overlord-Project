using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using ScriptableObjects;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator.Quests.nao_terminais
{
    public class Get : NonTerminalQuest
    {
        public Get(int lim, Dictionary<string, int> questWeightsByType) : base(lim, questWeightsByType)
        {
            maxQuestChance = 2.8f;
        }
    
        public void Option(List<QuestSO> questSos, List<NpcSO> possibleNpcSos, TreasureRuntimeSetSO possibleItems, WeaponTypeRuntimeSetSO enemyTypes)
        {
            DrawQuestType();
            DefineNextQuest(questSos, possibleNpcSos, possibleItems, enemyTypes);
        }

        protected void DefineNextQuest(List<QuestSO> questSos, List<NpcSO> possibleNpcSos, TreasureRuntimeSetSO possibleItems, WeaponTypeRuntimeSetSO enemyTypes)
        {
            if (r > 2.8)
            {
                CreateAndSaveGetQuestSo(questSos, possibleItems);
                var t = new Talk(lim, QuestWeightsByType);
                t.Option(questSos, possibleNpcSos);
                Option(questSos, possibleNpcSos, possibleItems, enemyTypes);
            }
            if (r > 2.5 && r <= 2.8)
            {
                CreateAndSaveGetQuestSo(questSos, possibleItems);
            }
            if (r > 2.2 && r <= 2.5)
            {
                CreateAndSaveDropQuestSo(questSos,
                    possibleItems, enemyTypes);
                var t = new Talk(lim, QuestWeightsByType);
                t.Option(questSos, possibleNpcSos);
                Option(questSos, possibleNpcSos, possibleItems, enemyTypes);
            }
            if (r <= 2.2)
            {
                CreateAndSaveDropQuestSo(questSos,
                    possibleItems, enemyTypes);
            }
        }

        private static void CreateAndSaveGetQuestSo(List<QuestSO> questSos,
            TreasureRuntimeSetSO possibleItems)
        {
            var getItemQuest = ScriptableObject.CreateInstance<ItemQuestSo>();
            var selectedItems = new Dictionary<ItemSo, int>();
            //TODO select more items
            var selectedItem = possibleItems.GetRandomItem();
            selectedItems.Add(selectedItem, 1);
            getItemQuest.Init(ItemsToString(selectedItems), false, questSos.Count > 0 ? questSos[questSos.Count-1] : null, selectedItems);
            getItemQuest.SaveAsAsset();
            questSos.Add(getItemQuest);

        }

        private static void CreateAndSaveDropQuestSo(List<QuestSO> questSos,
            TreasureRuntimeSetSO possibleItems, WeaponTypeRuntimeSetSO enemyTypes)
        {
            var dropQuest = ScriptableObject.CreateInstance<DropQuestSo>();

            var dropItemData = new Dictionary<ItemSo, EnemiesByType >();

            //TODO select more items
            var selectedItem = possibleItems.GetRandomItem();
            var selectedEnemyTypes = new EnemiesByType ();
            //TODO select more enemies
            var selectedEnemyType = enemyTypes.GetRandomItem();
            var nEnemiesToKill = RandomSingleton.GetInstance().Random.Next(7) + 5;
            selectedEnemyTypes.EnemiesByTypeDictionary.Add(selectedEnemyType, nEnemiesToKill);
            dropItemData.Add(selectedItem, selectedEnemyTypes);
            dropQuest.Init(DropItemsToString(dropItemData), false, questSos.Count > 0 ? questSos[questSos.Count-1] : null, dropItemData);
            dropQuest.SaveAsAsset();
            questSos.Add(dropQuest);
        }

        private static string ItemsToString(Dictionary<ItemSo, int> selectedItems)
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < selectedItems.Count; i++)
            {
                var itemAmountPair = selectedItems.ElementAt(i);
                stringBuilder.Append($"$Get {itemAmountPair.Value} {itemAmountPair.Key}");
                if (itemAmountPair.Value > 1)
                {
                    stringBuilder.Append("s");
                }

                if (i < (selectedItems.Count - 1))
                {
                    stringBuilder.Append(" and ");
                }
            }
            return stringBuilder.ToString();
        }
    
        private static string DropItemsToString(Dictionary<ItemSo, EnemiesByType > dropItems)
        {
            var stringBuilder = new StringBuilder();
            foreach (var itemToDrop in dropItems)
            {
                stringBuilder.Append($"$Get {itemToDrop.Key} From ");
                foreach (var itemsPerEnemy in itemToDrop.Value.EnemiesByTypeDictionary)
                {
                    stringBuilder.Append($"{itemsPerEnemy.Value} {itemsPerEnemy.Key}");
                }
            }
            return stringBuilder.ToString();
        }
    }
}