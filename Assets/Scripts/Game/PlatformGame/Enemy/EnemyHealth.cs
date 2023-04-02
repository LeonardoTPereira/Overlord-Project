using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Gameplay;
using ScriptableObjects;
using TMPro;
using PlatformGame.Weapons;
using PlatformGame.Util;

namespace PlatformGame.Enemy
{
    public class EnemyHealth : HealthController
    {
        [SerializeField] private int _minimumHealth = 1;
        [SerializeField] private int _maximumHealth = 6;

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
            maxHealth = (int)Mathf.Round(CalculateValueEnemySoTopdownToPlatform.TopdownToPlatform((float)enemySo.health, (float)_minimumHealth, (float)_maximumHealth, 1.0f, 6.0f));
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