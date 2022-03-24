using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
using Game.NPCs;
using ScriptableObjects;
using ScriptableObjects.SerializableDictionaryLite;

namespace Game.NarrativeGenerator.NpcRelatedNarrative
{
    [Serializable]
    public class NpcAmountDictionary : SerializableDictionaryBase<NpcSo, QuestList>
    {
    }
}