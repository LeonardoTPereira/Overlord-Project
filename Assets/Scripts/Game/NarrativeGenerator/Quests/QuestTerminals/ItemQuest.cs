using System.Collections.Generic;
using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests
{
    public class ItemQuestSO : QuestSO
    {
        public Dictionary<ItemSO, int> ItemsToCollectByType { get; set; }

        public override void Init()
        {
            base.Init();
            ItemsToCollectByType = new Dictionary<ItemSO, int>();
        }

        public void Init(string name, bool endsStoryLine, QuestSO previous, Dictionary<ItemSO, int> itemsByType)
        {
            base.Init(name, endsStoryLine, previous);
            ItemsToCollectByType = itemsByType;
        }

        public void AddItem(ItemSO item, int amount)
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
