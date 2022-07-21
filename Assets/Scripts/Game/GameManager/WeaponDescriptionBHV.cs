using Game.EnemyGenerator;
using ScriptableObjects;
using TMPro;
using UnityEngine;

public class WeaponDescriptionBHV : MonoBehaviour
{
    TextMeshProUGUI weaponDialogue;
    ProjectileTypeSO weaponSO;
    // Start is called before the first frame update
    void Start()
    {
        weaponDialogue = GetComponent<TextMeshProUGUI>();
    }

    protected void OnEnable()
    {
        WeaponSelectionButtonBHV.SelectWeaponButtonEvent += ShowWeaponInfo;
    }

    protected void OnDisable()
    {
        WeaponSelectionButtonBHV.SelectWeaponButtonEvent -= ShowWeaponInfo;
    }

    protected void ShowWeaponInfo(object sender, LoadWeaponButtonEventArgs eventArgs)
    {
        weaponSO = eventArgs.ProjectileSO;
        weaponDialogue.text = WeaponToString();
    }

    protected string WeaponToString()
    {
        string str = "";
        str += "\"" + weaponSO.description + "\"\n";
        str += "Damage: " + weaponSO.damage + "\n";
        str += "Projectile Speed: " + weaponSO.moveSpeed + "\n";
        str += "Attack Speed: " + weaponSO.atkSpeed + "\n";
        return str;
    }
}
