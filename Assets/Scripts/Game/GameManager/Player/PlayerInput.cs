using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GameManager.Player
{
    public abstract class PlayerInput : MonoBehaviour
    {
        protected Animator PlayerAnimator { get; private set; }

        protected virtual void OnEnable()
        {
            NpcController.DialogueOpenEventHandler += StopInput;
            NpcController.DialogueCloseEventHandler += StartInput;
            PlayerController.PlayerDeathEventHandler += StopInput;
            PlayerController.SceneLoaded += StartInput;
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }
        
        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if(scene.name == "Overworld" || scene.name == "LevelWithEnemies")
            {
                StartInput(null, EventArgs.Empty);
            }
        }
        
        protected virtual void OnDisable()
        {
            NpcController.DialogueOpenEventHandler -= StopInput;
            NpcController.DialogueCloseEventHandler -= StartInput;
            PlayerController.PlayerDeathEventHandler -= StopInput;
            PlayerController.SceneLoaded -= StartInput;
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        protected virtual void Start()
        {
            PlayerAnimator = GetComponent<Animator>();
        }

        protected abstract void StartInput(object sender, EventArgs eventArgs);
        protected abstract void StopInput(object sender, EventArgs eventArgs);

    }
}