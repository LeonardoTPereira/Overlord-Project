using System;
using UnityEngine;

namespace Game.NarrativeGenerator.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/GetQuest"), Serializable]
    class GetQuestSO : ItemQuestSO
    {
        private const int chance = 1/2;
    }
}
