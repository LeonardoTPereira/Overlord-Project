using UnityEngine;
using UnityEngine.InputSystem;
namespace Game.Dialogues
{
    public class DialogueInputActivator : MonoBehaviour {
        [SerializeField] private InputActionAsset actions;

        private void OnEnable() {
            actions.Enable();
        }

        private void OnDisable() {
            actions.Disable();
        }
    }
}