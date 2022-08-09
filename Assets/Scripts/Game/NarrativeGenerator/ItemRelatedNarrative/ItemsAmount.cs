using System;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using ScriptableObjects;
using UnityEngine;

namespace Game.NarrativeGenerator.ItemRelatedNarrative
{
    [Serializable]
    public class ItemsAmount
    {
        [SerializeField]
        private ItemAmountDictionary itemAmountBySo;
        public ItemAmountDictionary ItemAmountBySo
        {
            get => itemAmountBySo;
            set => itemAmountBySo = value;
        }

        public ItemsAmount()
        {
            ItemAmountBySo = new ItemAmountDictionary();
        }

        public ItemsAmount(ItemsAmount original)
        {
            ItemAmountBySo = new ItemAmountDictionary();
            foreach (var itemTypeAmountPair in original.ItemAmountBySo)
            {
                ItemAmountBySo.Add(itemTypeAmountPair.Key, itemTypeAmountPair.Value);
            }
        }

        public KeyValuePair<ItemSo, LinkedList<int>> GetRandom()
        {
            return ItemAmountBySo.GetRandom();
        }

        public int GetTotalItems()
        {
            return ItemAmountBySo.Sum(itemAmountPair => itemAmountPair.Value.Count * itemAmountPair.Key.Value);
        }
        
        public void AddNItemsFromType(KeyValuePair<ItemSo, LinkedList<int>> selectedType, int newEnemies)
        {
            var itemType = selectedType.Key;
            if (!itemAmountBySo.ContainsKey(itemType))
            {
                itemAmountBySo.Add(itemType, new LinkedList<int>());
            }
            for (var i = 0; i < newEnemies; i++)
            {
                itemAmountBySo[itemType].AddLast(selectedType.Value.First.Value);
                selectedType.Value.RemoveFirst();
            }
        }

        public void RemoveCurrentTypeIfEmpty(ItemSo selectedType)
        {
            if (itemAmountBySo.Count == 0)
                throw new ArgumentException($"Enemies in Quest cannot be an empty collection. " +
                                            $"{nameof(itemAmountBySo)}");
            if (itemAmountBySo[selectedType].Count <= 0)
            {
                itemAmountBySo.Remove(selectedType);
            }
        }
    }
}