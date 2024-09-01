using System;
using System.Collections;
using Game.Events;
using Game.ExperimentControllers;
using Game.LevelManager.DungeonManager;
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
        
        public static event EventHandler FullscreenUIEvent;
        public static event EventHandler ExitFullscreenUIEvent;
        public static event EventHandler EndedMiniMapEvent;

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
            interactAction.action.performed += ChangeMinimapView;
            RoomBhv.ShowRoomOnMiniMapEventHandler += ShowRoomOnMap;
            ArenaController.ShowRoomOnMiniMapEventHandler += ShowRoomOnMap;
        }

        private void OnDisable() {
            interactAction.action.performed -= ChangeMinimapView;
            RoomBhv.ShowRoomOnMiniMapEventHandler -= ShowRoomOnMap;
            ArenaController.ShowRoomOnMiniMapEventHandler -= ShowRoomOnMap;
        }
        
        private void ShowRoomOnMap(object sender, ShowRoomOnMiniMapEventArgs eventArgs)
        {

            ExitCurrentState();
            _viewState = ViewStates.Fullscreen;
            EnterNewState();
            StartCoroutine(MoveCameraToRoomAndBack(eventArgs.RoomPosition));
        }

        private IEnumerator MoveCameraToRoomAndBack(Vector3 destination)
        {
            var cameraPosition = MinimapCamera.transform.position;
            yield return StartCoroutine(MinimapCamera.GetComponent<MinimapCameraController>().SmoothlyMoveCameraToRoom(destination));
            yield return new WaitForSeconds(0.5f);
            MinimapCamera.transform.position = cameraPosition;
            ExitCurrentState();
            _viewState = ViewStates.Small;
            EnterNewState();
            EndedMiniMapEvent?.Invoke(null, EventArgs.Empty);
        }
        
        private void ChangeMinimapView(InputAction.CallbackContext context)
        {
            ExitCurrentState();

            UpdateState();
            
            EnterNewState();
        }

        private void UpdateState()
        {
            _viewState = (ViewStates) (((int) _viewState + 1) % (int) ViewStates.Count);
        }

        private void EnterNewState()
        {
            switch (_viewState)
            {
                case ViewStates.Fullscreen:
                    GoToFullScreenMode();
                    break;
                case ViewStates.Small:
                    GoToMinimapMode();
                    break;
            }
        }

        private void ExitCurrentState()
        {
            switch (_viewState)
            {
                case ViewStates.Fullscreen:
                    ExitFullscreen();
                    break;
                case ViewStates.Small:
                    ExitMinimapMode();
                    break;
            }
        }

        private void ExitMinimapMode()
        {
            MiniMap.SetActive(false);
        }

        private void GoToMinimapMode()
        {
            MinimapCamera.orthographicSize = 120;
            MiniMap.SetActive(true);
        }

        private void ExitFullscreen()
        {
            ExitFullscreenUIEvent?.Invoke(null, EventArgs.Empty);
            FullMap.SetActive(false);
        }

        private void GoToFullScreenMode()
        {
            FullscreenUIEvent?.Invoke(null, EventArgs.Empty);
            MinimapCamera.orthographicSize = 240;
            FullMap.SetActive(true);
        }
    }
}