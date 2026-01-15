using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects;

public class PointsOrCollectiblesLoader : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private MovementTypeSO movementType;

    void Start()
    {
    }

    public void SpawnObjects(int amount)
    {
        if (movementType != null)
            for (int i = 0; i < amount; i++)
            {
                Vector2 spawnPosition = EnemyLoader.Instance.GetSpawnPosition(movementType);
                Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
            }
    }
}
