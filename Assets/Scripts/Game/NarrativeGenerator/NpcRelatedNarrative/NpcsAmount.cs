using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
using Game.NPCs;
using MyBox;
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
            foreach (var npcTypeAmountPair in original.NpcAmountBySo)
            {
                NpcAmountBySo.Add(npcTypeAmountPair.Key, npcTypeAmountPair.Value);
            }
        }

        public KeyValuePair<NpcSo, QuestLine> GetRandom()
        {
            return NpcAmountBySo.GetRandom();
        }
    }
}