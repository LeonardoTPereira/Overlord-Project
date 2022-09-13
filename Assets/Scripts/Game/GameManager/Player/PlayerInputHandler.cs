using System;
using Game.Dialogues;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GameManager.Player
{
    public abstract class PlayerInputHandler : MonoBehaviour
    {
        protected Animator PlayerAnimator { get; private set; }

        protected virtual void OnEnable()
        {
            DialogueController.DialogueOpenEventHandler += StopInput;
            DialogueController.DialogueCloseEventHandler += StartInput;
            PlayerController.PlayerDeathEventHandler += StopInput;
            PlayerController.SceneLoaded += StartInput;
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
            MinimapController.FullscreenUIEvent += StopInput;
            MinimapController.ExitFullscreenUIEvent += StartInput;
        }
        
        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if(scene.name is "Overworld" or "LevelWithEnemies")
            {
                StartInput(null, EventArgs.Empty);
            }
        }
        
        protected virtual void OnDisable()
        {
            DialogueController.DialogueOpenEventHandler -= StopInput;
            DialogueController.DialogueCloseEventHandler -= StartInput;
            PlayerController.PlayerDeathEventHandler -= StopInput;
            PlayerController.SceneLoaded -= StartInput;
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
            MinimapController.FullscreenUIEvent -= StopInput;
            MinimapController.ExitFullscreenUIEvent -= StartInput;
        }

        protected virtual void Start()
        {
            PlayerAnimator = GetComponent<Animator>();
        }

        protected abstract void StartInput(object sender, EventArgs eventArgs);

        protected abstract void StopInput(object sender, EventArgs eventArgs);

    }
}