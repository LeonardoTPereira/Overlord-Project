using UnityEngine;
using System.Collections.Generic;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
//using Game.NarrativeGenerator.Quests.QuestGrammarNonterminals;
using Util;

namespace Game.NarrativeGenerator.Quests
{
    public class MarkovChain
    {
        public List<Symbol> symbolList = new List<Symbol>();
        public int symbolNumber = 0;

        #region MarkovChain Implementation
        /// <summary>
        /// Initializes the markov chain
        /// </summary>
        public MarkovChain ()
        {
            symbolList.Add( ScriptableObject.CreateInstance<StartSymbol>() );
            symbolNumber = 0;
            symbolList[0].canDrawNext = true;
            symbolList[0].symbolType = Constants.START;
        }

        /// <summary>
        /// Sets the next symbol of the chain
        /// </summary>
        public void SetSymbol ( string _symbol )
        {
            symbolNumber++;
            switch ( _symbol )
            {
                case Constants.MASTERY_QUEST:
                    // symbolList.Add( new Kill() );
                    symbolList.Add( ScriptableObject.CreateInstance<MasteryQuestSO>() );
                break;
                case Constants.IMMERSION_QUEST:
                    // symbolList.Add( new Talk() );
                    symbolList.Add( ScriptableObject.CreateInstance<ImmersionQuestSO>() );
                break;
                case Constants.ACHIEVEMENT_QUEST:
                    // symbolList.Add( new Get() );
                    symbolList.Add( ScriptableObject.CreateInstance<AchievementQuestSO>() );
                break;
                case Constants.CREATIVITY_QUEST:
                    // symbolList.Add( new Explore() );
                    symbolList.Add( ScriptableObject.CreateInstance<CreativityQuestSO>() );
                break;
                case Constants.KILL_QUEST:
                    symbolList.Add( ScriptableObject.CreateInstance<KillQuestSO>() );
                break;
                case Constants.DAMAGE_QUEST:
                    symbolList.Add( ScriptableObject.CreateInstance<DamageQuestSO>() );
                break;
                case Constants.LISTEN_QUEST:
                    symbolList.Add( ScriptableObject.CreateInstance<ListenQuestSO>() );
                break;
                case Constants.READ_QUEST:
                    symbolList.Add( ScriptableObject.CreateInstance<ReadQuestSO>() );
                break;
                case Constants.GIVE_QUEST:
                    symbolList.Add( ScriptableObject.CreateInstance<GiveQuestSO>() );
                break;
                case Constants.REPORT_QUEST:
                    symbolList.Add( ScriptableObject.CreateInstance<ReportQuestSO>() );
                break;
                case Constants.GATHER_QUEST:
                    symbolList.Add( ScriptableObject.CreateInstance<GetQuestSo>() );
                break;
                case Constants.EXCHANGE_QUEST:
                    symbolList.Add( ScriptableObject.CreateInstance<ExchangeQuestSO>() );
                break;
                case Constants.GOTO_QUEST:
                    symbolList.Add( ScriptableObject.CreateInstance<GotoQuestSO>() );
                break;
                case Constants.EXPLORE_QUEST:
                    symbolList.Add( ScriptableObject.CreateInstance<ExploreQuestSO>() );
                break;
                case Constants.EMPTY_QUEST:
                    symbolList.Add( ScriptableObject.CreateInstance<EmptyQuestSO>() );
                break;
                default:
                    Debug.LogError("Symbol type not found!");
                break;
            }
        }
        #endregion

        public Symbol GetLastSymbol ()
        {
            return this.symbolList[symbolNumber];
        }
    }
}