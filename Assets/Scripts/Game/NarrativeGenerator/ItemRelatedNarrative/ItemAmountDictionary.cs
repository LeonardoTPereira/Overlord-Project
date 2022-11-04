using System;
using System.Collections;
using ScriptableObjects;
using Util;

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

        public new ItemAmountDictionary Clone()
        {
            return new ItemAmountDictionary(this);
        }
    }
}