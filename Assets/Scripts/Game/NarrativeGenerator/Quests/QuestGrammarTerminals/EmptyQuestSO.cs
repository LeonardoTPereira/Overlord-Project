using UnityEngine;
using System;
using System.Collections.Generic;
using Game.NarrativeGenerator;
using Game.NPCs;
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

        public void Option( MarkovChain chain, List<QuestSO> questSos, List<NpcSo> possibleNpcSos)
        {
            
        }

        void NextSymbol(MarkovChain chain)
        {
            //
        }
    }
}