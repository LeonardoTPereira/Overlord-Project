using System;
using System.Collections.Generic;
using System.Text;

namespace Game.LevelGenerator
{
    /// This class holds level debug-purpose functions.
    public static class LevelDebug
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
                StringBuilder indent = new StringBuilder(_indent);
                // Print the nodes in the current stack
                for (int i = 0; i < last; i++)
                {
                    indent.Append(stacks[i].Count > 0 ? "|  " : "   ");
                }
                // Tag the nodes with the respective room type
                string tag = "";
                tag += node.Type == RoomType.Normal ? "N" : "";
                tag += node.Type == RoomType.Key ? "K" : "";
                tag += node.Type == RoomType.Locked ? "L" : "";
                Console.WriteLine(indent + "+- " + node.RoomID + "-" + tag);
                // Get non-null children nodes
                List<Room> next = new List<Room>();
                Room[] children = new Room[] {
                    node.Left,
                    node.Bottom,
                    node.Right
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
            int sizeX = _dungeon.MaxX - _dungeon.MinX + 1;
            int sizeY = _dungeon.MaxY - _dungeon.MinY + 1;
            int[,] map = new int[2 * sizeX, 2 * sizeY];
            for (int i = 0; i < 2 * sizeX; i++)
            {
                for (int j = 0; j < 2 * sizeY; j++)
                {
                    map[i, j] = Common.RoomType.NOTHING;
                }
            }

            // Set the corridors, keys and locked rooms
            RoomGrid grid = _dungeon.DungeonGrid;
            for (int i = _dungeon.MinX; i < _dungeon.MaxX + 1; ++i)
            {
                for (int j = _dungeon.MinY; j < _dungeon.MaxY + 1; ++j)
                {
                    int iep = (i - _dungeon.MinX) * 2;
                    int jep = (j - _dungeon.MinY) * 2;
                    Room current = grid[i, j];
                    if (current != null)
                    {
                        if (current.Type == RoomType.Normal)
                        {
                            map[iep, jep] = Common.RoomType.EMPTY;
                        }
                        // The key ID is the sequential positive index
                        else if (current.Type == RoomType.Key)
                        {
                            int _key = _dungeon.KeyIds.IndexOf(current.Key);
                            map[iep, jep] = _key + 1;
                        }
                        // If the room is locked, the room is a normal room,
                        // and the corridor is locked; but if the lock is the
                        // last one in the sequential order, then the room is
                        // the goal room
                        else if (current.Type == RoomType.Locked)
                        {
                            int _lock = _dungeon.LockIds.IndexOf(current.Key);
                            if (_lock == _dungeon.LockIds.Count - 1)
                            {
                                map[iep, jep] = Common.RoomType.BOSS;
                            }
                            else
                            {
                                map[iep, jep] = Common.RoomType.EMPTY;
                            }
                        }
                        // If the current room is a locked room and has a
                        // parent, then the connection between then is locked
                        // and is represented by the negative value of the
                        // index of the key that opens the lock
                        Room parent = current.Parent;
                        if (parent != null)
                        {
                            // Get the corridor between both rooms
                            int x = parent.X - current.X + iep;
                            int y = parent.Y - current.Y + jep;
                            // If the current room is locked
                            if (current.Type == RoomType.Locked)
                            {
                                // Then, the corridor is locked
                                int _key = _dungeon.KeyIds.IndexOf(current.Key);
                                map[x, y] = _key != -1 ? -(_key + 1) :
                                    Common.RoomType.NOTHING;
                            }
                            else
                            {
                                // Otherwise it is an usual corridor
                                map[x, y] = Common.RoomType.CORRIDOR;
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
                    if (map[i, j] == Common.RoomType.NOTHING)
                    {
                        Console.Write("  ");
                    }
                    else
                    {
                        if (i + _dungeon.MinX * 2 == 0 && j + _dungeon.MinY * 2 == 0)
                        {
                            Console.Write(" s");
                        }
                        else if (map[i, j] == Common.RoomType.CORRIDOR)
                        {
                            Console.Write(" c");
                        }
                        else if (map[i, j] == Common.RoomType.BOSS)
                        {
                            Console.Write(" B");
                        }
                        else if (map[i, j] < 0 || map[i, j] > 0)
                        {
                            Console.Write("{0,2}", map[i, j]);
                        }
                        else
                        {
                            Console.Write(" _");
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
            int sizeX = _dungeon.MaxX - _dungeon.MinX + 1;
            int sizeY = _dungeon.MaxY - _dungeon.MinY + 1;
            int[,] map = new int[2 * sizeX, 2 * sizeY];
            for (int i = 0; i < 2 * sizeX; i++)
            {
                for (int j = 0; j < 2 * sizeY; j++)
                {
                    map[i, j] = Common.RoomType.NOTHING;
                }
            }

            // Set the corridors, keys and locked rooms
            RoomGrid grid = _dungeon.DungeonGrid;
            for (int i = _dungeon.MinX; i < _dungeon.MaxX + 1; ++i)
            {
                for (int j = _dungeon.MinY; j < _dungeon.MaxY + 1; ++j)
                {
                    int iep = (i - _dungeon.MinX) * 2;
                    int jep = (j - _dungeon.MinY) * 2;
                    Room current = grid[i, j];
                    if (current != null)
                    {
                        map[iep, jep] = current.Enemies;
                        // If the current room is a locked room and has a
                        // parent, then the connection between then is locked
                        // and is represented by the negative value of the
                        // index of the key that opens the lock
                        Room parent = current.Parent;
                        if (parent != null)
                        {
                            // Get the corridor between both rooms
                            int x = parent.X - current.X + iep;
                            int y = parent.Y - current.Y + jep;
                            // If the current room is locked
                            if (current.Type == RoomType.Locked)
                            {
                                // Then, the corridor is locked
                                int _key = _dungeon.KeyIds.IndexOf(current.Key);
                                map[x, y] = _key != -1 ? -(_key + 1) :
                                    Common.RoomType.NOTHING;
                            }
                            else
                            {
                                // Otherwise it is an usual corridor
                                map[x, y] = Common.RoomType.CORRIDOR;
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
                    if (map[i, j] == Common.RoomType.NOTHING)
                    {
                        Console.Write("  ");
                    }
                    else
                    {
                        if (i + _dungeon.MinX * 2 == 0 && j + _dungeon.MinY * 2 == 0)
                        {
                            Console.Write(" s");
                        }
                        else if (map[i, j] == Common.RoomType.CORRIDOR)
                        {
                            Console.Write(" c");
                        }
                        else if (map[i, j] == Common.RoomType.BOSS)
                        {
                            Console.Write(" B");
                        }
                        else if (map[i, j] < 0 || map[i, j] > 0)
                        {
                            Console.Write("{0,2}", map[i, j]);
                        }
                        else
                        {
                            Console.Write(" _");
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
            if (_code == Common.RoomType.EMPTY)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
            }
            // If the room is a boss room
            else if (_code == Common.RoomType.BOSS)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            // If the room is a corridor
            else if (_code == Common.RoomType.CORRIDOR)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
            }
            // If there is no room
            else if (_code == Common.RoomType.NOTHING)
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