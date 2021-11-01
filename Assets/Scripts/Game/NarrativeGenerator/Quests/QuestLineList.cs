using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator.Quests
{
    public class QuestLineList : ScriptableObject
    {
        public List<QuestLine> QuestLines { get; }
        
        public void AddQuestLine(QuestLine questLine)
        {
            QuestLines.Add(questLine);
        }

        public QuestLine GetRandomQuestLine()
        {
            var random = RandomSingleton.GetInstance().Random;
            return QuestLines[random.Next(QuestLines.Count)];
        }
    }
}