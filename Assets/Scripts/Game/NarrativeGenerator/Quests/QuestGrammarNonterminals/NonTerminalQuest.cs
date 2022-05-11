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
            get {
                Dictionary<string, Func<int, int>> aux = new Dictionary<string, Func<int, int>>();
                aux.Add( Constants.KILL_TERMINAL, x => (int)Mathf.Clamp( 1/(x*0.25f), 0, 100) );
                aux.Add( Constants.EMPTY_TERMINAL, x => (int)Mathf.Clamp( ( 1 -( 1/(x*0.25f))), 0, 100));
                return aux;
            } 
            set {}
        }

        public virtual bool canDrawNext
        {
            get { return true; } 
            set {} 
        }
        private bool _canDrawNext;

        public void SetDictionary( Dictionary<string, Func<int,int>> _nextSymbolChances  )
        {
            // nextSymbolChances = _nextSymbolChances;
        }

        public void SetNextSymbol( MarkovChain chain )//, Dictionary<string, Func<int,int>> _nextSymbolChances)
        {
            // SetDictionary( _nextSymbolChances );
            int chance = (int) UnityEngine.Random.Range( 0, 100 );
            int cumulativeProbability = 0;
            foreach ( KeyValuePair<string, Func<int,int>> nextSymbolChance in nextSymbolChances )
            {
                Debug.Log(nextSymbolChance.Key+" has chance of "+cumulativeProbability);
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