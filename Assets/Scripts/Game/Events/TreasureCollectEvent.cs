using System;
using ScriptableObjects;

namespace Game.Events
{
    public delegate void TreasureCollectEvent(object sender, TreasureCollectEventArgs e);

    public class TreasureCollectEventArgs : EventArgs
    {
        public int QuestId { get; set; }
        public ItemSo Item { get; set; }

        public TreasureCollectEventArgs(ItemSo item, int questId)
        {
            Item = item;
            QuestId = questId;
        }
    }
}