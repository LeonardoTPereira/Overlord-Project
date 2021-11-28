/*using UnityEngine;
using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;

namespace Game.NarrativeGenerator
{
    public enum SymbolType
    {
        // Non-terminals
        Start,
        Kill,
        Talk,
        Get,
        Explore,
        // Terminals
        explore,
        kill,
        talk,
        empty,
        get,
        drop,
        item,
        secret
    }
    public interface Symbol 
    {
        public Dictionary<float,SymbolType> nextSymbolChance {get; set;}
        public bool canDrawNext {get; set;}
        // public Dictionary<SymbolType,float> nextSymbolChance {get; set;}
        void SetDictionary( Dictionary<float,SymbolType> _nextSymbolChances );
        void SetNextSymbol ( MarkovChain chain);
    }
}*/