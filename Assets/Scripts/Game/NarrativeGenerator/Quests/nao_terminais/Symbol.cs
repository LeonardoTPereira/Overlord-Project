using UnityEngine;
using System.Collections.Generic;

namespace Game.NarrativeGenerator
{
    public enum SymbolType
    {
        Kill,
        kill
    }
    public interface Symbol 
    {
        // public Dictionary<SymbolType,float> nextSymbolChance {get; set;}
        // void Dictionary( Dictionary<SymbolType, float> _nextSymbolChances );
        // void NextSymbol ( MarkovChain chain);
    }
}