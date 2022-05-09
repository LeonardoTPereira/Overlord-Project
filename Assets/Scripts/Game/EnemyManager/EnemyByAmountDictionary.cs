using System;
using ScriptableObjects;
using ScriptableObjects.SerializableDictionaryLite;

namespace Game.EnemyManager
{
    [Serializable]
    public class EnemyByAmountDictionary : SerializableDictionaryBase<EnemySO, int>
    {
    }
}