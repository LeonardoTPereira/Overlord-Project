using EnemyGenerator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionButtonBHV : MonoBehaviour
{
    [SerializeField]
    protected ProjectileTypeSO projectileSO;
    TextMeshProUGUI buttonName;
    Button button;
    Outline outline;
    public static event LoadWeaponButtonEvent SelectWeaponButtonEvent;

    // Start is called before the first frame update
    void Start()
    {
        buttonName = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        outline = GetComponent<Outline>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnSelectWeapon);
        buttonName.text = projectileSO.projectileName;
        buttonName.color = projectileSO.color;
        outline.enabled = false;
    }

    protected void OnEnable()
    {
        SelectWeaponButtonEvent += DisableOutline;
    }

    protected void OnDisable()
    {
        SelectWeaponButtonEvent -= DisableOutline;
    }

    protected void DisableOutline(object sender, LoadWeaponButtonEventArgs eventArgs)
    {
        if (!this.projectileSO.Equals(eventArgs.ProjectileSO))
            outline.enabled = false;
    }

    void OnSelectWeapon()
    {
        outline.enabled = true;
        SelectWeaponButtonEvent(this, new LoadWeaponButtonEventArgs(projectileSO));
    }
}
