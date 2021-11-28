using Game.LevelManager;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Game.NarrativeGenerator.Quests;

namespace LevelGenerator
{
    public static class Interface
    {
        /**
         * Prints the dungeon in the console, saves into a file, and can even save in a csv that is not used anymore
         * We now save it directly into a Unity's Resource Directory
         */
        public static void PrintNumericalGridWithConnections(
            Individual _individual,
            Fitness _fitness, QuestLine _questLine)
        {
            Dungeon dun = _individual.dungeon;

            //List of keys and locked rooms in the level
            List<int> lockedRooms = new List<int>();
            List<int> keys = new List<int>();

            string foldername = "Assets/Resources/Experiment/Dungeons";

            var filename = GetFilename(_individual, _fitness);

            DungeonFileSo dungeonFileSO = ScriptableObject.CreateInstance<DungeonFileSo>();

            //saves where the dungeon grid begins and ends in each direction
            foreach (Room room in dun.rooms)
            {
                if (room.type == RoomType.Key)
                {
                    keys.Add(room.key);
                }
                else if (room.type == RoomType.Locked)
                {
                    lockedRooms.Add(room.key);
                }
            }

            dun.SetBoundariesFromRoomList();

            //The size is normalized to be always positive (easier to handle a matrix)
            dun.SetDimensionsFromBoundaries();

            //Creates a matrix to hold each room and corridor (there may be a corridor between each room, that must be saved
            //hence 2*size
            int[,] map = new int[2 * dun.dimensions.Width, 2 * dun.dimensions.Height];
            //The top of the dungeon's file in unity must contain its dimensions
            dungeonFileSO.dimensions = new Dimensions(2 * dun.dimensions.Width, 2 * dun.dimensions.Height);
            //We initialize the map with the equivalent of an empty cell
            for (int i = 0; i < 2 * dun.dimensions.Width; ++i)
            {
                for (int j = 0; j < 2 * dun.dimensions.Height; ++j)
                {
                    map[i, j] = Common.RoomType.NOTHING;
                }
            }

            InitializeMapFromDungeon(dun, map, keys, lockedRooms);

            InitializeDungeonSoFromMap(dungeonFileSO, dun, map);
            //The assetdatabase stuff only works in the Unity's Editor
            //As is, we can't save a level file in a released build of the game
#if UNITY_EDITOR
            int count = 0;
            string path;

            Directory.CreateDirectory(foldername);

            //Saves the file with the name of its input for the EA and adds a number at the end if a file with the same name exists
            //This prevents the file is overwritten
            path = AssetDatabase.AssetPathToGUID($"{foldername}/{filename}.txt");
            while (path != "")
            {
                count++;
                path = AssetDatabase.AssetPathToGUID($"{foldername}/{filename}-{count}.txt");
            }

            if (count > 0)
                filename += "-" + count;
            filename = foldername + "/" + filename;


            int sameFilenameCounter = 0;

            if (File.Exists(filename + ".asset"))
            {
                do
                {
                    sameFilenameCounter++;
                } while (File.Exists(filename + "-" + sameFilenameCounter + ".asset"));

                filename += "-" + sameFilenameCounter;
            }

            AssetDatabase.CreateAsset(dungeonFileSO, filename + ".asset");
            _questLine.DungeonFileSos.Add(dungeonFileSO);
            AssetDatabase.SaveAssetIfDirty(_questLine);
            Debug.Log("Finished Writing dungeon data");
#endif
        }

        private static void InitializeDungeonSoFromMap(DungeonFileSo dungeonFileSO, Dungeon dun, int[,] map)
        {
            dungeonFileSO.rooms = new List<SORoom>();
            //Now we print it/save to a file/whatever
            for (var i = 0; i < dun.dimensions.Width * 2; ++i)
            {
                for (var j = 0; j < dun.dimensions.Height * 2; ++j)
                {
                    SORoom roomDataInFile;
                    
                    // Calculate the room position in the grid
                    int x = i / 2 + dun.boundaries.MinBoundaries.X;
                    int y = j / 2 + dun.boundaries.MinBoundaries.Y;

                    //If cell is empty, do nothing (or print empty space in console)
                    if (map[i, j] == Common.RoomType.NOTHING)
                    {
                        roomDataInFile = null;
                    }
                    //If there is something (room or corridor) print/save
                    else
                    {
                        var roomType = map[i, j];
                        var roomGrid = dun.grid[x, y];
                        var coordinates = new Coordinates(i + dun.boundaries.MinBoundaries.X * 2, j + dun.boundaries.MinBoundaries.Y * 2);
                        //For Unity's dungeon file we need to save the x and y position of the room
                        roomDataInFile = new SORoom
                        {
                            coordinates = new Coordinates(i, j)
                        };
                        roomDataInFile.Treasures = 0;
                        roomDataInFile.Npcs = 0;
                        ConvertEaDungeonToSoDungeon(coordinates, roomDataInFile, roomGrid, roomType);
                    }

                    if (roomDataInFile != null)
                    {
                        dungeonFileSO.rooms.Add(roomDataInFile);
                    }
                }
            }
        }

