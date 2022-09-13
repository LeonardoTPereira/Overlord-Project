using System;
using System.Collections.Generic;
using Game.LevelGenerator.EvolutionaryAlgorithm;
using Game.LevelGenerator.LevelSOs;
using Game.LevelManager;
using UnityEngine;
using Util;

namespace Game.LevelGenerator
{
    public static class Interface
    {
        /**
         * Prints the dungeon in the console, saves into a file, and can even save in a csv that is not used anymore
         * We now save it directly into a Unity's Resource Directory
         */
        public static DungeonFileSo CreateDungeonSoFromIndividual(Individual individual, int totalEnemies = 0, int totalTreasures = 0, int totalNpcs = 0)
        {
            var dun = individual.dungeon;

            //List of keys and locked rooms in the level
            var lockedRooms = new List<int>();
            var keys = new List<int>();

            var dungeonFileSo = ScriptableObject.CreateInstance<DungeonFileSo>();
            dungeonFileSo.BiomeName = individual.BiomeName;
            dungeonFileSo.TotalEnemies = totalEnemies;
            dungeonFileSo.TotalTreasures = totalTreasures;
            dungeonFileSo.TotalNpcs = totalNpcs;
            //saves where the dungeon grid begins and ends in each direction
            foreach (var room in dun.Rooms)
            {
                switch (room.Type1)
                {
                    case RoomType.Key:
                        keys.Add(room.Key);
                        break;
                    case RoomType.Locked:
                        lockedRooms.Add(room.Key);
                        break;
                    case RoomType.Normal:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            dun.SetBoundariesFromRoomList();

            //The size is normalized to be always positive (easier to handle a matrix)
            dun.SetDimensionsFromBoundaries();

            //Creates a matrix to hold each room and corridor (there may be a corridor between each room, that must be saved
            //hence 2*size
            int[,] map = new int[2 * dun.DungeonDimensions.Width, 2 * dun.DungeonDimensions.Height];
            //The top of the dungeon's file in unity must contain its dimensions
            dungeonFileSo.DungeonSizes = new Dimensions(2 * dun.DungeonDimensions.Width, 2 * dun.DungeonDimensions.Height);
            dungeonFileSo.FitnessFromEa = individual.Fitness;
            dungeonFileSo.ExplorationCoefficient = individual.exploration;
            dungeonFileSo.LeniencyCoefficient = individual.leniency;
            //We initialize the map with the equivalent of an empty cell
            for (int i = 0; i < 2 * dun.DungeonDimensions.Width; ++i)
            {
                for (int j = 0; j < 2 * dun.DungeonDimensions.Height; ++j)
                {
                    map[i, j] = Common.RoomType.NOTHING;
                }
            }

            InitializeMapFromDungeon(dun, map, keys, lockedRooms);

            InitializeDungeonSoFromMap(dungeonFileSo, dun, map);
            return dungeonFileSo;
        }

        private static void InitializeDungeonSoFromMap(DungeonFileSo dungeonFileSo, Dungeon dun, int[,] map)
        {
            dungeonFileSo.Rooms = new List<DungeonRoomData>();
            //Now we print it/save to a file/whatever
            for (var i = 0; i < dun.DungeonDimensions.Width * 2; ++i)
            {
                for (var j = 0; j < dun.DungeonDimensions.Height * 2; ++j)
                {
                    DungeonRoomData roomDataDataInFile;
                    
                    // Calculate the room position in the grid
                    var x = i / 2 + dun.DungeonBoundaries.MinBoundaries.X;
                    var y = j / 2 + dun.DungeonBoundaries.MinBoundaries.Y;

                    //If cell is empty, do nothing (or print empty space in console)
                    if (map[i, j] == Common.RoomType.NOTHING)
                    {
                        roomDataDataInFile = null;
                    }
                    //If there is something (room or corridor) print/save
                    else
                    {
                        var roomType = map[i, j];
                        var roomGrid = dun.DungeonGrid[x, y];
                        var coordinates = new Coordinates(i + dun.DungeonBoundaries.MinBoundaries.X * 2, j + dun.DungeonBoundaries.MinBoundaries.Y * 2);
                        //For Unity's dungeon file we need to save the x and y position of the room
                        roomDataDataInFile = new DungeonRoomData(i, j);
                        ConvertEaDungeonToSoDungeon(coordinates, roomDataDataInFile, roomGrid, roomType);
                    }

                    if (roomDataDataInFile != null)
                    {
                        dungeonFileSo.Rooms.Add(roomDataDataInFile);
                    }
                }
            }
        }

        private static void ConvertEaDungeonToSoDungeon(Coordinates coordinates, DungeonRoomData roomDataDataInFile,
            Room roomGrid, int roomType)
        {
            //If room is in (0,0) it is the starting one, we mark it with an "s" and save the "s"
            if (coordinates.X == 0 && coordinates.Y == 0)
            {
                roomDataDataInFile.Type = Constants.RoomTypeString.Start;
                roomDataDataInFile.TotalEnemies = roomGrid.Enemies;
            }
            //If it is a corridor, writes "c" in the file
            else if (roomType == Common.RoomType.CORRIDOR)
            {
                roomDataDataInFile.Type = Constants.RoomTypeString.Corridor;
            }
            //If is the boss room, writes "B". Currently is where the Triforce is located
            else if (roomType == Common.RoomType.BOSS)
            {
                roomDataDataInFile.Type = Constants.RoomTypeString.Boss;
                roomDataDataInFile.TotalEnemies = roomGrid.Enemies;
            }
            //If negative, is a locked corridor, save it as the negative number of the key that opens it
            else if (roomType < 0)
            {
                roomDataDataInFile.Type = Constants.RoomTypeString.Lock;
                roomDataDataInFile.Locks = new List<int>
                {
                    roomType
                };
            }
            else if (roomType == Common.RoomType.LEAF)
            {
                roomDataDataInFile.Type = Constants.RoomTypeString.Leaf;
                roomDataDataInFile.Treasures = 1;
                roomDataDataInFile.Npcs = 1;
                roomDataDataInFile.TotalEnemies = roomGrid.Enemies;
                roomDataDataInFile.IsLeaf = roomGrid.IsLeafNode();
            }
            else if (roomType == Common.RoomType.LOCKED)
            {
                roomDataDataInFile.Type = Constants.RoomTypeString.Lock;
                roomDataDataInFile.Treasures = 1;
                roomDataDataInFile.Npcs = 1;
                roomDataDataInFile.TotalEnemies = roomGrid.Enemies;
                roomDataDataInFile.IsLeaf = roomGrid.IsLeafNode();
            }
            //If the room has a positive value, it holds a key.
            //Save the key index so we know what key it is
            else if (roomType > 0)
            {
                roomDataDataInFile.TotalEnemies = roomGrid.Enemies;
                roomDataDataInFile.Type = Constants.RoomTypeString.Key;
                roomDataDataInFile.Keys = new List<int>
                {
                    roomType
                };
                roomDataDataInFile.IsLeaf = roomGrid.IsLeafNode();
            }
            //If the cell was none of the above, it must be an empty room
            else
            {
                roomDataDataInFile.Type = Constants.RoomTypeString.Normal;
                roomDataDataInFile.TotalEnemies = roomGrid.Enemies;
                roomDataDataInFile.IsLeaf = roomGrid.IsLeafNode();
            }
        }

        private static void InitializeMapFromDungeon(Dungeon dun, int[,] map, List<int> keys, List<int> lockedRooms)
        {
            //Now we visit each room and save the info on the corresponding cell of the matrix
            for (var i = dun.DungeonBoundaries.MinBoundaries.X; i < dun.DungeonBoundaries.MaxBoundaries.X + 1; ++i)
            {
                for (var j = dun.DungeonBoundaries.MinBoundaries.Y; j < dun.DungeonBoundaries.MaxBoundaries.Y + 1; ++j)
                {
                    //Converts the coordinate of the original grid (can be negative) to the positive ones used in the matrix
                    var iPositive = i - dun.DungeonBoundaries.MinBoundaries.X;
                    var jPositive = j - dun.DungeonBoundaries.MinBoundaries.Y;
                    //Gets the actual room
                    var actualRoom = dun.DungeonGrid[i, j];
                    //If there is something in this position in the grid:
                    SetRoomTypeInMap(map, keys, lockedRooms, actualRoom, iPositive, jPositive);
                }
            }
        }

        private static void SetRoomTypeInMap(int[,] map, List<int> keys, List<int> lockedRooms, Room actualRoom, int iPositive, int jPositive)
        {
            if (actualRoom != null)
            {
                switch (actualRoom.Type1)
                {
                    //If it is a normal room, check if is a leaf node. We are currently placing treasures there
                    //If not a leaf, just save as an empty room for now
                    //TODO: change to handle the new format of having the room's Key ID followed by amount of treasure and them enemy difficulty
                    //Will have to change to an array or something, with 0 treasures and 0 difficulty meaning no treasure and no enemy inside
                    case RoomType.Normal:
                        SetNormalRoomData(map, actualRoom, iPositive, jPositive);
                        break;
                    //If the room has a key, saves the corresponding key index in the matrix
                    //TODO: Must also change to allow the generation of treasures and enemies
                    case RoomType.Key:
                        map[iPositive * 2, jPositive * 2] = keys.IndexOf(actualRoom.Key) + 1;
                        break;
                    //If the room is locked from its parent, check if it is a boss room by checking if the key to open is the last one created
                    //It guarantees at least that is the deepest key in the tree, but not the longest route
                    //TODO: Must also change to allow the generation of treasures and enemies
                    case RoomType.Locked when lockedRooms.IndexOf(actualRoom.Key) == lockedRooms.Count - 1:
                        map[iPositive * 2, jPositive * 2] = Common.RoomType.BOSS;
                        break;
                    case RoomType.Locked:
                        map[iPositive * 2, jPositive * 2] = Common.RoomType.LOCKED;
                        break;
                    //If it is not a room, something is wrong
                    default:
                        Console.WriteLine("Something went wrong printing the tree!\n");
                        Console.WriteLine("This Room type does not exist!\n\n");
                        break;
                }

                //As (for now) every room must be connected to its parent or children
                //We need only to check its parent to create the corridors
                var parent = actualRoom.Parent;
                if (parent == null) return;
                var x = parent.X - actualRoom.X + 2 * iPositive;
                var y = parent.Y - actualRoom.Y + 2 * jPositive;
                //If corridor is lockes, save the index of the key that opens it
                //But as a negative value. A negative corridor is locked!
                //If not, save it only as a normal corridor
                if (actualRoom.Type1 == RoomType.Locked)
                {
                    map[x, y] = -(keys.IndexOf(actualRoom.Key) + 1);
                }
                else
                {
                    map[x, y] = Common.RoomType.CORRIDOR;
                }
            }
        }

        private static void SetNormalRoomData(int[,] map, Room actualRoom, int iPositive, int jPositive)
        {
            if (actualRoom.IsLeafNode())
            {
                map[iPositive * 2, jPositive * 2] = Common.RoomType.LEAF;
            }
            else
            {
                map[iPositive * 2, jPositive * 2] = Common.RoomType.EMPTY;
            }
        }


    }
}
