using EnemyGenerator;
using System.Collections;
using System.Collections.Generic;
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
        WeaponSelectionButtonBHV.selectWeaponButtonEvent += ShowWeaponInfo;
    }

    protected void OnDisable()
    {
        WeaponSelectionButtonBHV.selectWeaponButtonEvent -= ShowWeaponInfo;
    }

    protected void ShowWeaponInfo(ProjectileTypeSO weaponSO)
    {
        this.weaponSO = weaponSO;
        weaponDialogue.text = WeaponToString();
    }

    protected string WeaponToString()
    {
        string str = "";
        str += "\""+ weaponSO.description +"\"\n";
        str += "Dano: " + weaponSO.damage + "\n";
        str += "Velocidade do projétil: " + weaponSO.moveSpeed + "\n";
        str += "Velocidade de ataque: " + weaponSO.atkSpeed + "\n";
        return str;
    }
}
