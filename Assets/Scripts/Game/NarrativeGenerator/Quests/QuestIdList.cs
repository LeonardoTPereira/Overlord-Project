using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.NarrativeGenerator.Quests
{
    [Serializable]
    public class QuestIdList
    {
        [field: SerializeField] public List<int> QuestIds { get; set; }

        public QuestIdList()
        {
            QuestIds = new List<int>();
        }
        
        public QuestIdList(QuestIdList copiedList)
        {
            QuestIds = new List<int>(copiedList.QuestIds);
        }

        public void Add(int questId)
        {
            QuestIds.Add(questId);
        }
    }
}