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
    public abstract class EmptyQuestSo : QuestSo, ISymbol
    {
        public override string SymbolType { 
            get { return Constants.EMPTY_QUEST;} 
        }
        public override bool CanDrawNext { 
            get { return false; } 
        }
    }
}