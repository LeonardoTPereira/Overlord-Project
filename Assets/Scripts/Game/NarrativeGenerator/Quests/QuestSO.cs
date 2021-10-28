using UnityEditor;
using UnityEngine;
using ScriptableObjects;
using System.Collections.Generic;

namespace Game.NarrativeGenerator.Quests
{
    
    [CreateAssetMenu(fileName = "QuestSO", menuName = "Overlord-Project/QuestSO", order = 0)]
    public class QuestSO : ScriptableObject, Symbol {
        public Dictionary<float,SymbolType> nextSymbolChance {get; set;}
        public bool canDrawNext { get ; set; }
        private QuestSO nextWhenSuccess;
        private QuestSO nextWhenFailure;
        private QuestSO previous;
        private string questName;
        private bool endsStoryLine;
        private TreasureSO reward;

        public QuestSO NextWhenSuccess { get => nextWhenSuccess; set => nextWhenSuccess = value; }
        public QuestSO NextWhenFailure { get => nextWhenFailure; set => nextWhenFailure = value; }
        public QuestSO Previous { get => previous; set => previous = value; }
        public string QuestName { get => questName; set => questName = value; }
        public bool EndsStoryLine { get => endsStoryLine; set => endsStoryLine = value; }
        public TreasureSO Reward { get => reward; set => reward = value; }

        public virtual void Init()
        {
            nextWhenSuccess = null;
            nextWhenFailure = null;
            previous = null;
            questName = "Null";
            endsStoryLine = false;
            Reward = null;
        }

        public virtual void Init(string name, bool endsStoryLine, QuestSO previous)
        {
            QuestName = name;
            EndsStoryLine = endsStoryLine;
            Previous = previous;
            nextWhenSuccess = null;
            nextWhenFailure = null;
            Reward = null;
        }

        public void SaveAsAsset(string assetName)
        {
            AssetDatabase.CreateAsset(this, assetName+".asset");
        }

        void Symbol.SetDictionary( Dictionary<float,SymbolType> _nextSymbolChances  )
        {
            nextSymbolChance = _nextSymbolChances;
        }

        void Symbol.SetNextSymbol(MarkovChain chain)
        {
            float chance = (float) Random.Range( 0, 100 ) / 100 ;
            float currentSymbolChance = 0;
            foreach ( float nextSymbol in nextSymbolChance.Keys )
            {
                if ( nextSymbol > chance )
                {
                    SymbolType _nextSymbol;
                    nextSymbolChance.TryGetValue( nextSymbol, out _nextSymbol );
                    chain.SetSymbol( _nextSymbol );
                }
            }
        }
    }
}
