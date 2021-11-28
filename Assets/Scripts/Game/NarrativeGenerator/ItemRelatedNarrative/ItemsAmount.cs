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

        public KeyValuePair<ItemSo, int> GetRandom()
        {
            return ItemAmountBySo.GetRandom();
        }

        public int GetTotalItems()
        {
            return itemAmountBySo.Sum(itemAmountPair => itemAmountPair.Value);
        }
    }
}