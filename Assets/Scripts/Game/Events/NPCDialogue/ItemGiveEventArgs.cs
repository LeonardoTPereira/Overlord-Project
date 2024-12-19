using System;
using ScriptableObjects;

namespace Game.Events
{
    public delegate void ItemGiveEvent(object sender, ItemGiveEventArgs e);

    public class ItemGiveEventArgs : EventArgs
    {
        public ItemSo GivenItem { get; set; }
        public int QuestId { get; set; }
        public ItemGiveEventArgs(ItemSo givenItem, int questId)
        {
            GivenItem = givenItem;
            QuestId = questId;
        }
    }
}