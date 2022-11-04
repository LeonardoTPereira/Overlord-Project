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
    public class EmptyQuestSo : QuestSo, ISymbol
    {
        public override string SymbolType { 
            get { return Constants.EmptyQuest;} 
        }
        public override bool CanDrawNext => false;

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            throw new NotImplementedException();
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            throw new NotImplementedException();
        }

        public override void CreateQuestString()
        {
            throw new NotImplementedException();
        }
    }
}