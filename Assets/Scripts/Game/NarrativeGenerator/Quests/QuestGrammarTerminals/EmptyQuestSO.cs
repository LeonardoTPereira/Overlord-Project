using UnityEngine;
using System;
using System.Collections.Generic;
using Game.NarrativeGenerator;
using Util;
using ScriptableObjects;

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

        public void Option( MarkovChain chain, List<QuestSO> questSos, List<NpcSO> possibleNpcSos)
        {
            
        }

        void SetDictionary( Dictionary<string, Func<int,int>> _nextSymbolChances )
        {
            nextSymbolChances = _nextSymbolChances;
        }

        void NextSymbol(MarkovChain chain)
        {
            //
        }
    }
}