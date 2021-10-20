using System;
using System.Collections.Generic;

namespace LevelGenerator
{
    /// This class holds level debug-purpose functions.
    class LevelDebug
    {
        /// Indent of debugging functions and methods.
        public static readonly string INDENT = "  ";

        /// Print the tree representation of the entered level.
        public static void PrintTree(
            Dungeon _dungeon,
            string _indent
        ) {
            // Get tree root
            Room root = _dungeon.Rooms[0];
            // This list holds lists of nodes children
            List<List<Room>> stacks = new List<List<Room>>();
            // Add the root in the list of stacks
            stacks.Add(new List<Room>() { root });
            // Print the tree
            while (stacks.Count > 0)
            {
                // Calculate the last added stack index
                int last = stacks.Count - 1;
                // Get the last added stack
                List<Room> current = stacks[last];
                // Remove empty stacks
                if (current.Count == 0)
                {
                    stacks.RemoveAt(last);
                    continue;
                }
                // Get the first node and remove if from the list of nodes
                Room node = current[0];
                current.RemoveAt(0);
                string indent = _indent;
                // Print the nodes in the current stack
                for (int i = 0; i < last; i++)
                {
                    indent += (stacks[i].Count > 0) ? "|  " : "   ";
                }
                // Tag the nodes with the respective room type
                string tag = "";
                tag += node.type == RoomType.Normal ? "N" : "";
                tag += node.type == RoomType.Key ? "K" : "";
                tag += node.type == RoomType.Locked ? "L" : "";
                Console.WriteLine(indent + "+- " + node.id + "-" + tag);
                // Get non-null children nodes
                List<Room> next = new List<Room>();
                Room[] children = new Room[] {
                    node.left,
                    node.bottom,
                    node.right
                };
                foreach (Room child in children)
                {
                    if (child != null)
                    {
                        next.Add(child);
                    }
                }
                // Keep printing the branch while there are nodes
                if (next.Count > 0)
                {
                    stacks.Add(next);
                }
            }
        }

