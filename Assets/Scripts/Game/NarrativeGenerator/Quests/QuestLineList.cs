using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator.Quests
{
    [CreateAssetMenu(fileName = "QuestLineList", menuName = "Overlord-Project/QuestLineList", order = 0)]
    [Serializable]
    public class QuestLineList : ScriptableObject
    {
        [field: SerializeField] public List<QuestLine> QuestLinesList { get; set; }

        public void AddQuestLine(QuestLine questLine)
        {
            QuestLinesList.Add(questLine);
        }

        public QuestLine GetRandomQuestLine()
        {
            var random = RandomSingleton.GetInstance().Random;
            return QuestLinesList[random.Next(QuestLinesList.Count)];
        }
    }
}