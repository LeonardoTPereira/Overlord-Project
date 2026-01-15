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
    private GameObject npcPrefab;
    private RoomProgressData _roomProgressData;

    private bool cleared = false;

    public void Init(QuestLineList questlineList, DungeonRoomData data)
    {
        Data = data;
        if (data.NumOfNpcs > 0)
            ConfigureNpc(questlineList, data);
    }

    public void OnKilledEnemy()
    {
        if (_roomProgressData == null) return;
        _roomProgressData.RemainingEnemies--;
    }

    public void OnCollectedTreasure()
    {
        if (_roomProgressData == null) return;
        _roomProgressData.RemainingCollectibles--;
    }

    public void OnToolCollected()
    {         
        if (_roomProgressData == null) return;
        _roomProgressData.RemainingTools--;
    }

    public void OnLoreItemRead()
    {
        if (_roomProgressData == null) return;
        _roomProgressData.RemainingLoreItems--;
    }

    public void OnPlayerEnter()
    {
        if (!cleared)
        {
            if (Data.NumOfNpcs <= 0)
            {
                _roomProgressData = DungeonRoomStateManager.EnterRoom(
                    Data.Coordinates,
                    Data.TotalEnemies,
                    Data.Treasures,
                    Random.Range(0, Data.Treasures/1),
                    Random.Range(0, Data.Treasures / 2));

                EnemyLoader.Instance.LoadEnemies(_roomProgressData.RemainingEnemies, OnRoomCleared);
                PointsOrCollectiblesLoader.Instance.LoadCollectibles(_roomProgressData.RemainingCollectibles);
            }
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
        DungeonRoomStateManager.UpdateRoomState(Data.Coordinates, _roomProgressData);
        Debug.Log("Room cleared!");
        cleared = true;
        ShowExits();
        CollectKeys();
    }

    private void OnExitRoom()
    {
        if (npcPrefab != null)
        {
            DialogueManager.Instance.EndDialogue();
            Destroy(npcPrefab);
        }
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
            DungeonRoomStateManager.CollectedKeys.Add(Mathf.Abs(key));
    }

    private void ConfigureNpc(QuestLineList questlineList, DungeonRoomData data)
    {
        Coordinates roomCoords = data.Coordinates;

        NpcSo npc = questlineList.NpcSos.Find(npc =>
            HasValidCoordinates(npc) &&
            npc.RoomCoordinates.X == roomCoords.X &&
            npc.RoomCoordinates.Y == roomCoords.Y);

        if (npc == null)
        {
            npc = questlineList.NpcSos.Find(npc =>
                HasValidCoordinates(npc) &&
                npc.RoomCoordinates.X == -1 &&
                npc.RoomCoordinates.Y == -1);

            if (npc == null)
                return;

            npc.RoomCoordinates = new Coordinates(roomCoords.X, roomCoords.Y);
        }

        SpawnNpc(npc);
        ConfigureQuestDialogue(questlineList, npc);
    }

    private static bool HasValidCoordinates(NpcSo npc)
    {
        return npc.RoomCoordinates != null;
    }

    private void SpawnNpc(NpcSo npc)
    {
        currentNpc = npc;
        npcPrefab = Instantiate(currentNpc.Prefab, transform);        
    }

    private void ConfigureQuestDialogue(QuestLineList questlineList, NpcSo npc)
    {
        QuestLine questLine = questlineList.QuestLines.Find(q =>
            q.NpcInCharge != null &&
            q.NpcInCharge.NpcName == npc.NpcName);

        if (questLine == null)
            return;

        string[] questEndSentence =
            QuestLoader.Instance.GetQuestSentence(
                questLine.GetCurrentQuest().SymbolType);

        npcPrefab
            .GetComponent<DialogueTrigger>()
            .SetEndDialogue(questEndSentence);
    }

    public Transform GetSpawnPosition() => spawnPosition;
}