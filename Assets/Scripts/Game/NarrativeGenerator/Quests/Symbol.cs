using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game.NarrativeGenerator.Quests
{
    public interface Symbol 
    {
        public Dictionary<string, Func<int,int>> nextSymbolChances {get; set;}
        public string symbolType {get; set;}
        public bool canDrawNext {get; set;}
        public void SetDictionary( Dictionary<string, Func<int,int>> _nextSymbolChances );
        public void SetNextSymbol ( MarkovChain chain );//, Dictionary<string, Func<int,int>> _nextSymbolChances );
    }
}