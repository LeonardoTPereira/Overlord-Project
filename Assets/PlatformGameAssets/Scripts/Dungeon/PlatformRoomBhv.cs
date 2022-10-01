using System.Collections;
using System.Collections.Generic;
using Game.LevelManager.DungeonManager;
using PlatformGame.Enemy;
using UnityEngine;

namespace PlatformGame.Dungeon
{
    public class PlatformRoomBhv : RoomBhv
    {
        //Change enemy spawners later...
        //Do not forget EnemyKilledHandler event inside SpawnEnemies() to remove from enemy dictionary...

        protected override void RemoveFromDictionaryWhenEnemyDied(GameObject enemy)
        {
            enemy.GetComponent<EnemyHealth>().EnemyKilledHandler += RemoveFromDictionary;
        }

        protected override void SetEnemySpawners(float centerX, float centerY)
        {
            
        }
    }
}