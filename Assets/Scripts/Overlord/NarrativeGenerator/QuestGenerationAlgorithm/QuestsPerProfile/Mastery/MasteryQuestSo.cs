using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using Overlord.NarrativeGenerator.EnemyRelatedNarrative;
using MyBox;
using static Util.Enums;
using Overlord.NarrativeGenerator.NPCs;

namespace Overlord.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class MasteryQuestSo : QuestSo
    {
        public override string SymbolType => Constants.MasteryQuest;

        public override Dictionary<string, Func<int,float>> NextSymbolChances
        {
            get
            {                    
                Dictionary<string, Func<int, float>> masteryQuestWeights = new Dictionary<string, Func<int, float>>();
                masteryQuestWeights.Add( nameof(KillQuestSo), Constants.OneOptionQuestLineWeight );
               //masteryQuestWeights.Add( Constants.DAMAGE_QUEST, Constants.TwoOptionQuestLineWeight );
                masteryQuestWeights.Add( Constants.EmptyQuest, Constants.OneOptionQuestEmptyWeight );
                return masteryQuestWeights;
            } 
        }

        public override QuestSo DefineQuestSo ( List<QuestSo> questSos, NpcSo npcInCharge, in NarrativeSettings narrativeSettings, Language language)
        {
            switch ( SymbolType )
            {
                case Constants.KillQuest:
                    return CreateAndSaveKillQuestSo(questSos, npcInCharge, narrativeSettings.PossibleWeapons, narrativeSettings.EnemiesToKill, language);
                case Constants.DamageQuest:
                    return CreateAndSaveDamageQuestSo(questSos, npcInCharge, narrativeSettings.PossibleWeapons, language);
                default:
                    Debug.LogError("help something went wrong! - Mastery doesn't contain symbol: "+SymbolType);
                break;
            }

            return null;
        }

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            throw new NotImplementedException();
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            throw new NotImplementedException();
        }

        public override void CreateQuestString(Enums.Language l)
        {
            throw new NotImplementedException();
        }

        private static KillQuestSo CreateAndSaveKillQuestSo(List<QuestSo> questSos, NpcSo npcInCharge, WeaponTypeRuntimeSetSO enemyTypes, RangedInt enemiesToKill, Language language)
        {
            var killQuest = CreateInstance<KillQuestSo>();
            var selectedEnemyTypes = new EnemiesByType ();
            var questId = killQuest.GetInstanceID();
            var selectedEnemyType = enemyTypes.GetRandomItem();
            var nEnemiesToKill = RandomSingleton.GetInstance().Random.Next( enemiesToKill.Max - enemiesToKill.Min) + enemiesToKill.Min;
            for (var i = 0; i < nEnemiesToKill; i++)
            {
                selectedEnemyTypes.EnemiesByTypeDictionary.AddItemWithId(selectedEnemyType, questId);
            }
            killQuest.Init(KillEnemyTypesToString(selectedEnemyTypes, language), false, questSos.Count > 0 
                ? questSos[^1] : null, selectedEnemyTypes);
            killQuest.NpcInCharge = npcInCharge;
            if (questSos.Count > 0)
            {
                questSos[^1].Next = killQuest;
            }
            
            questSos.Add(killQuest);
            return killQuest;
        }

        private static DamageQuestSo CreateAndSaveDamageQuestSo(List<QuestSo> questSos, NpcSo npcInCharge, WeaponTypeRuntimeSetSO enemyTypes, Language language)
        {
            var damageQuest = ScriptableObject.CreateInstance<DamageQuestSo>();
            var selectedEnemyType = enemyTypes.GetRandomItem();
            var totalDamage = RandomSingleton.GetInstance().Random.Next(100) + 20;
            damageQuest.Init(selectedEnemyType.RealTypeName(language), false, 
                questSos.Count > 0 ? questSos[^1] : null, selectedEnemyType, totalDamage);
            
            if (questSos.Count > 0)
            {
                questSos[^1].Next = damageQuest;
            }
            damageQuest.NpcInCharge = npcInCharge;
            questSos.Add(damageQuest);
            return damageQuest;
        }

        private static string KillEnemyTypesToString(EnemiesByType  selectedEnemyTypes, Language language)
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < selectedEnemyTypes.EnemiesByTypeDictionary.Count; i++)
            {
                var typeAmountPair = selectedEnemyTypes.EnemiesByTypeDictionary.ElementAt(i);

                if (language == Language.Portuguese)
                    stringBuilder.Append($"Derrote {typeAmountPair.Value} {typeAmountPair.Key}");
                else if (language == Language.English)
                    stringBuilder.Append($"Kill {typeAmountPair.Value} {typeAmountPair.Key}");

                if (typeAmountPair.Value.QuestIds.Count > 1)
                {
                    stringBuilder.Append("s");
                }
                if (i < (selectedEnemyTypes.EnemiesByTypeDictionary.Count - 1))
                {
                    if (language == Language.Portuguese)
                        stringBuilder.Append(" e ");
                    else
                        stringBuilder.Append(" and ");
                }
            }
            return stringBuilder.ToString();
        }
    }
}