using System;
using System.Collections.Generic;
using Game.ExperimentControllers;
using ScriptableObjects;
using UnityEngine;
using Util;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.NarrativeGenerator.Quests
{
    
    [CreateAssetMenu(fileName = "QuestSo", menuName = "Overlord-Project/QuestSo", order = 0)]
    [Serializable]
    public abstract class QuestSo : ScriptableObject, ISavableGeneratedContent, ISymbol
    {
        public virtual string SymbolType {get; set;}
        public virtual Dictionary<string, Func<int,float>> NextSymbolChances
        {
            get => _nextSymbolChances;
            set => _nextSymbolChances = value;
        }
        protected Dictionary<string, Func<int,float>> _nextSymbolChances;
        public virtual bool CanDrawNext {
            get => true;
        }

        [SerializeReference] private QuestSo next;
        [SerializeReference] private QuestSo previous;
        [SerializeField] private string questName;
        [SerializeField] private bool endsStoryLine;
        [field: SerializeField] public bool IsCompleted { get; set; }
        [field: SerializeField] public bool IsClosed { get; set; }
        [field: SerializeField] public string QuestText { get; set; }
        private bool _canDrawNext;
        public QuestSo Next { get => next; set => next = value; }
        public QuestSo Previous { get => previous; set => previous = value; }
        public string QuestName { get => questName; set => questName = value; }
        public bool EndsStoryLine { get => endsStoryLine; set => endsStoryLine = value; }
        public int Id { get; set; }

        public virtual QuestSo DefineQuestSo (List<QuestSo> questSos, in GeneratorSettings generatorSettings)
        {
            return null;
        }

        public virtual void Init()
        {
            next = null;
            previous = null;
            questName = "Null";
            endsStoryLine = false;
            Id = GetInstanceID();
            IsCompleted = false;
            IsClosed = false;
        }

        public void Init(string questTitle, bool endsLine, QuestSo previousQuest)
        {
            QuestName = questTitle;
            EndsStoryLine = endsLine;
            Previous = previousQuest;
            next = null;
            Id = GetInstanceID();
            IsCompleted = false;
            IsClosed = false;
        }
        
        public virtual void Init(QuestSo copiedQuest)
        {
            QuestName = copiedQuest.QuestName;
            EndsStoryLine = copiedQuest.EndsStoryLine;
            Previous = copiedQuest.Previous;
            next = copiedQuest.Next;
            Id = copiedQuest.Id;
            QuestText = copiedQuest.QuestText;
            IsCompleted = copiedQuest.IsCompleted;
            IsClosed = copiedQuest.IsCompleted;
        }

        public virtual QuestSo Clone()
        {
            var cloneQuest = CreateInstance<QuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }

        public void SetNextSymbol(MarkovChain chain)
        {
            var chance = RandomSingleton.GetInstance().Next(0, 99);
            float cumulativeProbability = 0;
            foreach ( var nextSymbolChance in NextSymbolChances )
            {
                cumulativeProbability += nextSymbolChance.Value( chain.symbolNumber );
                if (cumulativeProbability < chance) continue;
                var nextSymbol = nextSymbolChance.Key;
                chain.SetSymbol( nextSymbol );
                break;
            }
        }

        public void SaveAsset(string directory)
        {
            #if UNITY_EDITOR
            const string newFolder = "Quests";
            var fileName = directory;
            if (!AssetDatabase.IsValidFolder(fileName + Constants.SeparatorCharacter + newFolder))
            {
                AssetDatabase.CreateFolder(fileName, newFolder);
            }
            fileName += Constants.SeparatorCharacter + newFolder;
            fileName += Constants.SeparatorCharacter;
            fileName += QuestName+".asset";
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(fileName);
            AssetDatabase.CreateAsset(this, uniquePath);
            #endif
        }

        public override string ToString()
        {
            return QuestText;
        }

        public abstract bool HasAvailableElementWithId<T>(T questElement, int questId);
        public abstract void RemoveElementWithId<T>(T questElement, int questId);
        public abstract void CreateQuestString();
    }
}
