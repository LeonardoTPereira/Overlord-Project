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
            symbolList[0].symbolType = Constants.IMMERSION_QUEST;
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
                    symbolList.Add( new Kill() );
                break;
                case Constants.IMMERSION_QUEST:
                    symbolList.Add( new Talk() );
                break;
                case Constants.ACHIEVEMENT_QUEST:
                    symbolList.Add( new Get() );
                break;
                case Constants.CREATIVITY_QUEST:
                    symbolList.Add( new Explore() );
                break;
                case Constants.KILL_QUEST:
                    symbolList.Add( ScriptableObject.CreateInstance<KillQuestSO>() );
                break;
                case Constants.LISTEN_QUEST:
                    symbolList.Add( ScriptableObject.CreateInstance<ListenQuestSO>() );
                break;
                case Constants.EMPTY_QUEST:
                    symbolList.Add( ScriptableObject.CreateInstance<EmptyQuestSO>() );
                break;
                case Constants.GATHER_QUEST:
                    symbolList.Add( ScriptableObject.CreateInstance<GetQuestSo>() );
                break;
                case Constants.ITEM_QUEST:
                    symbolList.Add( ScriptableObject.CreateInstance<ItemQuestSo>() );
                break;
                case Constants.SECRET_QUEST:
                    symbolList.Add( ScriptableObject.CreateInstance<SecretRoomQuestSO>() );
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