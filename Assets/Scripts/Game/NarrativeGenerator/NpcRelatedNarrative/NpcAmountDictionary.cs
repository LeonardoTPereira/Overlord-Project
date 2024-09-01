using System;
using Game.NarrativeGenerator.Quests;
using Game.NPCs;
using ScriptableObjects.SerializableDictionaryLite;

namespace Game.NarrativeGenerator.NpcRelatedNarrative
{
    [Serializable]
    public class NpcAmountDictionary : SerializableDictionaryBase<NpcSo, QuestLine>
    {
    }
}