using System;
using ScriptableObjects;

namespace Game.NarrativeGenerator.ItemRelatedNarrative
{
    [Serializable]
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