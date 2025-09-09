using ScriptableObjects.SerializableDictionaryLite;
using System.Collections.Generic;

namespace Game.DataCollection
{
    public class RoomDataByVisit : SerializableDictionaryBase<int, List<RoomData>>
    {
    }
}