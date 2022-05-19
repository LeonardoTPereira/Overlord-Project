using System.Collections.Generic;
using System;
using UnityEngine;
using Util;
using Game.NarrativeGenerator.Quests;

namespace Game.NarrativeGenerator.Quests.QuestGrammarNonterminals
{
    public abstract class NonTerminalQuest : Symbol
    {
        public virtual string symbolType {get; set;}
        public virtual Dictionary<string, Func<int,int>> nextSymbolChances
        {
            get;
            set;
        }

        public virtual bool canDrawNext
        {
            get { return true; } 
            set {} 
        }
        private bool _canDrawNext;

        public void SetDictionary( Dictionary<string, Func<int,int>> _nextSymbolChances  )
        {
            nextSymbolChances = _nextSymbolChances;
        }

        public void SetNextSymbol( MarkovChain chain )
        {
            int chance = (int) UnityEngine.Random.Range( 0, 100 );
            int cumulativeProbability = 0;
            foreach ( KeyValuePair<string, Func<int,int>> nextSymbolChance in nextSymbolChances )
            {
                cumulativeProbability += nextSymbolChance.Value( chain.symbolNumber );
                if ( cumulativeProbability >= chance )
                {
                    string nextSymbol = nextSymbolChance.Key;
                    chain.SetSymbol( nextSymbol );
                    break;
                }
            }
        }
    }
}