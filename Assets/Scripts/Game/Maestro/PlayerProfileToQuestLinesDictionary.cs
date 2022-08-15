using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
using ScriptableObjects.SerializableDictionaryLite;

namespace Game.Maestro
{
    [Serializable]
    public class PlayerProfileToQuestLinesDictionary : SerializableDictionaryBase<string, List<QuestLineList>>
    {
    }
}