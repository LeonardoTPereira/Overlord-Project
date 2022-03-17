using UnityEngine;

namespace Game.Quests
{
    public abstract class AbstractQuestElement : MonoBehaviour
    {
        public static event QuestElementEvent QuestElementEventHandler;
        protected void OnQuestTaskResolved(QuestElementEventArgs args)
        {
            QuestElementEventHandler?.Invoke(this, args);
        }
    }
}