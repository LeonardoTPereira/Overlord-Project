using Game.Events;
using Game.GameManager;
using UnityEngine;

namespace Game.DataCollection
{
    public class EnemyData : ScriptableObject
    {
        //TODO calculate damage done by each enemy SO
        //TODO save data of which enemies worked together on each room, counting clear time and damage done to player
        //This way, we know best offensive and defensive teams

        private void OnEnable()
        {
            HealthController.PlayerIsDamagedEventHandler += OnPlayerDamage;
        }
        
        private void OnDisable()
        {
            HealthController.PlayerIsDamagedEventHandler -= OnPlayerDamage;
        }
        
        private void OnPlayerDamage(object sender, PlayerIsDamagedEventArgs eventArgs)
        {
            //TODO get enemySO
        }
    }
}