namespace Overlord.NarrativeGenerator.Quests.QuestGrammarNonTerminals
{
    public class StartSymbol : QuestSo
    {
        public override bool CanDrawNext => true;

        public override string SymbolType => "StartSymbol";

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            throw new System.NotImplementedException();
        }

        public override void CreateQuestString(Util.Enums.Language language)
        {
            throw new System.NotImplementedException();
        }
    }
}