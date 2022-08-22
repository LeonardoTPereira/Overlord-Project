using System;
using System.Collections.Generic;
using System.Linq;
using Game.NarrativeGenerator.Quests;
using ScriptableObjects;
using ScriptableObjects.SerializableDictionaryLite;

namespace Game.EnemyManager
{
    [Serializable]
    public class EnemyByAmountDictionary : SerializableDictionaryBase<EnemySO, QuestIdList>
    {
        
    }
}