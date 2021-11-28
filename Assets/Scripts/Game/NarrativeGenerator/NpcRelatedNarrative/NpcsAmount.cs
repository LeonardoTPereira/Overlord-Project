using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.NpcRelatedNarrative;
using MyBox;
using ScriptableObjects;
using UnityEngine;

namespace Game.NarrativeGenerator.NpcRelatedNarrative
{
    [Serializable]
    public class NpcsAmount
    {
        [SerializeField]
        private NpcAmountDictionary npcAmountBySo;
        public NpcAmountDictionary NpcAmountBySo
        {
            get => npcAmountBySo;
            set => npcAmountBySo = value;
        }

        public NpcsAmount()
        {
            NpcAmountBySo = new NpcAmountDictionary();
        }

        public NpcsAmount(NpcsAmount original)
        {
            NpcAmountBySo = new NpcAmountDictionary();
            foreach (var weaponTypeAmountPair in original.NpcAmountBySo)
            {
                NpcAmountBySo.Add(weaponTypeAmountPair.Key, weaponTypeAmountPair.Value);
            }
        }

        public KeyValuePair<NpcSO, int> GetRandom()
        {
            return NpcAmountBySo.GetRandom();
        }
    }
}