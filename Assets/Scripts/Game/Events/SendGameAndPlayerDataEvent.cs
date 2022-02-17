using System;
using Game.EnemyGenerator;
using Game.DataInterfaces;

namespace Game.Events
{
    public delegate void SendGameAndPlayerDataEvent(object sender, SendGameAndPlayerDataArgs e);

    public class SendGameAndPlayerDataArgs : EventArgs
    {
        private PlayerAndGameplayData playerGameplayData;

        public SendGameAndPlayerDataArgs(PlayerAndGameplayData playerGameplayData)
        {
            PlayerGameplayData = playerGameplayData;
        }

        public PlayerAndGameplayData PlayerGameplayData
        {
            get => playerGameplayData;
            set => playerGameplayData = value;
        }
    }
}
