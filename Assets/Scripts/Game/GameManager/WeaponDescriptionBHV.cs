using Game.Events;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace Game.GameManager
{
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
            str += "Dano: " + weaponSO.damage + "\n";
            str += "Velocidade do projétil: " + weaponSO.moveSpeed + "\n";
            str += "Velocidade de ataque: " + weaponSO.atkSpeed + "\n";
            return str;
        }
    }
}