        private static void ConvertEaDungeonToSoDungeon(Coordinates coordinates, SORoom roomDataInFile, Room roomGrid,
            int roomType)
        {
            //If room is in (0,0) it is the starting one, we mark it with an "s" and save the "s"
            if (coordinates.X == 0 && coordinates.Y == 0)
            {
                roomDataInFile.type = "s";
                roomDataInFile.TotalEnemies = roomGrid.enemies;
            }
            //If it is a corridor, writes "c" in the file
            else if (roomType == Common.RoomType.CORRIDOR)
            {
                roomDataInFile.type = "c";
            }
            //If is the boss room, writes "B". Currently is where the Triforce is located
            else if (roomType == Common.RoomType.BOSS)
            {
                roomDataInFile.type = "B";
                roomDataInFile.TotalEnemies = roomGrid.enemies;
            }
            //If negative, is a locked corridor, save it as the negative number of the key that opens it
            else if (roomType < 0)
            {
                roomDataInFile.locks = new List<int>
                {
                    roomType
                };
            }
            //If it was a room with treasure, save it as a "T"
            //TODO: change this as now every room may contain treasures, enemies and/or keys
            else if (roomType == Common.RoomType.TREASURE)
            {
                roomDataInFile.treasures = 1;
                roomDataInFile.npcs = 1;
                roomDataInFile.TotalEnemies = roomGrid.enemies;
            }
            //If the room has a positive value, it holds a key.
            //Save the key index so we know what key it is
            else if (roomType > 0)
            {
                roomDataInFile.TotalEnemies = roomGrid.enemies;
                roomDataInFile.keys = new List<int>
                {
                    roomType
                };
            }
            //If the cell was none of the above, it must be an empty room
            else
            {
                roomDataInFile.TotalEnemies = roomGrid.enemies;
            }
        }

        private static void InitializeMapFromDungeon(Dungeon dun, int[,] map, List<int> keys, List<int> lockedRooms)
        {
            //Now we visit each room and save the info on the corresponding cell of the matrix
            for (int i = dun.boundaries.MinBoundaries.X; i < dun.boundaries.MaxBoundaries.X + 1; ++i)
            {
                for (int j = dun.boundaries.MinBoundaries.Y; j < dun.boundaries.MaxBoundaries.Y + 1; ++j)
                {
                    //Converts the coordinate of the original grid (can be negative) to the positive ones used in the matrix
                    int iPositive = i - dun.boundaries.MinBoundaries.X;
                    int jPositive = j - dun.boundaries.MinBoundaries.Y;
                    //Gets the actual room
                    Room actualRoom = dun.grid[i, j];
                    //If there is something in this position in the grid:
                    SetRoomTypeInMap(map, keys, lockedRooms, actualRoom, iPositive, jPositive);
                }
            }
        }

        private static void SetRoomTypeInMap(int[,] map, List<int> keys, List<int> lockedRooms, Room actualRoom, int iPositive, int jPositive)
        {
            if (actualRoom != null)
            {
                switch (actualRoom.type)
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
                        map[iPositive * 2, jPositive * 2] = keys.IndexOf(actualRoom.key) + 1;
                        break;
                    //If the room is locked from its parent, check if it is a boss room by checking if the key to open is the last one created
                    //It guarantees at least that is the deepest key in the tree, but not the longest route
                    //TODO: Must also change to allow the generation of treasures and enemies
                    case RoomType.Locked when lockedRooms.IndexOf(actualRoom.key) == lockedRooms.Count - 1:
                        map[iPositive * 2, jPositive * 2] = Common.RoomType.BOSS;
                        break;
                    case RoomType.Locked:
                        map[iPositive * 2, jPositive * 2] = Common.RoomType.TREASURE;
                        break;
                    //If it is not a room, something is wrong
                    default:
                        Console.WriteLine("Something went wrong printing the tree!\n");
                        Console.WriteLine("This Room type does not exist!\n\n");
                        break;
                }

                //As (for now) every room must be connected to its parent or children
                //We need only to check its parent to create the corridors
                Room parent = actualRoom.parent;
                if (parent == null) return;
                int x = parent.X - actualRoom.X + 2 * iPositive;
                int y = parent.Y - actualRoom.Y + 2 * jPositive;
                //If corridor is lockes, save the index of the key that opens it
                //But as a negative value. A negative corridor is locked!
                //If not, save it only as a normal corridor
                if (actualRoom.type == RoomType.Locked)
                {
                    map[x, y] = -(keys.IndexOf(actualRoom.key) + 1);
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
                map[iPositive * 2, jPositive * 2] = Common.RoomType.TREASURE;
            }
            else
            {
                map[iPositive * 2, jPositive * 2] = Common.RoomType.EMPTY;
            }
        }

        private static string GetFilename(Individual _individual, Fitness _fitness)
        {
            // Get the coordinate values corresponding to the Elite
            float ce = _individual.exploration;
            float le = _individual.leniency;
            int e = SearchSpace.GetCoefficientOfExplorationIndex(ce);
            int l = SearchSpace.GetLeniencyIndex(le);
            (float, float)[] listCE = SearchSpace.CoefficientOfExplorationRanges();
            (float, float)[] listLE = SearchSpace.LeniencyRanges();
            string strCE = ("" + listCE[e])
                .Replace(" ", "").Replace("(", "")
                .Replace(")", "").Replace(",", "~");
            string strLE = ("" + listLE[l])
                .Replace(" ", "").Replace("(", "")
                .Replace(")", "").Replace(",", "~");
            // Set the dungeon filename
            string filename = "";
            filename = "R" + _fitness.DesiredRooms +
                       "-K" + _fitness.DesiredKeys +
                       "-L" + _fitness.DesiredLocks +
                       "-E" + _fitness.DesiredEnemies +
                       "-L" + _fitness.DesiredLinearity +
                       "-CE" + strCE +
                       "-LE" + strLE;
            return filename;
        }
    }
}
