using System;
using UnityEngine;

namespace Game.Events
{
    public delegate void InitializePlayerHealthEvent(object sender, InitializePlayerHealthEventArgs e);
    public class  InitializePlayerHealthEventArgs : EventArgs
    {
        public int PlayerHealth => _playerHealth;
        private int _playerHealth;

        public InitializePlayerHealthEventArgs(int playerHealth)
        {
            _playerHealth = playerHealth;
        }
    }
}