using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects;

public class PointsOrCollectiblesLoader : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private MovementTypeSO movementType;

    public static PointsOrCollectiblesLoader Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void LoadCollectibles(int amount)
    {
        if (movementType != null)
            for (int i = 0; i < amount; i++)
            {
                Vector2 spawnPosition = EnemyLoader.Instance.GetSpawnPosition(movementType);
                Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
            }
    }
}
