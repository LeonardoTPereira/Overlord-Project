using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Gameplay;
using ScriptableObjects;
using TMPro;
using PlatformGame.Weapons;

namespace PlatformGame.Enemy
{
    public class EnemyHealth : HealthController
    {
        private EnemyAnimation _enemyAnimation;

        private EnemySO enemySo;
        
        public EventHandler<EnemySO> EnemyKilledHandler;

        protected override void InitializeHealth()
        {
            base.InitializeHealth();
            _enemyAnimation = GetComponent<EnemyAnimation>();
        }

        public void LoadHealth(EnemySO enemySo)
        {
            maxHealth = enemySo.health;
            this.enemySo = enemySo;
            InitializeHealth();
        }

        protected override void Kill()
        {
            base.Kill();
            EnemyKilledHandler?.Invoke(this, enemySo);
            _enemyAnimation.AnimateDeath();
        }
    }

}