using System.Collections.Generic;
using UnityEngine;
using Overlord.LevelGenerator.LevelSOs;

public class RoomController : MonoBehaviour
{
    public DungeonRoomData Data { get; private set; }

    [SerializeField] private Transform spawnPosition;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject lockedArrowPrefab;

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
            MinimapController.Instance.RevealRoom(Data);
            MinimapController.Instance.SetCurrentRoom(Data);
        }
    }

    private void OnRoomCleared()
    {
        Debug.Log("Room cleared!");
        cleared = true;
        ShowExits();
        CollectKeys();
    }

    private void ShowExits()
    {
        ShowExitArrows("Corridor", arrowPrefab);
        ShowExitArrows("LockedCorridor", lockedArrowPrefab);
    }

    private void ShowExitArrows(string corridorType, GameObject prefab)
    {
        foreach (var corridor in LevelLoader.Instance.GetNeighbors(corridorType, Data))
        {
            var targetRoom = LevelLoader.Instance.GetRoomBeyondCorridor(Data, corridor);
            if (targetRoom == null) continue;

            var arrowGO = Instantiate(prefab, transform);
            arrowGO.GetComponent<RoomExitArrow>().Init(Data, corridor, targetRoom);
            MinimapController.Instance.RevealRoom(corridor);
        }
    }

    private void CollectKeys()
    {
        if (Data.Keys == null) return;
        foreach (var key in Data.Keys)
            DungeonRuntimeData.CollectedKeys.Add(Mathf.Abs(key));
    }


    public Transform GetSpawnPosition() => spawnPosition;
}