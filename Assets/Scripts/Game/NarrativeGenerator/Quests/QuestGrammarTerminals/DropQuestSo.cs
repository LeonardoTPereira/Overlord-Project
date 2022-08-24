// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Game.NarrativeGenerator.EnemyRelatedNarrative;
// using Game.NarrativeGenerator.ItemRelatedNarrative;
// using ScriptableObjects;
// using UnityEngine;
// using Util;

// namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
// {
//     [CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/DropQuest"), Serializable]
//     public class DropQuestSo : ItemQuestSo
//     {
//         public Dictionary<EnemySO, float> DropChanceByEnemyType { get; set; }
//         //TODO change this. This is confusing and is not accessed as intended.
//         public Dictionary<ItemSo, EnemiesByType > ItemData { get; set; }
//         public Dictionary<ItemSo, Dictionary<float, int>> ItemDataByEnemyFitness { get; set; }

//         public override Dictionary<string, Func<int,int>> NextSymbolChances
//         {
//             get
//             {
//                 Dictionary<string, Func<int, int>> getSymbolWeights = new Dictionary<string, Func<int, int>>();
//                 getSymbolWeights.Add( Constants.ITEM_QUEST, Constants.TwoOptionQuestLineWeight );
//                 getSymbolWeights.Add( Constants.GATHER_QUEST, Constants.TwoOptionQuestLineWeight );
//                 getSymbolWeights.Add( Constants.EMPTY_QUEST, Constants.TwoOptionQuestEmptyWeight );
//                 return getSymbolWeights;
//             } 
//         }
//         public override string symbolType {
//             get { return Constants.EMPTY_QUEST; }//Constants.DROP_QUEST; }
//         }

//         public override void Init()
//         {
//             base.Init();
//             ItemData = new Dictionary<ItemSo, EnemiesByType >();
//             ItemDataByEnemyFitness = new Dictionary<ItemSo, Dictionary<float, int>>();
//         }
//         public void Init(string questName, bool endsStoryLine, QuestSo previous, Dictionary<ItemSo, EnemiesByType > dropItemData)
//         {
//             var itemsByType = new ItemAmountDictionary();
//             foreach (var itemToDrop in dropItemData)
//             {
//                 var totalItems = itemToDrop.Value.EnemiesByTypeDictionary.Sum(itemsPerEnemy => itemsPerEnemy.Value);
//                 itemsByType.Add(itemToDrop.Key, totalItems);
//             }
//             base.Init(questName, endsStoryLine, previous, itemsByType);
//             ItemData = dropItemData;
//         }        
//         public void Init(string questName, bool endsStoryLine, QuestSo previous, Dictionary<ItemSo, Dictionary<float, int>> dropItemData)
//         {
//             var itemsByType = new ItemAmountDictionary();
//             foreach (var itemToDrop in dropItemData)
//             {
//                 int totalItems = 0;
//                 foreach (var itemsPerEnemy in itemToDrop.Value)
//                 {
//                     totalItems += itemsPerEnemy.Value;
//                 }
//                 itemsByType.Add(itemToDrop.Key, totalItems);
//             }
//             base.Init(questName, endsStoryLine, previous, itemsByType);
//             ItemDataByEnemyFitness = dropItemData;
//         }
        
//         public override void Init(QuestSo copiedQuest)
//         {
//             base.Init(copiedQuest);
//             Debug.Log("Init Not Implemented for DropQuestSo");
//         }
        
//         public override QuestSo Clone()
//         {
//             var cloneQuest = CreateInstance<DropQuestSo>();
//             cloneQuest.Init(this);
//             return cloneQuest;
//         }
//     }
// }
