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
        public static event Action<int> PlayerTakeHealEvent;

        [SerializeField] private GameObject _gameOverPanel;

        protected override void Start()
        {
            base.Start();
        }

        protected override void InitializeHealth()
        {
            base.InitializeHealth();
            InitializePlayerHealthEvent?.Invoke(maxHealth);
        }

        protected override void Kill()
        {
            Debug.Log("GAME OVER");
            PlayerDiedEvent?.Invoke();
            _gameOverPanel.SetActive(true);
            base.Kill();
        }

        public override void TakeDamage(int damage)
        {            
            if (base.GetCanTakeDamage())
            {
                PlayerTakeDamageEvent?.Invoke(damage);
                base.TakeDamage(damage);
            }
        }

        public override void ApplyHeal(int heal)
        {
            base.ApplyHeal(heal);
            PlayerTakeHealEvent?.Invoke(heal);
        }
    }
}
