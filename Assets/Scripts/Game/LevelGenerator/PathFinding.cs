using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.LevelGenerator
{
    class PathFinding
    {
        protected Dungeon dungeon;
        protected Location start = null;
        protected Location target = null;
        protected int sizeX;
        protected int sizeY;
        protected int[,] map;
        protected List<Location> path = new List<Location>();
        protected List<Location> locksLocation = new List<Location>();
        protected List<Location> allLocksLocation = new List<Location>();
        protected List<Location> openList = new List<Location>();
        private List<Location> closedList = new List<Location>();
        private int nVisitedRooms = 0;
        private int neededLocks = 0;

        public List<Location> ClosedList {
            get => closedList;
            set => closedList = value;
        }
        public int NVisitedRooms {
            get => nVisitedRooms;
            set => nVisitedRooms = value;
        }
        public int NeededLocks {
            get => neededLocks;
            set => neededLocks = value;
        }

        /// Constructor.
        public PathFinding (
            Dungeon _dungeon
        ) {
            dungeon = _dungeon;
            neededLocks = 0;
            InitiatePathFinding();
        }

        /// Initiate the path finding setting map, sizes, rooms and filling the grid.
        private void InitiatePathFinding()
        {
            // The starting location is room (0,0)
            start = new Location {
                X = -2 * dungeon.MinX,
                Y = -2 * dungeon.MinY
            };
            // Size of the new grid
            sizeX = dungeon.MaxX - dungeon.MinX + 1;
            sizeY = dungeon.MaxY - dungeon.MinY + 1;
            map = new int[2 * sizeX, 2 * sizeY];
            // 101 is EMPTY
            for (int i = 0; i < 2 * sizeX; i++)
            {
                for (int j = 0; j < 2 * sizeY; j++)
                {
                    map[i, j] = 101;
                }
            }
            // Fill the new grid
            for (int i = dungeon.MinX; i < dungeon.MaxX + 1; i++)
            {
                for (int j = dungeon.MinY; j < dungeon.MaxY + 1; j++)
                {
                    // Convert the original coordinates (may be negative) to positive
                    int iPositive = i - dungeon.MinX;
                    int jPositive = j - dungeon.MinY;
                    Room current = dungeon.DungeonGrid[i, j];
                    // If the position has a room, check its type and fill the grid accordingly
                    if (current != null)
                    {
                        // 0 is a NORMAL ROOM
                        if (current.Type == RoomType.Normal)
                        {
                            map[iPositive * 2, jPositive * 2] = 0;
                        }
                        // The sequential, positivie index of the key is its representation
                        else if (current.Type == RoomType.Key)
                        {
                            map[iPositive * 2, jPositive * 2] = dungeon.KeyIds.IndexOf(current.Key) + 1;
                        }
                        // If the room is locked, the room is a normal room, only the corridor is locked. But is the lock is the last one in the sequential order, than the room is the objective
                        else if (current.Type == RoomType.Locked)
                        {
                            if (dungeon.LockIds.IndexOf(current.Key) == dungeon.LockIds.Count - 1)
                            {
                                map[iPositive * 2, jPositive * 2] = 102;
                                target = new Location { X = iPositive * 2, Y = jPositive * 2 };
                            }
                            else
                                map[iPositive * 2, jPositive * 2] = 0;
                        }
                        Room parent = current.Parent;
                        // If the actual room is a locked room and has a parent, then the connection between then is locked and is represented by the negative value of the index of the key that opens the lock
                        if (parent != null)
                        {
                            int x = parent.X - current.X + iPositive * 2;
                            int y = parent.Y - current.Y + jPositive * 2;
                            if (current.Type == RoomType.Locked)
                            {
                                locksLocation.Add(new Location { X = x, Y = y, Parent = new Location { X = 2 * (parent.X - current.X) + iPositive * 2, Y = 2 * (parent.Y - current.Y) + jPositive * 2 } });
                                int test = dungeon.KeyIds.IndexOf(current.Key);
                                if (test == -1)
                                {
                                    System.Console.WriteLine("There's a missing key here! What????");
                                    Console.ReadKey();
                                    map[x, y] = 100;
                                }
                                else
                                {
                                    map[x, y] = -(dungeon.KeyIds.IndexOf(current.Key) + 1);
                                }
                            }
                            // If the connection is open, 100 represents a normal corridor
                            else
                            {
                                map[x, y] = 100;
                            }
                        }
                    }
                }
            }
            // Add all the locks location to the list that will hold their values through the execution of the algorithm
            foreach (var locked in locksLocation)
            {
                allLocksLocation.Add(locked);
            }
        }

        /// Return a list of valid adjacent rooms (those which can be visited
        /// from the entered coordinate).
        public static List<Location> GetWalkableAdjacentSquares(
            int _x,
            int _y,
            int _sizeX,
            int _sizeY,
            int[,] _map
        ) {
            var proposedLocations = new List<Location>();
            if (_y > 0)
            {
                proposedLocations.Add(new Location { X = _x, Y = _y - 1 });
            }
            if (_y < 2 * _sizeY - 1)
            {
                proposedLocations.Add(new Location { X = _x, Y = _y + 1 });
            }
            if (_x > 0)
            {
                proposedLocations.Add(new Location { X = _x - 1, Y = _y });
            }
            if (_x < 2 * _sizeX - 1)
            {
                proposedLocations.Add(new Location { X = _x + 1, Y = _y });
            }
            return proposedLocations.Where(
                    l => (_map[l.X, l.Y] >= 0 && _map[l.X, l.Y] != 101)
                ).ToList();
        }

        /// Check if current location is a key room and...
        public void ValidateKeyRoom(
            Location _current
        ) {
            if (map[_current.X, _current.Y] > 0 &&
                map[_current.X, _current.Y] < 100
            ) {
                // If there is still a lock to be open (there may be more keys than locks in the level, so the verification is necessary) find its location and check if the key to open it is the one found
                if (locksLocation.Count > 0)
                {
                    foreach (var room in locksLocation)
                    {
                        // If the key we found is the one that open the room we are checking now, change the lock to an open corridor and update the algorithm's knowledge
                        if (map[room.X, room.Y] == -map[_current.X, _current.Y])
                        {
                            map[room.X, room.Y] = 100;
                            // Remove the lock from the unopenned locks location list
                            locksLocation.Remove(room);
                            // Check if the parent room of the locked room was already closed by the algorithm (if it was in the closed list)
                            foreach (var closedRoom in ClosedList)
                            {
                                // If it was already closed, reopen it. Remove from the closed list and add to the open list
                                if (closedRoom.X == room.Parent.X &&
                                    closedRoom.Y == room.Parent.Y
                                ) {
                                    ClosedList.Remove(closedRoom);
                                    nVisitedRooms--;
                                    openList.Add(closedRoom);
                                    // If the closed room was a locked one, also remove one of the needed locks, as it is now reopen and will be revisited
                                    foreach (var locked in allLocksLocation)
                                    {
                                        if (locked.X == closedRoom.X &&
                                            locked.Y == closedRoom.Y
                                        ) {
                                            neededLocks--;
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        /// Print all the path used by the algorithm.
        public static void PrintPathFinding(
            List<Location> _path,
            int _time = 10
        ) {
            int i = 1;
            foreach (var p in _path)
            {
                Console.SetCursorPosition(58, i++);
                Console.WriteLine(p.Y.ToString() + ", " + p.X.ToString());
                Console.SetCursorPosition(p.Y, p.X + 20);
                Console.Write('.');
                Console.SetCursorPosition(p.Y, p.X + 20);
                System.Threading.Thread.Sleep(_time);
            }
            Console.SetCursorPosition(0, 40);
        }

        /// NEED FIX -> run A* in random.seed(13).
        public static void PrintPathFound(
            Location _current,
            int _time = 10
        ) {
            while (_current != null)
            {
                Console.SetCursorPosition(_current.Y + 20, _current.X + 20);
                Console.Write('_');
                Console.SetCursorPosition(_current.Y + 20, _current.X + 20);
                _current = _current.Parent;
                System.Threading.Thread.Sleep(_time);
            }
            Console.SetCursorPosition(0, 40);
        }
    }
}
