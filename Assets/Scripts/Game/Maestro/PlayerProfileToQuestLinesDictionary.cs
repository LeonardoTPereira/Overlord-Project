using System;
using System.Collections.Generic;
using Overlord.NarrativeGenerator.Quests;
using ScriptableObjects.SerializableDictionaryLite;

namespace Game.Maestro
{
    [Serializable]
    public class PlayerProfileToQuestLinesDictionary : SerializableDictionaryBase<string, List<QuestLineList>>
    {
    }
}