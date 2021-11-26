using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.Quests.QuestTerminals;
using ScriptableObjects;
using UnityEngine;

namespace Game.NarrativeGenerator.Quests.nao_terminais
{
    public class Kill : NonTerminalQuest
    {
        public Kill(int lim, Dictionary<string, int> questWeightsByType) : base(lim, questWeightsByType)
        {
            maxQuestChance = 2.5f;
        }

        public void Option(List<QuestSO> questSos, List<NpcSO> possibleNpcSos, WeaponTypeRuntimeSetSO enemyTypes)
        {
            DrawQuestType();
            DefineNextQuest(questSos, possibleNpcSos, enemyTypes);
        }
    
        protected void DefineNextQuest(List<QuestSO> questSos, List<NpcSO> possibleNpcSos, WeaponTypeRuntimeSetSO enemyTypes)
        {

            if (r <= 2.3)
            {
                CreateAndSaveKillQuestSo(questSos, enemyTypes);
                Talk t = new Talk(lim, QuestWeightsByType);
                t.Option(questSos, possibleNpcSos);
                Option(questSos, possibleNpcSos, enemyTypes);
            }
            else
            {
                CreateAndSaveKillQuestSo(questSos, enemyTypes);
            }
        }

        private static void CreateAndSaveKillQuestSo(List<QuestSO> questSos, WeaponTypeRuntimeSetSO enemyTypes)
        {
            var killQuest = ScriptableObject.CreateInstance<KillQuestSO>();
            var selectedEnemyTypes = new EnemiesByType ();
            //TODO select more enemies
            var selectedEnemyType = enemyTypes.GetRandomItem();
            selectedEnemyTypes.EnemiesByTypeDictionary.Add(selectedEnemyType, 1);
            killQuest.Init(EnemyTypesToString(selectedEnemyTypes), false, questSos.Count > 0 ? questSos[questSos.Count-1] : null, selectedEnemyTypes);
            killQuest.SaveAsAsset();
            questSos.Add(killQuest);
        }
        
        private static string EnemyTypesToString(EnemiesByType  selectedEnemyTypes)
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < selectedEnemyTypes.EnemiesByTypeDictionary.Count; i++)
            {
                var typeAmountPair = selectedEnemyTypes.EnemiesByTypeDictionary.ElementAt(i);
                stringBuilder.Append($"Kill {typeAmountPair.Value} {typeAmountPair.Key}");
                if (typeAmountPair.Value > 1)
                {
                    stringBuilder.Append("s");
                }
                if (i < (selectedEnemyTypes.EnemiesByTypeDictionary.Count - 1))
                {
                    stringBuilder.Append(" and ");
                }
            }
            return stringBuilder.ToString();
        }
    }
}