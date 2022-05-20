using UnityEngine;

namespace Game.Quests
{
    public class PlaceholderCollectableQuestElement : AbstractQuestElement
    {
        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag("Player")) return;
            ResolveQuest();
            Destroy(gameObject);
        }

        protected virtual void ResolveQuest()
        {
            OnQuestTaskResolved(new QuestElementEventArgs(true));
        }
    }
}