using System;
using System.Collections.Generic;
using Game.NPCs;
using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests
{
    public interface ISymbol 
    {
        public Dictionary<string, Func<int,int>> NextSymbolChances {get; set;}
        public string SymbolType {get; set;}
        public bool CanDrawNext {get; set;}
        public void SetDictionary( Dictionary<string, Func<int,int>> symbolChances );
        public void SetNextSymbol ( MarkovChain chain );
        public QuestSo DefineQuestSo ( List<QuestSo> questSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSO possibleItems, WeaponTypeRuntimeSetSO enemyTypes );
    }
}