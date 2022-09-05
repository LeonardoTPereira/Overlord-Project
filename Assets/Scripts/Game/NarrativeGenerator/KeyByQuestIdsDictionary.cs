using System;
using System.Collections;
using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
using ScriptableObjects.SerializableDictionaryLite;

namespace Game.NarrativeGenerator
{
    [Serializable]
    public class KeyByQuestIdsDictionary<T> : SerializableDictionaryBase<T, QuestIdList>
    {
        public KeyByQuestIdsDictionary()
        {
        }

        public KeyByQuestIdsDictionary(KeyByQuestIdsDictionary<T> keyByQuestIdsDictionary)
        {
            foreach (var itemQuestPair in keyByQuestIdsDictionary)
            {
                Add(itemQuestPair.Key, new QuestIdList(itemQuestPair.Value));
            }
        }

        public void RemoveItemWithId(T key, int id)
        {
            if (!ContainsKey(key)) return;
            this[key].QuestIds.Remove(id);
            if (this[key].QuestIds.Count == 0)
            {
                Remove(key);
            }
        }

        
        public void AddItemWithId(T key, int questId)
        {
            if (!ContainsKey(key))
            {
                Add(key, new QuestIdList());
            }

            this[key].Add(questId);
        }
        
        public new object Clone()
        {
            return new KeyByQuestIdsDictionary<T>(this);
        }
    }
}