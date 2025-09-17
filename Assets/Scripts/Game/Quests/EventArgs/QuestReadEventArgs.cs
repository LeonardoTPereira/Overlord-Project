using Game.NPCs;
using ScriptableObjects;

namespace Game.Quests
{
    public class QuestReadEventArgs : QuestElementEventArgs
    {
        public ItemSo ReadableItem {get; set; }

        public QuestReadEventArgs(ItemSo readableItem, int questId):base(questId)
        {
            ReadableItem = readableItem;
        }
    }
}