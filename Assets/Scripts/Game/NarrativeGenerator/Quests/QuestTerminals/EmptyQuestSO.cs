using UnityEngine;
using System;
using System.Collections.Generic;
using Game.NarrativeGenerator;
using Util;

namespace Game.NarrativeGenerator.Quests
{
    // [CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/GetQuest"), Serializable]
    class EmptyQuestSO : QuestSO, Symbol
    {
        public Dictionary<SymbolType,Func<float,float>> nextSymbolChances {get; set;}

        public EmptyQuestSO ()
        {
            canDrawNext = false;
            symbolType = Constants.EMPTY_TERMINAL;
        }

        void SetDictionary( Dictionary<SymbolType, Func<float,float>> _nextSymbolChances )
        {
            nextSymbolChances = _nextSymbolChances;
        }

        void NextSymbol(MarkovChain chain)
        {
            //
        }
    }
}
