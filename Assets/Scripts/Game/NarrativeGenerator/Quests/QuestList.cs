using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.NarrativeGenerator.Quests
{
    public class QuestList : ScriptableObject
    {
        public List<QuestSO> graph;
        public QuestList()
        {
            graph = new List<QuestSO>();
        }
    }
}
