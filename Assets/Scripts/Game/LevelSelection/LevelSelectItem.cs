using Game.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.LevelSelection
{
    public class LevelSelectItem : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public string LevelId { get; set; }

        [field: SerializeField] public LevelData LevelData { get; set; }

        [SerializeField] private LevelDescription levelDescription;

        public bool IsSelected { get; private set; }

        private void Start()
        {
            IsSelected = false;
        }

        public void OnSelect(BaseEventData eventData)
        {
            IsSelected = true;
            levelDescription.CreateDescriptions(LevelData);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            IsSelected = false;
        }
    }
}