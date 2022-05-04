using Game.Events;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.MenuManager
{
    public class WeaponSelectionButtonBhv : MonoBehaviour
    {
        [SerializeField]
        protected ProjectileTypeSO projectileSo;
        private TextMeshProUGUI buttonName;
        private Button button;
        private Outline outline;
        public static event LoadWeaponButtonEvent SelectWeaponButtonEvent;

        // Start is called before the first frame update
        private void Start()
        {
            buttonName = GetComponentInChildren<TextMeshProUGUI>();
            button = GetComponent<Button>();
            outline = GetComponent<Outline>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnSelectWeapon);
            buttonName.text = projectileSo.projectileName;
            buttonName.color = projectileSo.color;
            outline.enabled = false;
        }

        private void OnEnable()
        {
            SelectWeaponButtonEvent += DisableOutline;
        }

        private void OnDisable()
        {
            SelectWeaponButtonEvent -= DisableOutline;
        }

        private void DisableOutline(object sender, LoadWeaponButtonEventArgs eventArgs)
        {
            if (!projectileSo.Equals(eventArgs.ProjectileSO))
                outline.enabled = false;
        }

        private void OnSelectWeapon()
        {
            outline.enabled = true;
            SelectWeaponButtonEvent?.Invoke(this, new LoadWeaponButtonEventArgs(projectileSo));
        }
    }
}
