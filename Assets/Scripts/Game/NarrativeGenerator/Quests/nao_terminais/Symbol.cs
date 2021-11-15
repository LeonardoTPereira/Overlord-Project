using UnityEngine;
using System;
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
        public Dictionary<string, Func<float,float>> nextSymbolChance {get; set;}
        public bool canDrawNext {get; set;}
        public void SetDictionary( Dictionary<string, Func<float,float>> _nextSymbolChances );
        public void SetNextSymbol ( MarkovChain chain);
    }
}