using System;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using ScriptableObjects.SerializableDictionaryLite;

namespace Game.EnemyManager
{
    [Serializable]
    public class EnemyByAmountDictionary : SerializableDictionaryBase<EnemySO, LinkedList<int>>
    {
        
    }
}