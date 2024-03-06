using Game.NarrativeGenerator.Quests;
using ScriptableObjects;
using ScriptableObjects.SerializableDictionaryLite;
using System;

namespace Game.EnemyManager
{
    [Serializable]
    public class EnemyByAmountDictionary : SerializableDictionaryBase<EnemySO, QuestIdList>
    {

    }
}