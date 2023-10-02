using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Gameplay;
using ScriptableObjects;
using TMPro;
using PlatformGame.Weapons;
using PlatformGame.Util;
using Game.Quests;

namespace PlatformGame.Enemy
{
    public class EnemyHealth : HealthController, IQuestElement
    {
        [SerializeField] private int _minimumHealth = 1;
        [SerializeField] private int _maximumHealth = 6;

        private EnemyAnimation _enemyAnimation;
        private EnemySO enemySo;        

        public EventHandler<EnemySO> EnemyKilledHandler;

        private int _questID;

        protected override void InitializeHealth()
        {
            base.InitializeHealth();
            _enemyAnimation = GetComponent<EnemyAnimation>();
        }

        public void LoadHealth(EnemySO enemySo, int questID)
        {
            _questID = questID;
            maxHealth = (int)Mathf.Round(CalculateValueEnemySoTopdownToPlatform.TopdownToPlatform((float)enemySo.health, (float)_minimumHealth, (float)_maximumHealth, 1.0f, 6.0f));
            this.enemySo = enemySo;
            InitializeHealth();
        }

        public override void TakeDamage(int damage)
        {
            ((IQuestElement)this).OnQuestTaskResolved(this, new QuestDamageEnemyEventArgs(enemySo.weapon, damage, _questID));
            base.TakeDamage(damage);
        }

        protected override void Kill()
        {
            base.Kill();
            EnemyKilledHandler?.Invoke(this, enemySo);
            // COLOCAR O EVENTO DE KILL DA QUEST COM O enemySo aqui
            ((IQuestElement)this).OnQuestTaskResolved(this, new QuestKillEnemyEventArgs(enemySo.weapon, _questID));
            _enemyAnimation.AnimateDeath();
        }
    }
}