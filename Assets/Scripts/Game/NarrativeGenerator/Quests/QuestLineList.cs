using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator.Quests
{
    [CreateAssetMenu(fileName = "QuestLineList", menuName = "Overlord-Project/QuestLineList", order = 0)]
    public class QuestLineList : ScriptableObject
    {
        public List<QuestLine> QuestLines;
        
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