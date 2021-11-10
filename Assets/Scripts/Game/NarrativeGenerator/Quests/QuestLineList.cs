using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator.Quests
{
    [CreateAssetMenu(menuName = "Narrative/QuestLineList")]
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