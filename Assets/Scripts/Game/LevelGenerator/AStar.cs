using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Thanks to https://bitbucket.org/dandago/experimental/src/7adeb5f8cdfb054b540887d53cabf27e22a10059/AStarPathfinding/?at=master
namespace Game.LevelGenerator
{
    /// Class with location, heuristic and real distances of the room and the
    /// room that was used to go to the current one (parent).
    class Location
    {
        public int X;
        public int Y;
        public int F;
        public int G;
        public int H;
        public Location Parent;
    }

    class AStar
    {
        static int sizeX;
        static int sizeY;

        /// The A* algorithm.
        public static int FindRoute(
            Dungeon _dungeon
        ) {
            // Counter for all the locks that were opened during the A* execution
            // TODO: Make the A* (or another algorithm) use only the really needed ones, the A* in the search phase opens some unecessary locked doors, but this could be prevented
            // By making partial A* from the start to the key of the first locked door found, then from the key to the door, from the door to the key to the next locked one, and so on
            int neededLocks = 0;

            // Location of the locks that were still not opened
            List<Location> locksLocation = new List<Location>();
            // Location of all the locks since the start of the algorithm
            List<Location> allLocksLocation = new List<Location>();

            //  The starting location is room (0,0)
            Location start = new Location {
                X = -2 * _dungeon.MinX,
                Y = -2 * _dungeon.MinY
            };
            Location target = null;

            int g = 0;
            // Size of the new grid
            sizeX = _dungeon.MaxX - _dungeon.MinX + 1;
            sizeY = _dungeon.MaxY - _dungeon.MinY + 1;
            int[,] map = new int[2 * sizeX, 2 * sizeY];

            // 101 is EMPTY
            for (int i = 0; i < 2 * sizeX; i++)
            {
                for (int j = 0; j < 2 * sizeY; j++)
                {
                    map[i, j] = 101;
                }
            }
            // Fill the new grid
            for (int i = _dungeon.MinX; i < _dungeon.MaxX + 1; i++)
            {
                for (int j = _dungeon.MinY; j < _dungeon.MaxY + 1; j++)
                {
                    // Convert the original coordinates (may be negative) to positive
                    int iPositive = i - _dungeon.MinX;
                    int jPositive = j - _dungeon.MinY;
                    Room current = _dungeon.DungeonGrid[i, j];
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
                            map[iPositive * 2, jPositive * 2] = _dungeon.KeyIds.IndexOf(current.Key)+1;
                        }
                        // If the room is locked, the room is a normal room, only the corridor is locked. But is the lock is the last one in the sequential order, than the room is the objective
                        else if (current.Type == RoomType.Locked)
                        {
                            if (_dungeon.LockIds.IndexOf(current.Key) == _dungeon.LockIds.Count -1)
                            {
                                map[iPositive * 2, jPositive * 2] = 102;
                            }
                            else
                                map[iPositive * 2, jPositive * 2] = 0;
                        }

                        if (current.IsGoal)
                        {
                            target = new Location { X = iPositive * 2, Y = jPositive * 2 };
                        }
                        Room parent = current.Parent;
                        // If the current room is a locked room and has a parent, then the connection between then is locked and is represented by the negative value of the index of the key that opens the lock
                        if (parent != null)
                        {

                            int x = parent.X - current.X + iPositive * 2;
                            int y = parent.Y - current.Y + jPositive * 2;
                            if (current.Type == RoomType.Locked)
                            {
                                locksLocation.Add(new Location { X = x, Y = y, Parent = new Location {
                                    X = 2 * (parent.X - current.X) + iPositive * 2,
                                    Y = 2 * (parent.Y - current.Y) + jPositive * 2 } }
                                );
                                map[x, y] = -(_dungeon.KeyIds.IndexOf(current.Key)+1);
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

            List<Location> path = new List<Location>();
            // List of visited rooms that are not closed yet
            List<Location> openList = new List<Location>();
            // List of closed rooms. They were visited and all neighboors explored.
            List<Location> closedList = new List<Location>();

            //start by adding the original position to the open list
            openList.Add(start);
            //While there are rooms to visit in the path
            while (openList.Count > 0)
            {
                // get the square with the lowest F score
                var lowest = openList.Min(l => l.F);
                Location current = openList.First(l => l.F == lowest);
                // If the current is a key, change the locked door status in the map
                if (map[current.X, current.Y] > 0 && map[current.X, current.Y] < 100)
                {
                    // If there is still a lock to be open (there may be more keys than locks in the level, so the verification is necessary) find its location and check if the key to open it is the one found
                    if (locksLocation.Count > 0)
                    {
                        foreach (var room in locksLocation)
                        {
                            // If the key we found is the one that open the room we are checking now, change the lock to an open corridor and update the algorithm's knowledge
                            if (map[room.X, room.Y] == -map[current.X, current.Y])
                            {
                                map[room.X, room.Y] = 100;
                                // Remove the lock from the unopenned locks location list
                                locksLocation.Remove(room);
                                // Check if the parent room of the locked room was already closed by the algorithm (if it was in the closed list)
                                foreach (var closedRoom in closedList)
                                {
                                    // If it was already closed, reopen it. Remove from the closed list and add to the open list
                                    if (closedRoom.X == room.Parent.X && closedRoom.Y == room.Parent.Y)
                                    {
                                        closedList.Remove(closedRoom);
                                        openList.Add(closedRoom);
                                        // If the closed room was a locked one, also remove one of the needed locks, as it is now reopen and will be revisited
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

                // Add the current square to the closed list
                closedList.Add(current);
                // Check if the current room is a locked one. If it is, add 1 to the number of locks needed to reach the goal
                foreach (var locked in allLocksLocation)
                {
                    if (locked.X == current.X && locked.Y == current.Y)
                    {
                        neededLocks++;
                        break;
                    }
                }

                // Remove it from the open list
                openList.Remove(current);

                // If we added the destination to the closed list, we've found a path
                if (closedList != null)
                {
                    Location first = null;

                    foreach (var l in closedList)
                    {
                        if (l.X != target.X || l.Y != target.Y) continue;
                        first = l;
                        break;
                    }

                    if (first != null)
                    {
                        break;
                    }
                }

                var adjacentSquares = GetWalkableAdjacentSquares(current.X, current.Y, map);
                g++;

                foreach (var adjacentSquare in adjacentSquares)
                {
                    // If this adjacent square is already in the closed list, ignore it
                    if (closedList.FirstOrDefault(l => l.X == adjacentSquare.X
                            && l.Y == adjacentSquare.Y) != null)
                        continue;

                    // If it's not in the open list...
                    if (openList.FirstOrDefault(l => l.X == adjacentSquare.X
                            && l.Y == adjacentSquare.Y) == null)
                    {
                        // compute its score, set the parent
                        adjacentSquare.G = g;
                        adjacentSquare.H = ComputeHScore(adjacentSquare.X, adjacentSquare.Y, target.X, target.Y);
                        adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                        adjacentSquare.Parent = current;

                        // and add it to the open list
                        path.Add(adjacentSquare);
                        openList.Insert(0, adjacentSquare);
                    }
                    else
                    {
                        // Test if using the current G score makes the adjacent square's F score lower, if yes update the parent because it means it's a better path
                        if (g + adjacentSquare.H < adjacentSquare.F)
                        {
                            adjacentSquare.G = g;
                            adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                            adjacentSquare.Parent = current;
                        }
                    }
                }
            }

            return neededLocks;
        }

        /// Return a list of valid adjacent rooms (those which can be visited
        /// from the entered coordinate).
        private static List<Location> GetWalkableAdjacentSquares(
            int _x,
            int _y,
            int[,] _map
        ) {
            var proposedLocations = new List<Location>();
            if (_y > 0)
            {
                proposedLocations.Add(new Location { X = _x, Y = _y - 1 });
            }
            if (_y < 2 * sizeY - 1)
            {
                proposedLocations.Add(new Location { X = _x, Y = _y + 1 });
            }
            if (_x > 0)
            {
                proposedLocations.Add(new Location { X = _x - 1, Y = _y });
            }
            if (_x < 2 * sizeX - 1)
            {
                proposedLocations.Add(new Location { X = _x + 1, Y = _y });
            }
            return proposedLocations.Where(
                    l => (_map[l.X,l.Y] >= 0 && _map[l.X,l.Y] != 101)
                ).ToList();
        }

        /// Compute the heuristic score, in this case, a Manhattan Distance.
        private static int ComputeHScore(
            int _x,
            int _y,
            int _targetX,
            int _targetY
        ) {
            return Math.Abs(_targetX - _x) + Math.Abs(_targetY - _y);
        }
    }
}