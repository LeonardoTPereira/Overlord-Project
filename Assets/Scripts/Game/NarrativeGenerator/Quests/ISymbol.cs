using System;
using System.Collections.Generic;
using Game.NPCs;
using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests
{
    public interface ISymbol 
    {
        public Dictionary<string, Func<int,int>> NextSymbolChances {get; set;}
        public string SymbolType {get;}
        public bool CanDrawNext {get;}
        public void SetNextSymbol ( MarkovChain chain );
        public QuestSo DefineQuestSo ( List<QuestSo> questSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSO possibleItems, WeaponTypeRuntimeSetSO enemyTypes );
    }
}