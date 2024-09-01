using UnityEngine;
using System.Collections.Generic;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NarrativeGenerator.Quests.QuestGrammarNonTerminals;
using Util;

namespace Game.NarrativeGenerator.Quests
{
    public class MarkovChain
    {
        public List<ISymbol> symbolList = new List<ISymbol>();
        public int symbolNumber = 0;

        #region MarkovChain Implementation
        /// <summary>
        /// Initializes the markov chain
        /// </summary>
        public MarkovChain ()
        {
            symbolList.Add( ScriptableObject.CreateInstance<StartSymbol>() );
            symbolNumber = 0;
        }

        /// <summary>
        /// Sets the next symbol of the chain
        /// </summary>
        public void SetSymbol ( string symbol )
        {
            symbolNumber++;
            switch ( symbol )
            {
                case Constants.MasteryQuest:
                    symbolList.Add( ScriptableObject.CreateInstance<MasteryQuestSo>() );
                break;
                case Constants.ImmersionQuest:
                    symbolList.Add( ScriptableObject.CreateInstance<ImmersionQuestSo>() );
                break;
                case Constants.AchievementQuest:
                    symbolList.Add( ScriptableObject.CreateInstance<AchievementQuestSo>() );
                break;
                case Constants.CreativityQuest:
                    symbolList.Add( ScriptableObject.CreateInstance<CreativityQuestSo>() );
                break;
                case nameof(KillQuestSo):
                    symbolList.Add( ScriptableObject.CreateInstance<KillQuestSo>() );
                break;
                case Constants.DamageQuest:
                    symbolList.Add( ScriptableObject.CreateInstance<DamageQuestSo>() );
                break;
                case Constants.ListenQuest:
                    symbolList.Add( ScriptableObject.CreateInstance<ListenQuestSo>() );
                break;
                case Constants.ReadQuest:
                    symbolList.Add( ScriptableObject.CreateInstance<ReadQuestSo>() );
                break;
                case Constants.GiveQuest:
                    symbolList.Add( ScriptableObject.CreateInstance<GiveQuestSo>() );
                break;
                case Constants.ReportQuest:
                    symbolList.Add( ScriptableObject.CreateInstance<ReportQuestSo>() );
                break;
                case Constants.GatherQuest:
                    symbolList.Add( ScriptableObject.CreateInstance<GatherQuestSo>() );
                break;
                case Constants.ExchangeQuest:
                    symbolList.Add( ScriptableObject.CreateInstance<ExchangeQuestSo>() );
                break;
                case Constants.GotoQuest:
                    symbolList.Add( ScriptableObject.CreateInstance<GotoQuestSo>() );
                break;
                case Constants.ExploreQuest:
                    symbolList.Add( ScriptableObject.CreateInstance<ExploreQuestSo>() );
                break;
                case Constants.EmptyQuest:
                    symbolList.Add( ScriptableObject.CreateInstance<EmptyQuestSo>() );
                break;
                default:
                    Debug.LogError("Symbol type not found!");
                break;
            }
        }
        #endregion

        public ISymbol GetLastSymbol ()
        {
            return this.symbolList[symbolNumber];
        }
    }
}