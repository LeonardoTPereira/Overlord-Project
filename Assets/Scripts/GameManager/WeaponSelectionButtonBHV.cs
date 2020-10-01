using EnemyGenerator;
using System.Collections;
using System.Collections.Generic;
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
    public delegate void SelectWeaponButtonEvent(ProjectileTypeSO projectileSO);
    public static event SelectWeaponButtonEvent selectWeaponButtonEvent;

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
        selectWeaponButtonEvent += DisableOutline;
    }

    protected void OnDisable()
    {
        selectWeaponButtonEvent -= DisableOutline;
    }

    protected void DisableOutline(ProjectileTypeSO projectileSO)
    {
        if (!this.projectileSO.Equals(projectileSO))
            outline.enabled = false;
    }

    void OnSelectWeapon()
    {
        outline.enabled = true;
        selectWeaponButtonEvent(projectileSO);
    }
}
