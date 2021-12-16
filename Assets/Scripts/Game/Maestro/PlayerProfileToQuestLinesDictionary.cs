using RotaryHeart.Lib.SerializableDictionary;
using System;
using Game.NarrativeGenerator.Quests;

namespace Game.Maestro
{
    [Serializable]
    public class PlayerProfileToQuestLinesDictionary : SerializableDictionaryBase<string, QuestLineList>
    {
    }
}