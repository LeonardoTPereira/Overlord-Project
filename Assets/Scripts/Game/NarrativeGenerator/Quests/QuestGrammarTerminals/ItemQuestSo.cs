using System.Collections.Generic;
using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests
{
    public class ItemQuestSo : QuestSO
    {
        public Dictionary<ItemSo, int> ItemsToCollectByType { get; set; }

        public override void Init()
        {
            base.Init();
            ItemsToCollectByType = new Dictionary<ItemSo, int>();
        }

        public void Init(string name, bool endsStoryLine, QuestSO previous, Dictionary<ItemSo, int> itemsByType)
        {
            base.Init(name, endsStoryLine, previous);
            ItemsToCollectByType = itemsByType;
        }

        public void AddItem(ItemSo item, int amount)
        {
            if (ItemsToCollectByType.TryGetValue(item, out var currentAmount))
            {
                ItemsToCollectByType[item] = currentAmount + amount;
            }
            else
            {
                ItemsToCollectByType.Add(item, amount);
            }
        }
    }

}
