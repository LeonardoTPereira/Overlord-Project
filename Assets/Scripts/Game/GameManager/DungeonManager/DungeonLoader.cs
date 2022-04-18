using System;
using System.Collections.Generic;
using System.Linq;
using Game.Events;
using Game.LevelGenerator.LevelSOs;
using Game.LevelManager;
using Game.NarrativeGenerator.Quests;
using UnityEngine;
using Util;

namespace Game.GameManager.DungeonManager
{
    public static class DungeonLoader
    {
        private static Map _dungeonMap;
        public static List<RoomBhv> roomPrefabs;
        public static Dictionary<Coordinates, RoomBhv> roomBHVMap; //2D array for easy room indexing
        public static event StartMapEvent StartMapEventHandler;
        public static void LoadNewLevel(DungeonFileSo dungeonFileSo, QuestLine currentQuestLine, Dimensions dimensions)
        {
            LoadDungeon(dungeonFileSo);
            
            EnemyLoader.DistributeEnemiesInDungeon(_dungeonMap, currentQuestLine);
            ItemDispenser.DistributeItemsInDungeon(_dungeonMap, currentQuestLine);
            NpcDispenser.DistributeNpcsInDungeon(_dungeonMap, currentQuestLine);

            OnStartMap(dungeonFileSo.name, _dungeonMap);
            roomBHVMap = new Dictionary<Coordinates, RoomBhv>();

            var selectedRoom = roomPrefabs[RandomSingleton.GetInstance().Random.Next(roomPrefabs.Count)];
            InstantiateRooms(selectedRoom, dimensions);
            ConnectRoooms();
        }
        
        private static void OnStartMap(string mapName, Map map)
        {
            roomBHVMap[map.StartRoomCoordinates].OnRoomEnter();
            StartMapEventHandler?.Invoke(null, new StartMapEventArgs(mapName, map));
        }
        
        private static void SetDestinations(Coordinates targetCoordinates, Coordinates sourceCoordinates, int orientation)
        {
            switch (orientation)
            {
                case 1:
                    roomBHVMap[sourceCoordinates].doorWest.SetDestination(roomBHVMap[targetCoordinates].doorEast);
                    roomBHVMap[targetCoordinates].doorEast.SetDestination(roomBHVMap[sourceCoordinates].doorWest);
                    break;
                case 2:
                    roomBHVMap[sourceCoordinates].doorNorth.SetDestination(roomBHVMap[targetCoordinates].doorSouth);
                    roomBHVMap[targetCoordinates].doorSouth.SetDestination(roomBHVMap[sourceCoordinates].doorNorth);
                    break;
            }
        }
        
        public static List<int> CheckCorridor(Coordinates targetCoordinates)
        {
            if (!_dungeonMap.DungeonPartByCoordinates.ContainsKey(targetCoordinates)) return null;
            //Sets door
            var lockedCorridor = _dungeonMap.DungeonPartByCoordinates[targetCoordinates] as DungeonLockedCorridor;

            return lockedCorridor != null ? lockedCorridor.LockIDs : new List<int>();
        }
        
        private static void LoadDungeon(DungeonFileSo dungeonFileSo)
        {
            _dungeonMap = new Map(dungeonFileSo, null);
        }

        private static void InstantiateRooms(RoomBhv roomBhv, Dimensions dimensions)
        {
            foreach (var currentPart in _dungeonMap.DungeonPartByCoordinates.Values.OfType<DungeonRoom>())
            {
                var newRoom = RoomLoader.InstantiateRoom(currentPart, roomBhv);
                CheckConnections(currentPart, newRoom, dimensions);
                roomBHVMap.Add(currentPart.Coordinates, newRoom); 
            }
        }

        private static void CheckConnections(DungeonRoom dungeonRoom, RoomBhv newRoom, Dimensions dimensions)
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
        
        private static bool IsLeftEdge(Coordinates coordinates)
        {
            return coordinates.X > 1;
        }

        private static bool IsRightEdge(Coordinates coordinates, int width)
        {
            return coordinates.X < (width - 1);
        }

        private static bool IsBottomEdge(Coordinates coordinates)
        {
            return coordinates.Y > 1;
        }
        private static bool IsTopEdge(Coordinates coordinates, int height)
        {
            return coordinates.Y < (height - 1);
        }
        
        public static void ConnectRoooms()
        {
            foreach (RoomBhv currentRoom in roomBHVMap.Values)
            {
                CreateConnectionsBetweenRooms(currentRoom);
            }
        }
        
        public static void CreateConnectionsBetweenRooms(RoomBhv currentRoom)
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