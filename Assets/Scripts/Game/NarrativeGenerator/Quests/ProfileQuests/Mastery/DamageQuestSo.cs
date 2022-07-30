using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class DamageQuestSo : MasteryQuestSo
    {
        public override string symbolType {
            get { return Constants.DAMAGE_QUEST; }
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, EnemiesByType  enemiesByType)
        {
            // base.Init(questName, endsStoryLine, previous);
            // EnemiesToKillByType = enemiesByType;
        }
        public void Init(string questName, bool endsStoryLine, QuestSo previous, Dictionary<float, int> enemiesByFitness)
        {
            // base.Init(questName, endsStoryLine, previous);
            // EnemiesToKillByFitness = enemiesByFitness;
        }
    }
}