using LevelGenerator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace LevelGenerator
{
    class Interface
    {
        /*
         * Prints the tree in the command line in a pretty structure
         * Not being used anymore for quite some time, but is useful if any changes in the tree structure are made
         */
        public static void PrintTree(Room root)
        {
            string typeString = "?";
            Room first = root;
            List<Room> firstStack = new List<Room>();
            List<Room> aux;
            firstStack.Add(first);

            List<List<Room>> childListStack = new List<List<Room>>();
            childListStack.Add(firstStack);

            while (childListStack.Count > 0)
            {
                List<Room> childStack = childListStack[childListStack.Count - 1];

                if (childStack.Count == 0)
                {
                    childListStack.RemoveAt(childListStack.Count - 1);
                }
                else
                {
                    first = childStack[0];
                    childStack.RemoveAt(0);

                    string indent = "";
                    for (int i = 0; i < childListStack.Count - 1; i++)
                    {
                        indent += (childListStack[i].Count > 0) ? "|  " : "   ";
                    }
                    //Sets the string representing the type of the room accordingly
                    Type type = first.Type;
                    if (type == Type.normal)
                        typeString = "N";
                    else if (type == Type.key)
                        typeString = "K";
                    else if (type == Type.locked)
                        typeString = "L";
                    else
                    {
                        Console.WriteLine("Something went wrong printing the tree!\n");
                        Console.WriteLine("This Room type does not exist!\n\n");
                    }
                    Console.WriteLine(indent + "+- " + first.RoomId + "-" + typeString);

                    aux = new List<Room>();

                    if (first.LeftChild != null)
                        aux.Add(first.LeftChild);
                    if (first.BottomChild != null)
                        aux.Add(first.BottomChild);
                    if (first.RightChild != null)
                        aux.Add(first.RightChild);

                    if (aux.Count > 0)
                    {
                        childListStack.Add(aux);
                    }
                }
            }
            Console.Write("\n");
        }

       /**
        * Prints the dungeon in the console, saves into a file, and can even save in a csv that is not used anymore
        * We now save it directly into a Unity's Resource Directory
        */
        public static void PrintNumericalGridWithConnections(Dungeon dun)
        {
            //Data to navigate the dungeon to print
            Room actualRoom, parent;
            RoomGrid grid = dun.roomGrid;
            Type type;
            int x, y, iPositive, jPositive;
            bool isRoom;

            //List of keys and locked rooms in the level
            List<int> lockedRooms = new List<int>();
            List<int> keys = new List<int>();

            //The boundaries of the level's grid
            int minX, minY, maxX, maxY;
            minX = Constants.MATRIXOFFSET;
            minY = Constants.MATRIXOFFSET;
            maxX = -Constants.MATRIXOFFSET;
            maxY = -Constants.MATRIXOFFSET;

            //Where to save the new dungeon in Unity
            string foldername = "Assets/Resources/Levels/";
            string filename, dungeonData = "";
            filename = "R" + Constants.nV + "-K" + Constants.nK + "-L" + Constants.nL + "-L" + Constants.lCoef;
            
            //saves where the dungeon grid begins and ends in each direction
            foreach (Room room in dun.RoomList)
            {
                if (room.Type == Type.key)
                    keys.Add(room.KeyToOpen);
                else if (room.Type == Type.locked)
                    lockedRooms.Add(room.KeyToOpen);
                if (room.X < minX)
                    minX = room.X;
                if (room.Y < minY)
                    minY = room.Y;
                if (room.X > maxX)
                    maxX = room.X;
                if (room.Y > maxY)
                    maxY = room.Y;
            }

            //The size is normalized to be always positive (easier to handle a matrix)
            int sizeX = maxX - minX + 1;
            int sizeY = maxY - minY + 1;

            //Creates a matrix to hold each room and corridor (there may be a corridor between each room, that must be saved
            //hence 2*size
            int[,] map = new int[2 * sizeX, 2 * sizeY];
            //The top of the dungeon's file in unity must contain its dimensions
            dungeonData += 2*sizeX + "\n";
            dungeonData += 2*sizeY + "\n";

            //We initialize the map with the equivalent of an empty cell
            for (int i = 0; i < 2 * sizeX; ++i)
            {
                for (int j = 0; j < 2 * sizeY; ++j)
                {
                    map[i, j] = Util.RoomType.NOTHING;
                }
            }

            //Now we visit each room and save the info on the corresponding cell of the matrix
            for (int i = minX; i < maxX + 1; ++i)
            {
                for (int j = minY; j < maxY + 1; ++j)
                {
                    //Converts the coordinate of the original grid (can be negative) to the positive ones used in the matrix
                    iPositive = i - minX;
                    jPositive = j - minY;
                    //Gets the actual room
                    actualRoom = grid[i, j];
                    //If there is something in this position in the grid:
                    if (actualRoom != null)
                    {
                        type = actualRoom.Type;
                        //If it is a normal room, check if is a leaf node. We are currently placing treasures there
                        //If not a leaf, just save as an empty room for now
                        //TODO: change to handle the new format of having the room's Key ID followed by amount of treasure and them enemy difficulty
                        //Will have to change to an array or something, with 0 treasures and 0 difficulty meaning no treasure and no enemy inside
                        if (type == Type.normal)
                        {
                            if(actualRoom.IsLeafNode())
                                map[iPositive * 2, jPositive * 2] = Util.RoomType.TREASURE;
                            else
                                map[iPositive * 2, jPositive * 2] = Util.RoomType.EMPTY;
                        }
                        //If the room has a key, saves the corresponding key index in the matrix
                        //TODO: Must also change to allow the generation of treasures and enemies
                        else if (type == Type.key)
                        {
                            map[iPositive * 2, jPositive * 2] = keys.IndexOf(actualRoom.KeyToOpen) + 1;
                        }
                        //If the room is locked from its parent, check if it is a boss room by checking if the key to open is the last one created
                        //It guarantees at least that is the deepest key in the tree, but not the longest route
                        //TODO: Must also change to allow the generation of treasures and enemies
                        else if (type == Type.locked)
                        {
                            if (lockedRooms.IndexOf(actualRoom.KeyToOpen) == lockedRooms.Count - 1)
                                map[iPositive * 2, jPositive * 2] = Util.RoomType.BOSS;
                            else
                                map[iPositive * 2, jPositive * 2] = Util.RoomType.TREASURE;
                        }
                        //If it is not a room, something is wrong
                        else
                        {
                            Console.WriteLine("Something went wrong printing the tree!\n");
                            Console.WriteLine("This Room type does not exist!\n\n");
                        }
                        //As (for now) every room must be connected to its parent or children
                        //We need only to check its parent to create the corridors
                        parent = actualRoom.Parent;
                        if (parent != null)
                        {
                            x = parent.X - actualRoom.X + 2 * iPositive;
                            y = parent.Y - actualRoom.Y + 2 * jPositive;
                            //If corridor is lockes, save the index of the key that opens it
                            //But as a negative value. A negative corridor is locked!
                            //If not, save it only as a normal corridor
                            if (type == Type.locked)
                                map[x, y] = -(keys.IndexOf(actualRoom.KeyToOpen) + 1);
                            else
                                map[x, y] = Util.RoomType.CORRIDOR;
                        }
                    }
                    //Else the room is empty, do nothing
                    else
                    {
                        //map[iPositive * 2, jPositive * 2] = 'O';
                    }
                }
            }

            //Now we print it/save to a file/whatever
            for (int i = 0; i < sizeX * 2; ++i)
            {
                for (int j = 0; j < sizeY * 2; ++j)
                {
                    isRoom = false;

                    //This whole block was to print in a console
                    if (map[i, j] == Util.RoomType.EMPTY)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;

                    }
                    else if (map[i, j] == Util.RoomType.CORRIDOR)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    }
                    else if (map[i, j] == 7)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else if (map[i, j] == Util.RoomType.NOTHING)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (map[i, j] == Util.RoomType.BOSS)
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

                    //If cell is empty, do nothing (or print empty space in console)
                    if (map[i, j] == Util.RoomType.NOTHING)
                    {
                        Console.Write("  ");
                        //writer.WriteLine(" ");
                    }
                    //If there is something (room or corridor) print/save
                    else
                    {
                        //For Unity's dungeon file we need to save the x and y position of the room
                        dungeonData += i + "\n";
                        dungeonData += j + "\n";
                        //this writerRG was used in the CSV pre-Unity, ignore it as legacy
                        //writerRG.WriteLine(i);
                        //writerRG.WriteLine(j);
                        //If room is in (0,0) it is the starting one, we mark it with an "s" and save the "s"
                        if (i + minX * 2 == 0 && j + minY * 2 == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(" s");
                            dungeonData += "s\n";
                            //TODO: save the info about the treasure and difficulty
                            //dungeonData += "0\n"; //Treasure
                            //dungeonData += "0\n"; //Difficulty
                            //writerRG.WriteLine("s");
                            //Marks that it is a room
                            isRoom = true;
                        }
                        //If it is a corridor, writes "c" in the file
                        else if (map[i, j] == Util.RoomType.CORRIDOR)
                        {
                            Console.Write(" c");
                            dungeonData += "c\n";
                            //writerRG.WriteLine("c");
                        }
                        //If is the boss room, writes "B". Currently is where the Triforce is located
                        else if (map[i, j] == Util.RoomType.BOSS)
                        {
                            Console.Write(" B");
                            dungeonData += "B\n";
                            //TODO: save the info about the treasure and difficulty
                            //dungeonData += "0\n"; //Treasure
                            //dungeonData += "0\n"; //Difficulty
                            //writerRG.WriteLine("B");
                            //Marks that it is a room
                            isRoom = true;
                        }
                        //If negative, is a locked corridor, save it as the negative number of the key that opens it
                        else if (map[i, j] < 0)
                        {
                            Console.Write("{0,2}", map[i, j]);
                            dungeonData += map[i, j] + "\n";
                            //writerRG.WriteLine(map[i, j]);
                        }
                        //If it was a room with treasure, save it as a "T"
                        //TODO: change this as now every room may contain treasures, enemies and/or keys
                        else if (map[i, j] == Util.RoomType.TREASURE)
                        {
                            Console.Write("{0,2}", map[i, j]);
                            dungeonData += "T\n";
                            //TODO: save the info about the treasure and difficulty
                            //dungeonData += "50\n"; //Treasure
                            //dungeonData += "0\n"; //Difficulty
                            //writerRG.WriteLine("T");
                            isRoom = true;
                        }
                        //If the room has a positive value, it holds a key.
                        //Save the key index so we know what key it is
                        else if (map[i, j] > 0)
                        {
                            Console.Write("{0,2}", map[i, j]);
                            dungeonData += map[i, j] + "\n";
                            //TODO: save the info about the treasure and difficulty
                            //dungeonData += "0\n"; //Treasure
                            //dungeonData += "0\n"; //Difficulty
                            //writerRG.WriteLine(map[i, j]);
                            isRoom = true;
                        }
                        //If the cell was none of the above, it must be an empty room
                        else
                        {
                            Console.Write("{0,2}", map[i, j]);
                            dungeonData += map[i, j] + "\n";
                            //TODO: save the info about the treasure and difficulty
                            //dungeonData += "0\n"; //Treasure
                            //dungeonData += "0\n"; //Difficulty
                            //writerRG.WriteLine(map[i, j]);
                            isRoom = true;
                        }

                    }
                }
                Console.Write("\n");
                //writer.Write("\r\n");
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
                path = AssetDatabase.AssetPathToGUID(foldername + filename +"-"+ count + ".txt");
            }
            if (count > 0)
                filename += "-"+count;
            filename = foldername + filename + ".txt";
            //UnityEngine.Debug.Log("Filename: " + filename);

            //Finally, saves the data
            using (StreamWriter writer = new StreamWriter(filename, false, Encoding.UTF8))
            {
                UnityEngine.Debug.Log("Writing dungeon data");
                writer.Write(dungeonData);
                writer.Flush();
                writer.Close();
                //writerRG.Flush();
                //writerRG.Close();
                Console.Write("\n");
            }
            UnityEngine.Debug.Log("Finished Writing dungeon data");
#endif
        }
    }
}
