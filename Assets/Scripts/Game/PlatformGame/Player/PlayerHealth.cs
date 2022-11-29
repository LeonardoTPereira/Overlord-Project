using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlatformGame.Player
{
    public class PlayerHealth : HealthController
    {
        public static event Action<int> InitializePlayerHealthEvent;
        public static event Action PlayerDiedEvent;
        public static event Action<int> PlayerTakeDamageEvent;

        protected override void InitializeHealth()
        {
            base.InitializeHealth();
            InitializePlayerHealthEvent?.Invoke(maxHealth);
        }

        protected override void Kill()
        {
            Debug.Log("GAME OVER");
            PlayerDiedEvent?.Invoke();
            base.Kill();
            SceneManager.LoadScene("ExperimentLevelSelector");
            Destroy(gameObject);
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            PlayerTakeDamageEvent?.Invoke(damage);
        }
    }
}
