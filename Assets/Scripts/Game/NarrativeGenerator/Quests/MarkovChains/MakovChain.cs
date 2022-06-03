using UnityEngine;
using System.Collections.Generic;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NarrativeGenerator.Quests.QuestGrammarNonterminals;
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
            symbolList.Add( new StartSymbol() );
            symbolNumber = 0;
            symbolList[0].canDrawNext = true;
            symbolList[0].symbolType = Constants.TALK_QUEST;
        }

        /// <summary>
        /// Sets the next symbol of the chain
        /// </summary>
        public void SetSymbol ( string _symbol )
        {
            symbolNumber++;
            switch (_symbol)
            {
                case Constants.KILL_QUEST:
                    symbolList.Add( ScriptableObject.CreateInstance<MasteryQuestSOs>() );
                break;
                case Constants.TALK_QUEST:
                    symbolList.Add( ScriptableObject.CreateInstance<CreativityQuestSOs>() );
                break;
                case Constants.GET_QUEST:
                    symbolList.Add( ScriptableObject.CreateInstance<AchievmentQuestSOs>() );
                break;
                case Constants.EXPLORE_QUEST:
                    symbolList.Add( ScriptableObject.CreateInstance<ImmersionQuestSOs>() );
                break;
                case Constants.EXCHANGE:
                    symbolList.Add( ScriptableObject.CreateInstance<Exchange>() );
                break;
                case Constants.LISTEN:
                    symbolList.Add( ScriptableObject.CreateInstance<Listen>() );
                break;
                case Constants.READ:
                    symbolList.Add( ScriptableObject.CreateInstance<Read>() );
                break;
                case Constants.REPORT:
                    symbolList.Add( ScriptableObject.CreateInstance<Report>() );
                break;
                case Constants.GIVE:
                    symbolList.Add( ScriptableObject.CreateInstance<Give>() );
                break;
                case Constants.GATHER:
                    symbolList.Add( ScriptableObject.CreateInstance<Gather>() );
                break;
                case Constants.EXPERIMENT:
                    symbolList.Add( ScriptableObject.CreateInstance<Experiment>() );
                break;
                case Constants.REPAIR:
                    symbolList.Add( ScriptableObject.CreateInstance<Repair>() );
                break;
                case Constants.TAKE:
                    symbolList.Add( ScriptableObject.CreateInstance<Take>() );
                break;
                case Constants.USE:
                    symbolList.Add( ScriptableObject.CreateInstance<Use>() );
                break;
                case Constants.DEFEND:
                    symbolList.Add( ScriptableObject.CreateInstance<Defend>() );
                break;
                case Constants.KILL:
                    symbolList.Add( ScriptableObject.CreateInstance<Kill>() );
                break;
                case Constants.CAPTURE:
                    symbolList.Add( ScriptableObject.CreateInstance<Capture>() );
                break;
                case Constants.DAMAGE:
                    symbolList.Add( ScriptableObject.CreateInstance<Damage>() );
                break;
                case Constants.STEALTH:
                    symbolList.Add( ScriptableObject.CreateInstance<Stealth>() );
                break;
                case Constants.ESCORT:
                    symbolList.Add( ScriptableObject.CreateInstance<Escort>() );
                break;
                case Constants.GOTO:
                    symbolList.Add( ScriptableObject.CreateInstance<Goto>() );
                break;
                case Constants.SPY:
                    symbolList.Add( ScriptableObject.CreateInstance<Spy>() );
                break;
                case Constants.EMPTY_TERMINAL:
                    symbolList.Add( ScriptableObject.CreateInstance<EmptyQuestSO>() );
                break;
                default:
                    Debug.LogError("symbol not found");
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