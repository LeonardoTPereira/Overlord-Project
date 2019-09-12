using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LevelGenerator
{
    class PathFinding
    {
        //protected int auxoffset = 0;

        private int nVisitedRooms = 0;
        private int neededLocks = 0;
        protected Dungeon dun;
        protected List<Location> path = new List<Location>();

        protected Location current = null;
        protected Location start = null;
        protected Location target = null;

        protected List<Location> locksLocation = new List<Location>();
        protected List<Location> allLocksLocation = new List<Location>();

        protected Room actualRoom, parent;
        protected RoomGrid grid;

        protected Type type;

        protected int x, y, iPositive, jPositive;

        protected List<int> lockedRooms = new List<int>();
        protected List<int> keys = new List<int>();
        
        protected List<Location> openList = new List<Location>();
        private List<Location> closedList = new List<Location>();

        protected int minX, minY, maxX, maxY;

        protected int sizeX;
        protected int sizeY;

        protected int[,] map;

        public List<Location> ClosedList { get => closedList; set => closedList = value; }
        public int NVisitedRooms { get => nVisitedRooms; set => nVisitedRooms = value; }
        public int NeededLocks { get => neededLocks; set => neededLocks = value; }

        // Constructor
        public PathFinding (Dungeon _dun)
        {
            dun = _dun.Copy();
            neededLocks= 0;

            grid = dun.roomGrid;
            minX = Constants.MATRIXOFFSET;
            minY = Constants.MATRIXOFFSET;
            maxX = -Constants.MATRIXOFFSET;
            maxY = -Constants.MATRIXOFFSET;

            initiatePathFinding();
        }

        // Check what adjacent rooms exits and can be visited and return the valid ones
        public static List<Location> GetWalkableAdjacentSquares(int x, int y, int sizeX, int sizeY, int[,] map)
        {
            var proposedLocations = new List<Location>();
            if (y > 0)
                proposedLocations.Add(new Location { X = x, Y = y - 1 });
            if (y < (2 * sizeY) - 1)
                proposedLocations.Add(new Location { X = x, Y = y + 1 });
            if (x > 0)
                proposedLocations.Add(new Location { X = x - 1, Y = y });
            if (x < (2 * sizeX) - 1)
                proposedLocations.Add(new Location { X = x + 1, Y = y });

            return proposedLocations.Where(l => (map[l.X, l.Y] >= 0 && map[l.X, l.Y] != 101)).ToList();
        }

        // Check if current location is a key room and...
        public void validateKeyRoom(Location current)
        {
            if (map[current.X, current.Y] > 0 && map[current.X, current.Y] < 100)
            {
                //If there is still a lock to be open (there may be more keys than locks in the level, so the verification is necessary) find its location and check if the key to open it is the one found
                if (locksLocation.Count > 0)
                {
                    foreach (var room in locksLocation)
                    {
                        //If the key we found is the one that open the room we are checking now, change the lock to an open corridor and update the algorithm's knowledge
                        if (map[room.X, room.Y] == -map[current.X, current.Y])
                        {
                            map[room.X, room.Y] = 100;
                            //remove the lock from the unopenned locks location list
                            locksLocation.Remove(room);
                            //Check if the parent room of the locked room was already closed by the algorithm (if it was in the closed list)
                            foreach (var closedRoom in ClosedList)
                            {
                                //If it was already closed, reopen it. Remove from the closed list and add to the open list
                                if (closedRoom.X == room.Parent.X && closedRoom.Y == room.Parent.Y)
                                {
                                    ClosedList.Remove(closedRoom);
                                    nVisitedRooms--;
                                    //Console.SetCursorPosition(0, 15 + auxoffset);
                                    //auxoffset += 1;
                                    //Console.WriteLine("Removed!");
                                    openList.Add(closedRoom);
                                    //If the closed room was a locked one, also remove one of the needed locks, as it is now reopen and will be revisited
                                    foreach (var locked in allLocksLocation)
                                    {
                                        if (locked.X == closedRoom.X && locked.Y == closedRoom.Y)
                                        {
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

        // Print all the path used by the algorithm
        public static void PrintPathFinding(List<Location> path, int time = 10)
        {
            int i = 1;
            foreach (var p in path)
            {
                Console.SetCursorPosition(58, i++);
                Console.WriteLine(p.Y.ToString() + ", " + p.X.ToString());

                // show current square on the map
                Console.SetCursorPosition(p.Y, p.X + 20);
                Console.Write('.');
                Console.SetCursorPosition(p.Y, p.X + 20);
                System.Threading.Thread.Sleep(time);
            }
            Console.SetCursorPosition(0, 40);
        }

        // NEED FIX -> run A* in random.seed(13)
        public static void PrintPathFound(Location current, int time = 10)
        {
            while (current != null)
            {
                Console.SetCursorPosition(current.Y + 20, current.X + 20);
                Console.Write('_');
                Console.SetCursorPosition(current.Y + 20, current.X + 20);
                current = current.Parent;
                System.Threading.Thread.Sleep(time);
            }
            Console.SetCursorPosition(0, 40);
        }

        // Initiate the path finding setting map, sizes, rooms and filling the grid
        private void initiatePathFinding()
        {
            foreach (Room room in dun.RoomList)
            {
                if (room.Type == Type.key)
                    keys.Add(room.KeyToOpen);
                if (room.Type == Type.locked)
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

            //The starting location is room (0,0)
            start = new Location { X = -2 * minX, Y = -2 * minY };
            //Size of the new grid
            sizeX = maxX - minX + 1;
            sizeY = maxY - minY + 1;
            map = new int[2 * sizeX, 2 * sizeY];

            //101 is EMPTY
            for (int i = 0; i < 2 * sizeX; ++i)
            {
                for (int j = 0; j < 2 * sizeY; ++j)
                {
                    map[i, j] = 101;
                }
            }
            //Fill the new grid
            for (int i = minX; i < maxX + 1; ++i)
            {
                for (int j = minY; j < maxY + 1; ++j)
                {
                    //Converts the original coordinates (may be negative) to positive
                    iPositive = i - minX;
                    jPositive = j - minY;
                    actualRoom = grid[i, j];
                    //If the position has a room, check its type and fill the grid accordingly
                    if (actualRoom != null)
                    {
                        type = actualRoom.Type;
                        //0 is a NORMAL ROOM
                        if (type == Type.normal)
                        {
                            map[iPositive * 2, jPositive * 2] = 0;
                        }
                        //The sequential, positivie index of the key is its representation
                        else if (type == Type.key)
                        {
                            map[iPositive * 2, jPositive * 2] = keys.IndexOf(actualRoom.KeyToOpen) + 1;
                        }
                        //If the room is locked, the room is a normal room, only the corridor is locked. But is the lock is the last one in the sequential order, than the room is the objective
                        else if (type == Type.locked)
                        {
                            if (lockedRooms.IndexOf(actualRoom.KeyToOpen) == lockedRooms.Count - 1)
                            {
                                map[iPositive * 2, jPositive * 2] = 102;
                                target = new Location { X = iPositive * 2, Y = jPositive * 2 };
                            }
                            else
                                map[iPositive * 2, jPositive * 2] = 0;
                        }
                        else
                        {
                            Debug.Log("Something went wrong printing the tree!\n");
                            Debug.Log("This Room type does not exist!\n\n");
                        }
                        parent = actualRoom.Parent;
                        //If the actual room is a locked room and has a parent, then the connection between then is locked and is represented by the negative value of the index of the key that opens the lock
                        if (parent != null)
                        {

                            x = parent.X - actualRoom.X + 2 * iPositive;
                            y = parent.Y - actualRoom.Y + 2 * jPositive;
                            if (type == Type.locked)
                            {
                                locksLocation.Add(new Location { X = x, Y = y, Parent = new Location { X = 2 * (parent.X - actualRoom.X) + 2 * iPositive, Y = 2 * (parent.Y - actualRoom.Y) + 2 * jPositive } });
                                int test = keys.IndexOf(actualRoom.KeyToOpen);
                                if (test == -1)
                                {
                                    Debug.Log("There's a missing key here! What????");
                                    Console.ReadKey();
                                    map[x, y] = 100;
                                }
                                else
                                    map[x, y] = -(keys.IndexOf(actualRoom.KeyToOpen) + 1);
                            }
                            //If the connection is open, 100 represents a normal corridor
                            else
                                map[x, y] = 100;
                        }
                    }
                }
            }
            //Add all the locks location to the list that will hold their values through the execution of the algorithm
            foreach (var locked in locksLocation)
            {
                allLocksLocation.Add(locked);
            }
        }
    }
}
