using System;
using UnityEngine;

namespace Game.Events
{
    public delegate void PlayerIsDamagedEvent(object sender, PlayerIsDamagedEventArgs e);
    public class  PlayerIsDamagedEventArgs : EventArgs
    {
        private int _enemyIndex;
        private int _damageDone;
        private int _playerHealth;
        private Vector3 _impactDirection;

        public PlayerIsDamagedEventArgs(int enemyIndex, int damageDone, int playerHealth, Vector3 impactDirection)
        {
            EnemyIndex = enemyIndex;
            DamageDone = damageDone;
            PlayerHealth = playerHealth;
            ImpactDirection = impactDirection;
        }

        public int EnemyIndex { get => _enemyIndex; set => _enemyIndex = value; }
        public int DamageDone { get => _damageDone; set => _damageDone = value; }
        public int PlayerHealth { get => _playerHealth; set => _playerHealth = value; }
        public Vector3 ImpactDirection { get => _impactDirection; set => _impactDirection = value; }
    }
}