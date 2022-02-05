using System;
using ScriptableObjects.SerializableDictionaryLite;

namespace Game.LevelGenerator.LevelSOs
{
    [Serializable]
    public class ProfileDungeonDictionary : SerializableDictionaryBase<string, DungeonFileSOList>
    {
    }
}