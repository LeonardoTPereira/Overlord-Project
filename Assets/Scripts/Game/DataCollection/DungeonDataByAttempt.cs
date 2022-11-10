using System;
using System.Collections.Generic;
using ScriptableObjects.SerializableDictionaryLite;
using UnityEngine;

namespace Game.DataCollection
{
    [Serializable]
    public class DungeonDataByAttempt : SerializableDictionaryBase<string, List<DungeonData>>
    {
    }
}