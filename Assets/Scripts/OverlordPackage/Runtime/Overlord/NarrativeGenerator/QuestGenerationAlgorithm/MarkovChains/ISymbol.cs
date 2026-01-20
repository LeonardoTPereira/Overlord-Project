using System;
using System.Collections.Generic;
using Overlord.NarrativeGenerator.NPCs;
using static Util.Enums;

namespace Overlord.NarrativeGenerator.Quests
{
    public interface ISymbol 
    {
        public Dictionary<string, Func<int,float>> NextSymbolChances {get; set;}
        public string SymbolType {get;}
        public bool CanDrawNext {get;}
        public void SetNextSymbol ( MarkovChain chain );
        public QuestSo DefineQuestSo (List<QuestSo> questSos, NpcSo npcInCharge, in NarrativeSettings generatorSettings, Language language);
    }
}