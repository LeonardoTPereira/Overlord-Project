using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.NarrativeGenerator.Quests
{
    [Serializable]
    public class QuestList
    {
        [SerializeField]
        private List<QuestSO> quests;

        public QuestList()
        {
            quests = new List<QuestSO>();
        }
        public List<QuestSO> Quests
        {
            get => quests;
            set => quests = value;
        }
    }
}