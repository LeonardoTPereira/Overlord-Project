using UnityEngine;
using UnityEngine.InputSystem;

namespace Fog.Dialogue.Samples {
    public class InputActivator : MonoBehaviour {
        [SerializeField] private InputActionAsset actions;

        private void OnEnable() {
            actions.Enable();
        }

        private void OnDisable() {
            actions.Disable();
        }
    }
}
