using UnityEngine;
using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;

namespace Game.NarrativeGenerator
{
    public interface Symbol 
    {
        public Dictionary<string, Func<float,float>> nextSymbolChances {get; set;}
        public bool canDrawNext {get; set;}
        public void SetDictionary( Dictionary<string, Func<float,float>> _nextSymbolChances );
        public void SetNextSymbol ( MarkovChain chain );
    }
}