using System.Collections.Generic;
using ScriptableObjects.SerializableDictionaryLite;

namespace Game.DataCollection
{
    public class DungeonDataByAttempt : SerializableDictionaryBase<string, List<DungeonData>>
    {
    }
}