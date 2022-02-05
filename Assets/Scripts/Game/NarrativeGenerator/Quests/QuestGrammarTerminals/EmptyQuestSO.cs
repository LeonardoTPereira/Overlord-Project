/*using UnityEngine;
using System;
using System.Collections.Generic;
using Game.NarrativeGenerator;
using Util;

namespace Game.NarrativeGenerator.Quests
{
    // [CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/GetQuest"), Serializable]
    class EmptyQuestSO : QuestSO, Symbol
    {
        public override string symbolType { 
            get { return Constants.EMPTY_TERMINAL;} 
        }
        public override bool canDrawNext { 
            get { return false; } 
        }

        void SetDictionary( Dictionary<string, Func<float,float>> _nextSymbolChances )
        {
            nextSymbolChances = _nextSymbolChances;
        }

        void NextSymbol(MarkovChain chain)
        {
            //
        }
    }
}
*/