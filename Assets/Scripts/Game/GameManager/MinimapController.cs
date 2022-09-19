using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.GameManager
{
    public class MinimapController : MonoBehaviour
    {
        private enum ViewStates
        {
            Small,
            None,
            Fullscreen,
            Count
        }
        [SerializeField] private InputActionAsset actions;


        public static event EventHandler FullscreenUIEvent;
        public static event EventHandler ExitFullscreenUIEvent;

        [field: SerializeField] private GameObject FullMap {get; set; }
        [field: SerializeField] private GameObject MiniMap {get; set; }
        private ViewStates _viewState; 
        [SerializeField] private InputActionReference interactAction;
        [field: SerializeField] private Camera MinimapCamera { get; set; }

        private void Awake()
        {
            _viewState = ViewStates.Small;
        }

        private void Start()
        {
            var mainCamera = Camera.main;
            if (mainCamera == null) return;
            var minimapCameraTransform = MinimapCamera.transform;
            var mainCameraTransform = mainCamera.transform;
            minimapCameraTransform.position = mainCameraTransform.position;
            minimapCameraTransform.parent = mainCameraTransform;
        }

        private void OnEnable() {
            actions.Enable();
            interactAction.action.performed += ChangeMinimapView;
        }

        private void OnDisable() {
            actions.Disable();
            interactAction.action.performed -= ChangeMinimapView;
        }
        
        public void ChangeMinimapView(InputAction.CallbackContext context)
        {
            switch (_viewState)
            {
                case ViewStates.Fullscreen:
                    ExitFullscreenUIEvent?.Invoke(null, EventArgs.Empty);
                    FullMap.SetActive(false);
                    break;
                case ViewStates.Small:
                    MiniMap.SetActive(false);
                    break;
            }

            _viewState = (ViewStates) (((int)_viewState + 1) % (int)ViewStates.Count);
            switch (_viewState)
            {
                case ViewStates.Fullscreen:
                    FullscreenUIEvent?.Invoke(null, EventArgs.Empty);
                    MinimapCamera.orthographicSize = 240;
                    FullMap.SetActive(true);
                    break;
                case ViewStates.Small:
                    MinimapCamera.orthographicSize = 120;
                    MiniMap.SetActive(true);
                    break;
            }
        }

    }
}