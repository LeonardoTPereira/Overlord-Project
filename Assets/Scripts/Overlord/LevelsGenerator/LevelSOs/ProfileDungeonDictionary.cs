using System;
using ScriptableObjects.SerializableDictionaryLite;

namespace Overlord.LevelGenerator.LevelSOs
{
    [Serializable]
    public class ProfileDungeonDictionary : SerializableDictionaryBase<string, DungeonFileSOList>
    {
    }
}