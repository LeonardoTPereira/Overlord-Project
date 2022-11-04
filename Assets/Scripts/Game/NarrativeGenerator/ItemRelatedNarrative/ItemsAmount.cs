using System;
using System.Collections.Generic;
using System.Linq;
using Game.NarrativeGenerator.Quests;
using MyBox;
using ScriptableObjects;
using UnityEngine;

namespace Game.NarrativeGenerator.ItemRelatedNarrative
{
    [Serializable]
    public class ItemsAmount : ICloneable
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
            ItemAmountBySo = (ItemAmountDictionary) original.ItemAmountBySo.Clone();
        }

        public KeyValuePair<ItemSo, QuestIdList> GetRandom()
        {
            return ItemAmountBySo.GetRandom();
        }

        public int GetTotalItems()
        {
            return ItemAmountBySo.Sum(itemAmountPair => itemAmountPair.Value.QuestIds.Count * itemAmountPair.Key.Value);
        }
        
        public void AddNItemsFromType(KeyValuePair<ItemSo, QuestIdList> selectedType, int newItems)
        {
            var itemType = selectedType.Key;
            if (!itemAmountBySo.ContainsKey(itemType))
            {
                itemAmountBySo.Add(itemType, new QuestIdList());
            }
            for (var i = 0; i < newItems; i++)
            {
                itemAmountBySo[itemType].QuestIds.Add(selectedType.Value.QuestIds.First());
                selectedType.Value.QuestIds.RemoveAt(0);
            }
        }

        public void RemoveCurrentTypeIfEmpty(ItemSo selectedType)
        {
            if (itemAmountBySo.Count == 0)
                throw new ArgumentException($"Enemies in Quest cannot be an empty collection. " +
                                            $"{nameof(itemAmountBySo)}");
            if (itemAmountBySo[selectedType].QuestIds.Count <= 0)
            {
                itemAmountBySo.Remove(selectedType);
            }
        }
        
        public object Clone()
        {
            return new ItemsAmount(this);
        }
    }
}