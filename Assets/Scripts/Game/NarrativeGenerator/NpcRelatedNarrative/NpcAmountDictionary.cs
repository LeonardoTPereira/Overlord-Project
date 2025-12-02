using System;
using Overlord.NarrativeGenerator.NPCs;
using Overlord.NarrativeGenerator.Quests;
using ScriptableObjects.SerializableDictionaryLite;

namespace Game.NarrativeGenerator.NpcRelatedNarrative
{
    [Serializable]
    public class NpcAmountDictionary : SerializableDictionaryBase<NpcSo, QuestLine>
    {
    }
}