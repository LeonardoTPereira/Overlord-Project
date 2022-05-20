using System;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    [CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/GetQuest"), Serializable]
    public class GetQuestSo : ItemQuestSo
    {
        public override string symbolType {
            get { return Constants.GET_TERMINAL; }
        }
    }
}
