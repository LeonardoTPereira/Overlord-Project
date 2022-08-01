using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.EnemyManager;
using Game.GameManager;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using UnityEngine;
using Util;

namespace Game.ExperimentControllers
{
    public class ArenaController : MonoBehaviour
    {
        [field: SerializeField] public List<Vector3> spawnPoints { get; set; }
        [field: SerializeField] public EnemyByAmountDictionary enemyByType { get; set; }

        private EnemyLoader _enemyLoader;
        private void Start()
        {
            _enemyLoader = GetComponent<EnemyLoader>();
            EnemyLoader.LoadEnemies(enemyByType.Keys.ToList());
            StartCoroutine(SpawnEnemies());
        }

        private IEnumerator SpawnEnemies()
        {
            yield return new WaitForEndOfFrame();
            var selectedSpawnPoints = new List<int>();
            foreach (var enemyByAmount in enemyByType)
            {
                for (var i = 0; i < enemyByAmount.Value; i++)
                {
                    int actualSpawn;
                    if (selectedSpawnPoints.Count >= spawnPoints.Count)
                    {
                        selectedSpawnPoints.Clear();
                    }
                    do
                    {
                        actualSpawn = RandomSingleton.GetInstance().Next(0, spawnPoints.Count);
                    } while (selectedSpawnPoints.Contains(actualSpawn));
                    var enemy = _enemyLoader.InstantiateEnemyFromScriptableObject(
                        new Vector3(spawnPoints[actualSpawn].x, spawnPoints[actualSpawn].y, 0f), 
                        transform.rotation, enemyByAmount.Key);
                    selectedSpawnPoints.Add(actualSpawn);
                }
            }
        }
    }
    
    
}