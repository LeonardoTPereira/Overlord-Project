using Game.Events;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace Game.MenuManager
{
    public class WeaponDescriptionBhv : MonoBehaviour
    {
        private TextMeshProUGUI weaponDialogue;
        private ProjectileTypeSO weaponSo;
        // Start is called before the first frame update
        private void Start()
        {
            weaponDialogue = GetComponent<TextMeshProUGUI>();
        }

        protected void OnEnable()
        {
            WeaponSelectionButtonBhv.SelectWeaponButtonEvent += ShowWeaponInfo;
        }

        protected void OnDisable()
        {
            WeaponSelectionButtonBhv.SelectWeaponButtonEvent -= ShowWeaponInfo;
        }

        private void ShowWeaponInfo(object sender, LoadWeaponButtonEventArgs eventArgs)
        {
            weaponSo = eventArgs.ProjectileSO;
            weaponDialogue.text = WeaponToString();
        }

        private string WeaponToString()
        {
            var str = "";
            str += "\"" + weaponSo.description + "\"\n";
            str += "Dano: " + weaponSo.damage + "\n";
            str += "Velocidade do projétil: " + weaponSo.moveSpeed + "\n";
            str += "Velocidade de ataque: " + weaponSo.atkSpeed + "\n";
            return str;
        }
    }
}
