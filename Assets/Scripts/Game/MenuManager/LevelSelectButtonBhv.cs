using Game.Events;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.MenuManager
{
    public class LevelSelectButtonBhv : MonoBehaviour
    {
        [SerializeField] private LevelConfigSO levelConfigSo;
        private TextMeshProUGUI buttonName;
        private Button button;
        private Outline outline;
        public static event LevelSelectEvent SelectLevelButtonEventHandler;

        private void Start()
        {
            buttonName = GetComponentInChildren<TextMeshProUGUI>();
            button = GetComponent<Button>();
            outline = GetComponent<Outline>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnSelectLevel);
            buttonName.text = levelConfigSo.levelName;
            outline.enabled = false;
        }
        protected void OnEnable()
        {
            SelectLevelButtonEventHandler += DisableOutline;
        }

        protected void OnDisable()
        {
            SelectLevelButtonEventHandler -= DisableOutline;
        }

        private void DisableOutline(object sender, LevelSelectEventArgs args)
        {
            if (!args.LevelSO.Equals(levelConfigSo))
                outline.enabled = false;
        }

        private void OnSelectLevel()
        {
            outline.enabled = true;
            SelectLevelButtonEventHandler?.Invoke(this, new LevelSelectEventArgs(levelConfigSo));
        }
    }
}
