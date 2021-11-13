using Game.LevelManager;
using Game.EnemyGenerator;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using Game.NarrativeGenerator.Quests;

namespace LevelGenerator
{
    class Interface
    {
        /**
         * Prints the dungeon in the console, saves into a file, and can even save in a csv that is not used anymore
         * We now save it directly into a Unity's Resource Directory
         */
        public static void PrintNumericalGridWithConnections(
            Individual _individual,
            Fitness _fitness,
            TreasureRuntimeSetSO _treasureRuntimeSetSO
        ) {
            Dungeon dun = _individual.dungeon;
            int remainingItems, remainingNpcs;
            if(dun.DungeonQuestLine != null)
            {
                remainingItems = dun.DungeonQuestLine.ItemParametersForQuestLine.TotalItems;
                remainingNpcs = dun.DungeonQuestLine.NpcParametersForQuestLine.totalNpcs;
            }
            else
            {
                remainingItems = 0;
                remainingNpcs = 0;
            }

            //List of keys and locked rooms in the level
            List<int> lockedRooms = new List<int>();
            List<int> keys = new List<int>();

            string foldername = "Assets/Resources/Experiment/Dungeons";

            // Get the coordinate values corresponding to the Elite
            float ce = _individual.exploration;
            float le = _individual.leniency;
            int e = SearchSpace.GetCoefficientOfExplorationIndex(ce);
            int l = SearchSpace.GetLeniencyIndex(le);
            (float, float)[] listCE = SearchSpace.
                CoefficientOfExplorationRanges();
            (float, float)[] listLE = SearchSpace.
                LeniencyRanges();
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
            SORoom roomDataInFile = null;
            //We initialize the map with the equivalent of an empty cell
            for (int i = 0; i < 2 * dun.dimensions.Width; ++i)
            {
                for (int j = 0; j < 2 * dun.dimensions.Height; ++j)
                {
                    map[i, j] = Common.RoomType.NOTHING;
                }
            }

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
                    if (actualRoom != null)
                    {
                        //If it is a normal room, check if is a leaf node. We are currently placing treasures there
                        //If not a leaf, just save as an empty room for now
                        //TODO: change to handle the new format of having the room's Key ID followed by amount of treasure and them enemy difficulty
                        //Will have to change to an array or something, with 0 treasures and 0 difficulty meaning no treasure and no enemy inside
                        if (actualRoom.type == RoomType.Normal)
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
                        //If the room has a key, saves the corresponding key index in the matrix
                        //TODO: Must also change to allow the generation of treasures and enemies
                        else if (actualRoom.type == RoomType.Key)
                        {
                            map[iPositive * 2, jPositive * 2] = keys.IndexOf(actualRoom.key) + 1;
                        }
                        //If the room is locked from its parent, check if it is a boss room by checking if the key to open is the last one created
                        //It guarantees at least that is the deepest key in the tree, but not the longest route
                        //TODO: Must also change to allow the generation of treasures and enemies
                        else if (actualRoom.type == RoomType.Locked)
                        {
                            if (lockedRooms.IndexOf(actualRoom.key) == lockedRooms.Count - 1)
                            {
                                map[iPositive * 2, jPositive * 2] = Common.RoomType.BOSS;
                            }
                            else
                            {
                                map[iPositive * 2, jPositive * 2] = Common.RoomType.TREASURE;
                            }
                        }
                        //If it is not a room, something is wrong
                        else
                        {
                            Console.WriteLine("Something went wrong printing the tree!\n");
                            Console.WriteLine("This Room type does not exist!\n\n");
                        }
                        //As (for now) every room must be connected to its parent or children
                        //We need only to check its parent to create the corridors
                        Room parent = actualRoom.parent;
                        if (parent != null)
                        {
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
                }
            }
            dungeonFileSO.rooms = new List<SORoom>();
            //Now we print it/save to a file/whatever
            for (int i = 0; i < dun.dimensions.Width * 2; ++i)
            {
                for (int j = 0; j < dun.dimensions.Height * 2; ++j)
                {
                    //This whole block was to print in a console
                    if (map[i, j] == Common.RoomType.EMPTY)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                    }
                    else if (map[i, j] == Common.RoomType.CORRIDOR)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    }
                    else if (map[i, j] == 7)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else if (map[i, j] == Common.RoomType.NOTHING)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (map[i, j] == Common.RoomType.BOSS)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else if (map[i, j] > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }
                    else if (map[i, j] < 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    }

                    // Calculate the room position in the grid
                    int x = i / 2 + dun.boundaries.MinBoundaries.X;
                    int y = j / 2 + dun.boundaries.MinBoundaries.Y;

                    //If cell is empty, do nothing (or print empty space in console)
                    if (map[i, j] == Common.RoomType.NOTHING)
                    {
                        Console.Write("  ");
                        roomDataInFile = null;
                    }
                    //If there is something (room or corridor) print/save
                    else
                    {
                        System.Random random = new System.Random();

                        int enemyType_Randomizer = UnityEngine.Random.Range(0, 3);

                        //For Unity's dungeon file we need to save the x and y position of the room
                        roomDataInFile = new SORoom
                        {
                            coordinates = new Coordinates(i, j)
                        };
                        //this writerRG was used in the CSV pre-Unity, ignore it as legacy
                        //writerRG.WriteLine(i);
                        //writerRG.WriteLine(j);
                        //If room is in (0,0) it is the starting one, we mark it with an "s" and save the "s"
                        if (i + dun.boundaries.MinBoundaries.X * 2 == 0 && j + dun.boundaries.MinBoundaries.Y * 2 == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(" s");
                            roomDataInFile.type = "s";
                            roomDataInFile.Enemies = dun.grid[x, y].enemies;
                            roomDataInFile.Treasures = 0;
                        }
                        //If it is a corridor, writes "c" in the file
                        else if (map[i, j] == Common.RoomType.CORRIDOR)
                        {
                            Console.Write(" c");
                            roomDataInFile.type = "c";
                        }
                        //If is the boss room, writes "B". Currently is where the Triforce is located
                        else if (map[i, j] == Common.RoomType.BOSS)
                        {
                            Console.Write(" B");
                            roomDataInFile.type = "B";
                            roomDataInFile.Enemies = dun.grid[x, y].enemies;
                            roomDataInFile.Treasures = 0;
                            roomDataInFile.EnemiesType = enemyType_Randomizer;
                        }
                        //If negative, is a locked corridor, save it as the negative number of the key that opens it
                        else if (map[i, j] < 0)
                        {
                            Console.Write("{0,2}", map[i, j]);

                            roomDataInFile.locks = new List<int>
                            {
                                map[i, j]
                            };
                        }
                        //If it was a room with treasure, save it as a "T"
                        //TODO: change this as now every room may contain treasures, enemies and/or keys
                        else if (map[i, j] == Common.RoomType.TREASURE)
                        {
                            Console.Write("{0,2}", map[i, j]);

                            int maxPossibleItems = Math.Min(_treasureRuntimeSetSO.Items.Count + 1, remainingItems + 1);
                            int numberItems = UnityEngine.Random.Range(0, maxPossibleItems);
                            int numberNpcs;

                            int difficulty = dun.grid[x, y].enemies;
                            if (remainingNpcs > 0)
                            {
                                numberNpcs = UnityEngine.Random.Range(0, 2);
                            }
                            else
                            {
                                numberNpcs = 0;
                            }
                            if (numberNpcs > 0)
                            {
                                numberNpcs = (dun.DungeonQuestLine.NpcParametersForQuestLine.totalNpcs - remainingNpcs + 1);
                                difficulty = 0;
                            }

                            roomDataInFile.Npcs = numberNpcs;
                            remainingItems -= numberItems;
                            remainingNpcs--;

                            roomDataInFile.Enemies = difficulty;
                            roomDataInFile.Treasures = numberItems;
                            roomDataInFile.EnemiesType = enemyType_Randomizer;
                        }
                        //If the room has a positive value, it holds a key.
                        //Save the key index so we know what key it is
                        else if (map[i, j] > 0)
                        {
                            Console.Write("{0,2}", map[i, j]);
                            int difficulty = dun.grid[x, y].enemies;


                            roomDataInFile.Enemies = difficulty;
                            roomDataInFile.Treasures = 0;
                            roomDataInFile.EnemiesType = enemyType_Randomizer;
                            int maxPossibleItems = Math.Min(_treasureRuntimeSetSO.Items.Count, remainingItems + 1);
                            int numberItems = UnityEngine.Random.Range(0, maxPossibleItems);

                            roomDataInFile.Npcs = 0;
                            remainingItems -= numberItems;

                            roomDataInFile.keys = new List<int>
                            {
                                map[i, j]
                            };
                        }
                        //If the cell was none of the above, it must be an empty room
                        else
                        {
                            Console.Write("{0,2}", map[i, j]);
                            int maxPossibleItems = Math.Min(_treasureRuntimeSetSO.Items.Count + 1, remainingItems + 1);
                            int numberItems = UnityEngine.Random.Range(0, maxPossibleItems);
                            roomDataInFile.Treasures = numberItems;
                            //TODO Logica de carregar inimigos de acordo com probabilidade

                            roomDataInFile.EnemiesType = enemyType_Randomizer;
                            int numberNpcs;
                            int difficulty = dun.grid[x, y].enemies;
                            if (remainingNpcs > 0)
                            {
                                numberNpcs = UnityEngine.Random.Range(0, 2);
                            }
                            else
                            {
                                numberNpcs = 0;
                            }
                            if(numberNpcs > 0)
                            {
                                numberNpcs = (dun.DungeonQuestLine.NpcParametersForQuestLine.totalNpcs - remainingNpcs +1);
                                difficulty = 0;
                            }

                            roomDataInFile.Npcs = numberNpcs;
                            remainingItems -= numberItems;
                            remainingNpcs--;

                            roomDataInFile.Enemies = difficulty;
                        }
                    }
                    if (roomDataInFile != null)
                    {
                        dungeonFileSO.rooms.Add(roomDataInFile);
                    }
                }
                Console.Write("\n");
            }
            //The assetdatabase stuff only works in the Unity's Editor
            //As is, we can't save a level file in a released build of the game
#if UNITY_EDITOR
            int count = 0;
            string path;

            //Saves the file with the name of its input for the EA and adds a number at the end if a file with the same name exists
            //This prevents the file is overwritten
            path = AssetDatabase.AssetPathToGUID(foldername + filename + ".txt");
            while (path != "")
            {
                count++;
                path = AssetDatabase.AssetPathToGUID(foldername + filename + "-" + count + ".txt");
            }
            if (count > 0)
                filename += "-" + count;
            filename = foldername + filename;


            int sameFilenameCounter = 0;

            if(File.Exists(filename + ".asset"))
            {
                do
                {
                    sameFilenameCounter++;
                } while (File.Exists(filename + "-" + sameFilenameCounter + ".asset"));
                filename += "-"+sameFilenameCounter;
            }
            AssetDatabase.CreateAsset(dungeonFileSO, filename + ".asset");
            dun.DungeonQuestLine = new QuestLine();
            dun.DungeonQuestLine.Init();
            dun.DungeonQuestLine.DungeonFileSos.Add(dungeonFileSO);
            AssetDatabase.Refresh();
            Debug.Log("Finished Writing dungeon data");
#endif
        }
    }
}
