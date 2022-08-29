using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace PlatformGame.Player
{
    public class PlayerHealth : HealthController
    {
        public static event Action<int> InitializePlayerHealthEvent;
        public static event Action PlayerDiedEvent;
        public static event Action<int> PlayerTakeDamageEvent;

        protected override void InitializeHealth()
        {
            InitializePlayerHealthEvent += FindObjectOfType<LifebarWindow>().UI_SetMaxLife; // Gambiarra temporária para Initialize..Event não retornar null
            base.InitializeHealth();
            InitializePlayerHealthEvent?.Invoke(maxHealth); // Estah retornando null
        }

        protected override void Kill()
        {
            Debug.Log("GAME OVER");
            PlayerDiedEvent?.Invoke();
            base.Kill();
            Destroy(gameObject);
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            PlayerTakeDamageEvent?.Invoke(damage);
        }
    }
}
