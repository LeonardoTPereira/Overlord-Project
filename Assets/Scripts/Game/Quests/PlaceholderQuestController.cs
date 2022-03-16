using UnityEngine;

namespace Game.Quests
{
    public class PlaceholderQuestController : MonoBehaviour
    {
        [SerializeField] private int countableQuestElements = 0;
        [SerializeField] private int completedTasks = 0;

        public int CountableQuestElements
        {
            get => countableQuestElements;
            set => countableQuestElements = value;
        }

        public int CompletedTasks
        {
            get => completedTasks;
            set => completedTasks = value;
        }

        private void OnEnable()
        {
            AbstractQuestElement.QuestElementEventHandler += UpdateQuest;
        }

        private void OnDisable()
        {
            AbstractQuestElement.QuestElementEventHandler -= UpdateQuest;
        }

        private void UpdateQuest(object sender, QuestElementEventArgs args)
        {
            if (args is QuestCountableElementEventArgs eventArgs)
            {
                CountableQuestElements += eventArgs.Amount;
            }
            else
            {
                if (args.Success)
                {
                    CompletedTasks++;
                }
            }
        }
    }
}