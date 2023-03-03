using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Dialogues;
using Game.Events;
using Game.ExperimentControllers;
using Game.GameManager;
using Game.LevelGenerator.LevelSOs;
using Game.LevelManager.DungeonManager;
using Game.NarrativeGenerator.Quests;
using UnityEngine;
using Util;

namespace Game.LevelManager.DungeonLoader
{
    public class DungeonLoader : MonoBehaviour
    {

        protected static Map _dungeonMap;
        public List<RoomBhv> roomPrefabs;
        public Dictionary<Coordinates, RoomBhv> RoomBhvMap; //2D array for easy room indexing
        public RoomBhv roomBehavior;

        public int TotalTreasures { get; private set; }
        public static event StartMapEvent StartMapEventHandler;
        [field: SerializeField] public GeneratorSettings CurrentGeneratorSettings { get; set; }
        private Enums.RoomThemeEnum _selectedTheme;

        private void OnEnable()
        {
            TaggedDialogueHandler.MarkRoomOnMiniMapEventHandler += FindRoomAndMarkToVisit;
        }

        private void FindRoomAndMarkToVisit(object sender, MarkRoomOnMinimapEventArgs e)
        {
            RoomBhvMap[e.RoomCoordinates].MarkToVisit();
        }

        private void OnDisable()
        {
            TaggedDialogueHandler.MarkRoomOnMiniMapEventHandler -= FindRoomAndMarkToVisit;
        }

        public void LoadNewLevel(DungeonFileSo dungeonFileSo, QuestLineList currentQuestLineList)
        {
            Debug.Log("Loading new Level");
            LoadDungeon(dungeonFileSo);
            
            EnemyLoader.DistributeEnemiesInDungeon(_dungeonMap, currentQuestLineList);
            var itemsToDistribute = currentQuestLineList.ItemParametersForQuestLines.ItemsByType;
            var totalItems = currentQuestLineList.ItemParametersForQuestLines.TotalItems;
            ItemDispenser.DistributeItemsInDungeon(_dungeonMap, itemsToDistribute, totalItems);
            TotalTreasures = currentQuestLineList.ItemParametersForQuestLines.TotalItems;
            NpcDispenser.DistributeNpcsInDungeon(_dungeonMap, currentQuestLineList.NpcSos);

            RoomBhvMap = new Dictionary<Coordinates, RoomBhv>();

            _selectedTheme = (Enums.RoomThemeEnum) RandomSingleton.GetInstance().Random.Next((int) Enums.RoomThemeEnum.Count);
            InstantiateRooms();
            ConnectRoooms();
        }
        
        public IEnumerator OnStartMap(string mapName)
        {
            yield return null;
            StartMapEventHandler?.Invoke(null, new StartMapEventArgs(mapName, _dungeonMap, TotalTreasures));
            RoomBhvMap[_dungeonMap.StartRoomCoordinates].OnRoomEnter();
        }
        
        private void SetDestinations(Coordinates targetCoordinates, Coordinates sourceCoordinates, int orientation)
        {
            switch (orientation)
            {
                case 1:
                    RoomBhvMap[sourceCoordinates].doorWest.SetDestination(RoomBhvMap[targetCoordinates].doorEast);
                    RoomBhvMap[targetCoordinates].doorEast.SetDestination(RoomBhvMap[sourceCoordinates].doorWest);
                    break;
                case 2:
                    RoomBhvMap[sourceCoordinates].doorNorth.SetDestination(RoomBhvMap[targetCoordinates].doorSouth);
                    RoomBhvMap[targetCoordinates].doorSouth.SetDestination(RoomBhvMap[sourceCoordinates].doorNorth);
                    break;
            }
        }
        
        public List<int> CheckCorridor(Coordinates targetCoordinates)
        {
            if (!_dungeonMap.DungeonPartByCoordinates.ContainsKey(targetCoordinates)) return null;
            //Sets door
            var lockedCorridor = _dungeonMap.DungeonPartByCoordinates[targetCoordinates] as DungeonLockedCorridor;

            return lockedCorridor != null ? lockedCorridor.LockIDs : new List<int>();
        }
        
        protected virtual void LoadDungeon(DungeonFileSo dungeonFileSo)
        {
            _dungeonMap = new Map(dungeonFileSo, CurrentGeneratorSettings.CreateRooms, CurrentGeneratorSettings.RoomSize, CurrentGeneratorSettings.GameType);
        }

        private void InstantiateRooms()
        {
            foreach (var currentPart in _dungeonMap.DungeonPartByCoordinates.Values.OfType<DungeonRoom>())
            {

                var newRoom = RoomLoader.InstantiateRoom(currentPart, roomBehavior, CurrentGeneratorSettings.GameType);
                CheckConnections(currentPart, newRoom, _dungeonMap.Dimensions);
                newRoom.SetTheme(_selectedTheme);
                RoomBhvMap.Add(currentPart.Coordinates, newRoom); 

            }
        }

        private void CheckConnections(DungeonRoom dungeonRoom, RoomBhv newRoom, Dimensions dimensions)
        {
            Coordinates targetCoordinates;
            if (IsLeftEdge(dungeonRoom.Coordinates))
            { // west
                targetCoordinates = new Coordinates(dungeonRoom.Coordinates.X - 1, dungeonRoom.Coordinates.Y);
                newRoom.westDoor = CheckCorridor(targetCoordinates);
            }
            if (IsBottomEdge(dungeonRoom.Coordinates))
            { // north
                targetCoordinates = new Coordinates(dungeonRoom.Coordinates.X, dungeonRoom.Coordinates.Y - 1);
                newRoom.northDoor = CheckCorridor(targetCoordinates);
            }
            if (IsRightEdge(dungeonRoom.Coordinates, dimensions.Width))
            {
                targetCoordinates = new Coordinates(dungeonRoom.Coordinates.X + 1, dungeonRoom.Coordinates.Y);
                newRoom.eastDoor = CheckCorridor(targetCoordinates);
            }
            if (IsTopEdge(dungeonRoom.Coordinates, dimensions.Height))
            {
                targetCoordinates = new Coordinates(dungeonRoom.Coordinates.X, dungeonRoom.Coordinates.Y + 1);
                newRoom.southDoor = CheckCorridor(targetCoordinates);
            }
        }
        
        private bool IsLeftEdge(Coordinates coordinates)
        {
            return coordinates.X > 1;
        }

        private bool IsRightEdge(Coordinates coordinates, int width)
        {
            return coordinates.X < (width - 1);
        }

        private bool IsBottomEdge(Coordinates coordinates)
        {
            return coordinates.Y > 1;
        }
        private bool IsTopEdge(Coordinates coordinates, int height)
        {
            return coordinates.Y < (height - 1);
        }
        
        public void ConnectRoooms()
        {
            foreach (RoomBhv currentRoom in RoomBhvMap.Values)
            {
                CreateConnectionsBetweenRooms(currentRoom);
            }
        }
        
        public void CreateConnectionsBetweenRooms(RoomBhv currentRoom)
        {
            Coordinates targetCoordinates;
            if (currentRoom.westDoor != null)
            { // west
                targetCoordinates = new Coordinates(currentRoom.roomData.Coordinates.X - 2, currentRoom.roomData.Coordinates.Y);
                SetDestinations(targetCoordinates, currentRoom.roomData.Coordinates, 1);
            }
            if (currentRoom.northDoor != null)
            { // west
                targetCoordinates = new Coordinates(currentRoom.roomData.Coordinates.X, currentRoom.roomData.Coordinates.Y - 2);
                SetDestinations(targetCoordinates, currentRoom.roomData.Coordinates, 2);
            }
        }
    }
}