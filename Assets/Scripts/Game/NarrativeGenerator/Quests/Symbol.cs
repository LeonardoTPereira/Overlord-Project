using UnityEngine;
using System;
using System.Collections.Generic;

using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using Game.NPCs;
using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests
{
    public interface Symbol 
    {
        public Dictionary<string, Func<int,int>> NextSymbolChances {get; set;}
        public string symbolType {get; set;}
        public bool canDrawNext {get; set;}
        public void SetDictionary( Dictionary<string, Func<int,int>> _nextSymbolChances );
        public void SetNextSymbol ( MarkovChain chain );
        public void DefineQuestSo ( MarkovChain chain, List<QuestSo> QuestSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSO possibleItems, WeaponTypeRuntimeSetSO enemyTypes );
    }
}