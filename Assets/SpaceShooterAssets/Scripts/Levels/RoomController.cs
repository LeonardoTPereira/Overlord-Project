using System.Collections.Generic;
using UnityEngine;
using Overlord.LevelGenerator.LevelSOs;

public class RoomController : MonoBehaviour
{
    public DungeonRoomData Data { get; private set; }

    [SerializeField] private Transform spawnPosition;
    [SerializeField] private RoomExitArrow arrowPrefab;

    private bool cleared = false;

    public void Init(DungeonRoomData data)
    {
        Data = data;
    }

    public void OnPlayerEnter()
    {
        if (!cleared)
        {
            EnemyLoader.Instance.LoadEnemies(Data.TotalEnemies, OnRoomCleared);
        }
    }

    private void OnRoomCleared()
    {
        cleared = true;
        ShowExits();
        CollectKeys();
    }

    private void CollectKeys()
    {
        if (Data.Keys == null) return;
        foreach (var key in Data.Keys)
            DungeonRuntimeData.CollectedKeys.Add(Mathf.Abs(key));
    }

    private void ShowExits()
    {
        foreach (var neighbor in LevelLoader.Instance.GetNeighbors(Data))
        {
            var arrow = Instantiate(arrowPrefab, transform);
            arrow.Init(Data, neighbor);
        }
    }

    public Transform GetSpawnPosition() => spawnPosition;
}