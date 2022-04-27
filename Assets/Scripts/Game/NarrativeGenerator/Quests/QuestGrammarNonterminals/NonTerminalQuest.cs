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
        public Dictionary<string, Func<int,int>> nextSymbolChances {get; set;}
        public virtual bool canDrawNext {
            get { return true; } 
            set {} 
        }
        private bool _canDrawNext;
        protected float r;
        protected int lim;
        protected float maxQuestChance;
        protected readonly Dictionary<string, int> QuestWeightsByType;
        private const int QuestLimit = 2;

        protected NonTerminalQuest(int lim, Dictionary<string, int> questWeightsByType)
        {
            this.QuestWeightsByType = questWeightsByType;
            this.lim = lim;
        }

        protected void DrawQuestType()
        {
            r = ((QuestWeightsByType[Constants.TALK_QUEST] +
                  QuestWeightsByType[Constants.GET_QUEST] * 2 +
                  QuestWeightsByType[Constants.KILL_QUEST] * 3 +
                  QuestWeightsByType[Constants.EXPLORE_QUEST] * 4) / 16.0f) *
                UnityEngine.Random.Range(0f, 3f);
            if (lim == QuestLimit)
            {
                r = maxQuestChance;
            }
            lim++;
        }

        public void SetDictionary( Dictionary<string, Func<int,int>> _nextSymbolChances  )
        {
            nextSymbolChances = _nextSymbolChances;
        }

        public void SetNextSymbol(MarkovChain chain)
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