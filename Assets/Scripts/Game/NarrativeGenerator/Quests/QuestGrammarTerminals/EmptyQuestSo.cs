using UnityEngine;
using System;
using System.Collections.Generic;
using Game.NarrativeGenerator;
using Game.NPCs;
using Util;
using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests
{
    // [CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/GetQuest"), Serializable]
    public class EmptyQuestSo : QuestSo, Symbol
    {
        public override string symbolType { 
            get { return Constants.EMPTY_QUEST;} 
        }
        public override bool canDrawNext { 
            get { return false; } 
        }
    }
}