        /// Print the map with missions.
        public static void PrintMissionMap(
            Dungeon _dungeon,
            string _indent
        ) {
            // Initialize the auxiliary map
            int sizeX = _dungeon.maxX - _dungeon.minX + 1;
            int sizeY = _dungeon.maxY - _dungeon.minY + 1;
            int[,] map = new int[2 * sizeX, 2 * sizeY];
            for (int i = 0; i < 2 * sizeX; i++)
            {
                for (int j = 0; j < 2 * sizeY; j++)
                {
                    map[i, j] = (int) Common.RoomCode.E;
                }
            }

            // Set the corridors, keys and locked rooms
            RoomGrid grid = _dungeon.grid;
            for (int i = _dungeon.minX; i < _dungeon.maxX + 1; ++i)
            {
                for (int j = _dungeon.minY; j < _dungeon.maxY + 1; ++j)
                {
                    int iep = (i - _dungeon.minX) * 2;
                    int jep = (j - _dungeon.minY) * 2;
                    Room current = grid[i, j];
                    if (current != null)
                    {
                        if (current.type == RoomType.Normal)
                        {
                            map[iep, jep] = (int) Common.RoomCode.N;
                        }
                        // The key ID is the sequential positive index
                        else if (current.type == RoomType.Key)
                        {
                            int _key = _dungeon.keyIds.IndexOf(current.key);
                            map[iep, jep] = _key + 1;
                        }
                        // If the room is locked, the room is a normal room,
                        // and the corridor is locked; but if the lock is the
                        // last one in the sequential order, then the room is
                        // the goal room
                        else if (current.type == RoomType.Locked)
                        {
                            int _lock = _dungeon.lockIds.IndexOf(current.key);
                            if (_lock == _dungeon.lockIds.Count - 1)
                            {
                                map[iep, jep] = (int) Common.RoomCode.B;
                            }
                            else
                            {
                                map[iep, jep] = (int) Common.RoomCode.N;
                            }
                        }
                        // If the current room is a locked room and has a
                        // parent, then the connection between then is locked
                        // and is represented by the negative value of the
                        // index of the key that opens the lock
                        Room parent = current.parent;
                        if (parent != null)
                        {
                            // Get the corridor between both rooms
                            int x = parent.x - current.x + iep;
                            int y = parent.y - current.y + jep;
                            // If the current room is locked
                            if (current.type == RoomType.Locked)
                            {
                                // Then, the corridor is locked
                                int _key = _dungeon.keyIds.IndexOf(current.key);
                                map[x, y] = _key != -1 ? -(_key + 1) :
                                    (int) Common.RoomCode.E;
                            }
                            else
                            {
                                // Otherwise it is an usual corridor
                                map[x, y] = (int) Common.RoomCode.C;
                            }
                        }
                    }
                }
            }

            // Print the dungeon in the console
            for (int i = 0; i < sizeX * 2; i++)
            {
                Console.Write(_indent);
                for (int j = 0; j < sizeY * 2; j++)
                {
                    // Set the room color
                    SetRoomColor(map[i, j]);
                    // Check room cores and print the corresponding string code
                    if (map[i, j] == (int) Common.RoomCode.E)
                    {
                        Console.Write("  ");
                    }
                    else
                    {
                        if (i + _dungeon.minX * 2 == 0 && j + _dungeon.minY * 2 == 0)
                        {
                            Console.Write(" s");
                        }
                        else if (map[i, j] == (int) Common.RoomCode.C)
                        {
                            Console.Write(" c");
                        }
                        else if (map[i, j] == (int) Common.RoomCode.B)
                        {
                            Console.Write(" B");
                        }
                        else if (map[i, j] < 0 || map[i, j] > 0)
                        {
                            Console.Write("{0,2}", map[i, j]);
                        }
                        else
                        {
                            Console.Write(" _", map[i, j]);
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        /// Print the map with enemies.
        public static void PrintEnemyMap(
            Dungeon _dungeon,
            string _indent
        ) {
            // Initialize the auxiliary map
            int sizeX = _dungeon.maxX - _dungeon.minX + 1;
            int sizeY = _dungeon.maxY - _dungeon.minY + 1;
            int[,] map = new int[2 * sizeX, 2 * sizeY];
            for (int i = 0; i < 2 * sizeX; i++)
            {
                for (int j = 0; j < 2 * sizeY; j++)
                {
                    map[i, j] = (int) Common.RoomCode.E;
                }
            }

            // Set the corridors, keys and locked rooms
            RoomGrid grid = _dungeon.grid;
            for (int i = _dungeon.minX; i < _dungeon.maxX + 1; ++i)
            {
                for (int j = _dungeon.minY; j < _dungeon.maxY + 1; ++j)
                {
                    int iep = (i - _dungeon.minX) * 2;
                    int jep = (j - _dungeon.minY) * 2;
                    Room current = grid[i, j];
                    if (current != null)
                    {
                        map[iep, jep] = current.enemies;
                        // If the current room is a locked room and has a
                        // parent, then the connection between then is locked
                        // and is represented by the negative value of the
                        // index of the key that opens the lock
                        Room parent = current.parent;
                        if (parent != null)
                        {
                            // Get the corridor between both rooms
                            int x = parent.x - current.x + iep;
                            int y = parent.y - current.y + jep;
                            // If the current room is locked
                            if (current.type == RoomType.Locked)
                            {
                                // Then, the corridor is locked
                                int _key = _dungeon.keyIds.IndexOf(current.key);
                                map[x, y] = _key != -1 ? -(_key + 1) :
                                    (int) Common.RoomCode.E;
                            }
                            else
                            {
                                // Otherwise it is an usual corridor
                                map[x, y] = (int) Common.RoomCode.C;
                            }
                        }
                    }
                }
            }

            // Print the dungeon in the console
            for (int i = 0; i < sizeX * 2; i++)
            {
                Console.Write(_indent);
                for (int j = 0; j < sizeY * 2; j++)
                {
                    // Set the room color
                    SetRoomColor(map[i, j]);
                    // Check room cores and print the corresponding string code
                    if (map[i, j] == (int) Common.RoomCode.E)
                    {
                        Console.Write("  ");
                    }
                    else
                    {
                        if (i + _dungeon.minX * 2 == 0 && j + _dungeon.minY * 2 == 0)
                        {
                            Console.Write(" s");
                        }
                        else if (map[i, j] == (int) Common.RoomCode.C)
                        {
                            Console.Write(" c");
                        }
                        else if (map[i, j] == (int) Common.RoomCode.B)
                        {
                            Console.Write(" B");
                        }
                        else if (map[i, j] < 0 || map[i, j] > 0)
                        {
                            Console.Write("{0,2}", map[i, j]);
                        }
                        else
                        {
                            Console.Write(" _", map[i, j]);
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        /// Define the room color that will be printed on the console.
        private static void SetRoomColor(
            int _code
        ) {
            // If the room is a room
            if (_code == (int) Common.RoomCode.N)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
            }
            // If the room is a boss room
            else if (_code == (int) Common.RoomCode.B)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            // If the room is a corridor
            else if (_code == (int) Common.RoomCode.C)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
            }
            // If there is no room
            else if (_code == (int) Common.RoomCode.E)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            // If the room has a key
            else if (_code > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }
            // If the room is locked
            else if (_code < 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
            }
        }
    }
}