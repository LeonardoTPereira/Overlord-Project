using System;
using System.Collections.Generic;
using Game.ExperimentControllers;

namespace Game.NarrativeGenerator.Quests
{
    public interface ISymbol 
    {
        public Dictionary<string, Func<int,float>> NextSymbolChances {get; set;}
        public string SymbolType {get;}
        public bool CanDrawNext {get;}
        public void SetNextSymbol ( MarkovChain chain );
        public QuestSo DefineQuestSo (List<QuestSo> questSos, in GeneratorSettings generatorSettings);
    }
}