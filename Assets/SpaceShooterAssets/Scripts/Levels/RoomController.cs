using Overlord.LevelGenerator.LevelSOs;
using Overlord.NarrativeGenerator.NPCs;
using Overlord.NarrativeGenerator.Quests;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class RoomController : MonoBehaviour
{
    public DungeonRoomData Data { get; private set; }

    [SerializeField] private Transform spawnPosition;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject lockedArrowPrefab;
    private NpcSo currentNpc;
    private QuestLine currentQuestLine;
    private GameObject npcPrefab;

    private bool cleared = false;

    public void Init(QuestLineList questlineList, DungeonRoomData data)
    {
        Data = data;
        if (data.NumOfNpcs > 0)
            ConfigureNpc(questlineList, data);
    }

    private void ConfigureNpc(QuestLineList questlineList, DungeonRoomData data)
    {
        foreach (NpcSo npc in questlineList.NpcSos)
        {
            if (npc.RoomCoordinates != null &&
                npc.RoomCoordinates.X == data.Coordinates.X &&
                npc.RoomCoordinates.Y == data.Coordinates.Y)
            {
                currentNpc = npc;
                npcPrefab = Instantiate(currentNpc.Prefab, transform);
                //npcGO.GetComponent<NPCController>().Init(currentNpc, null);
                return;
            }
        }
        foreach (NpcSo npc in questlineList.NpcSos)
        {
            if (npc.RoomCoordinates != null &&
                npc.RoomCoordinates.X == -1 && 
                npc.RoomCoordinates.Y == -1)
            {
                Coordinates npcCoordinate = new Coordinates(data.Coordinates.X, data.Coordinates.Y);
                npc.RoomCoordinates = npcCoordinate;
                currentNpc = npc;
                npcPrefab = Instantiate(currentNpc.Prefab, transform);
                return;
            }
        }
    }

    public void OnPlayerEnter()
    {
        if (!cleared)
        {
            if (Data.NumOfNpcs <= 0)
                EnemyLoader.Instance.LoadEnemies(Data.TotalEnemies, OnRoomCleared);
            else
            {
                OnRoomCleared();

            }
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

    private void OnExitRoom()
    {
        if (npcPrefab != null)
            Destroy(npcPrefab);
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
            arrowGO.GetComponent<RoomExitArrow>().Init(Data, corridor, targetRoom, OnExitRoom);
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