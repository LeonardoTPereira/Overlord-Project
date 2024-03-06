using ScriptableObjects.SerializableDictionaryLite;
using System;
using System.Collections.Generic;

namespace Game.DataCollection
{
    [Serializable]
    public class DungeonDataByAttempt : SerializableDictionaryBase<string, List<DungeonData>>
    {
    }
}