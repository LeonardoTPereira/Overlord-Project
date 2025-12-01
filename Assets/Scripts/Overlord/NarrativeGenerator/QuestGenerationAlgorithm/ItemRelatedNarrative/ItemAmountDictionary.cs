using System;
using ScriptableObjects;

namespace Overlord.NarrativeGenerator.ItemRelatedNarrative
{
    [Serializable]
    // SerializableDictionaryBase<T, QuestIdList> -> <ItemSo, QuestIdList> -> <ItemSo, List<int>>
    public class ItemAmountDictionary : KeyByQuestIdsDictionary<ItemSo>
    {
        public ItemAmountDictionary()
        {
        }

        public ItemAmountDictionary(KeyByQuestIdsDictionary<ItemSo> keyByQuestIdsDictionary) : base(keyByQuestIdsDictionary)
        {
        }

        public new object Clone()
        {
            return new ItemAmountDictionary(this);
        }
        
    }
}