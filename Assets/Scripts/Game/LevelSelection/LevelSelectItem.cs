using Game.SaveLoadSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.LevelSelection
{
    public class LevelSelectItem : MonoBehaviour, ISelectHandler, IDeselectHandler, ISaveable
    {
        public string LevelId { get; set; }

        [field: SerializeField] public LevelData Level { get; set; }

        [SerializeField] private LevelDescription levelDescription;
        [SerializeField] private Image portrait;

        public bool IsSelected { get; private set; }

        private void Start()
        {
            IsSelected = false;
            if (!Level.HasCompleted()) return;
            GetComponent<Selectable>().interactable = false;
            portrait.color = Color.gray;
        }

        public void OnSelect(BaseEventData eventData)
        {
            IsSelected = true;
            levelDescription.CreateDescriptions(Level);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            IsSelected = false;
        }

        public object SaveState()
        {
            if (Level is RealTimeLevelData realTimeLevelData)
            {
                return realTimeLevelData.SaveState();
            }

            return null;
        }

        public void LoadState(object state)
        {
            if (Level is RealTimeLevelData realTimeLevelData)
            {
                realTimeLevelData.LoadState(state);
            }        
        }
    }
}