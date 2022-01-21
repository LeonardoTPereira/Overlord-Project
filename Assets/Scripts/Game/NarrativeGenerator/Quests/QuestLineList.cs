using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator.Quests
{
    public class QuestLineList : ScriptableObject
    {
        public List<QuestLine> QuestLines { get { return _questLines;} }
        private List<QuestLine> _questLines = new List<QuestLine>();
        public void AddQuestLine(QuestLine questLine)
        {
            _questLines.Add(questLine);
        }

        public QuestLine GetRandomQuestLine()
        {
            var random = RandomSingleton.GetInstance().Random;
            return _questLines[random.Next(_questLines.Count)];
        }
    }
}