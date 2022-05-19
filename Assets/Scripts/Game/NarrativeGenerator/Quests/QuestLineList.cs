using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator.Quests
{
    [CreateAssetMenu(fileName = "QuestLineList", menuName = "Overlord-Project/QuestLineList", order = 0)]
    [Serializable]
    public class QuestLineList : ScriptableObject, SaveableGeneratedContent
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
        
        public void SaveAsset(string directory)
        {
#if UNITY_EDITOR
            const string questLineName = "QuestLineList";
            var fileName = directory + questLineName + ".asset";
            if (!AssetDatabase.GUIDFromAssetPath(fileName).Empty()) return;
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(fileName);
            AssetDatabase.CreateAsset(this, uniquePath);
#endif
        }
    }
}