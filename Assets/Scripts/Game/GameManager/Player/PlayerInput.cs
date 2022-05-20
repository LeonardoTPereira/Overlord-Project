using System;
using Game.Dialogues;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GameManager.Player
{
    public abstract class PlayerInput : MonoBehaviour
    {
        protected Animator PlayerAnimator { get; private set; }

        protected virtual void OnEnable()
        {
            DialogueController.DialogueOpenEventHandler += StopInput;
            DialogueController.DialogueCloseEventHandler += StartInput;
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
            DialogueController.DialogueOpenEventHandler -= StopInput;
            DialogueController.DialogueCloseEventHandler -= StartInput;
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