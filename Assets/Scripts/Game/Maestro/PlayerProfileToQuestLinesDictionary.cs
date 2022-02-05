using System;
using Game.NarrativeGenerator.Quests;
using ScriptableObjects.SerializableDictionaryLite;

namespace Game.Maestro
{
    [Serializable]
    public class PlayerProfileToQuestLinesDictionary : SerializableDictionaryBase<string, QuestLineList>
    {
    }
}