using UnityEngine;
using System.Collections.Generic;

namespace Game.NarrativeGenerator 
{
    public class MarkovChain
    {
        private Symbol symbol;

        public MarkovChain ()
        {
            symbol = null;
        }

        public void SetSymbol ( Symbol _symbol )
        {
            this.symbol = _symbol;
        }

        public Symbol GetSymbol ()
        {
            return this.symbol;
        }
    }
}