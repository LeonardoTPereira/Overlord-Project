using Util;

public static class QuestCategoryMapper
{
    public static QuestCategory FromSymbol(string symbol)
    {
        switch (symbol)
        {
            case Constants.KillQuest:
            case Constants.DamageQuest:
                return QuestCategory.Mastery;

            case Constants.ListenQuest:
            case Constants.ReadQuest:
            case Constants.ReportQuest:
            case Constants.GiveQuest:
                return QuestCategory.Immersion;

            case Constants.ExploreQuest:
            case Constants.GotoQuest:
                return QuestCategory.Creativity;

            case Constants.GatherQuest:
            case Constants.ExchangeQuest:
                return QuestCategory.Achievement;

            default:
                throw new System.Exception($"Unknown quest symbol {symbol}");
        }
    }
}
