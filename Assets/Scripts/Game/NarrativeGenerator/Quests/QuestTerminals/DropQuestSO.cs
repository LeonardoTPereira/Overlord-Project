using System;
using ScriptableObjects;
using UnityEngine;

namespace Game.NarrativeGenerator.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/DropQuest"), Serializable]
    class DropQuestSO : ItemQuestSO
    {
        EnemySO enemySO;
        float dropChance;
    }

    public override Init()
    {
        
    }
}
