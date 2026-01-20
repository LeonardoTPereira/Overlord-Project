using System;
using Util;

namespace Overlord.NarrativeGenerator.Quests.QuestGrammarTerminals
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

        public override void CreateQuestString(Enums.Language l)
        {
            throw new NotImplementedException();
        }
    }
}