using UnityEngine;
namespace Game.Quests
{
    public class PlaceholderCountableQuestElement : PlaceholderCollectableQuestElement
    {
        public int amount = 10;

        protected override void ResolveQuest()
        {
            OnQuestTaskResolved(new QuestCountableElementEventArgs(true, amount));
        }
    }
}