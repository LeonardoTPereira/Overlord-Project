/*using UnityEngine;
using System.Collections.Generic;

namespace Game.NarrativeGenerator.Quests
{
    // [CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/GetQuest"), Serializable]
    class EmptyQuestSO : QuestSO, Symbol
    {
        public Dictionary<SymbolType,float> nextSymbolChance {get; set;}

        public EmptyQuestSO ()
        {
            canDrawNext = false;
            symbolType = SymbolType.empty;
        }

        void SetDictionary( Dictionary<SymbolType, float> _nextSymbolChances )
        {
            nextSymbolChance = _nextSymbolChances;
        }

        void NextSymbol(MarkovChain chain)
        {
            //
        }
    }
}
*/