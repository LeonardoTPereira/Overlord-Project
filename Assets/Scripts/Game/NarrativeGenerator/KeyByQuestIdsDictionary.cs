using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.SerializableDictionaryLite;

namespace Game.NarrativeGenerator
{
    [Serializable]
    public class KeyByQuestIdsDictionary<T> : SerializableDictionaryBase<T, LinkedList<int>>
    {
        public KeyByQuestIdsDictionary()
        {
        }

        public KeyByQuestIdsDictionary(KeyByQuestIdsDictionary<T> keyByQuestIdsDictionary)
        {
            foreach (var data in keyByQuestIdsDictionary)
            {
                Add(data.Key, data.Value);
            }
        }

        public void RemoveItemWithId(T key, int id)
        {
            if (!ContainsKey(key)) return;
            this[key].Remove(id);
            if (this[key].Count == 0)
            {
                Remove(key);
            }
        }

        public void AddItemWithId(T key, int questId)
        {
            if (!ContainsKey(key))
            {
                Add(key, new LinkedList<int>());
            }

            this[key].AddFirst(questId);
        }
        
        public new KeyByQuestIdsDictionary<T> Clone()
        {
            return new KeyByQuestIdsDictionary<T>(this);
        }
    }
}