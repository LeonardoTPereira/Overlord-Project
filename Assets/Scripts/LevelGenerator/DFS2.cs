//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace LevelGenerator
//{
//    class DFS : PathFinding
//    {
//        //Size of the grid
//        static int sizeX;
//        static int sizeY;

//        //The DFS Algorithm
//        public static void FindRoute(Dungeon dun)
//        {
//            List<Location> path = new List<Location>();

//            Location current = null;
//            Location start = null;
//            Location target = null;

//            List<Location> locksLocation = new List<Location>();
//            List<Location> allLocksLocation = new List<Location>();

//            dun.neededLocks = 0;
//            Room actualRoom, parent;
//            RoomGrid grid = dun.roomGrid;

//            Type type;

//            int x, y, iPositive, jPositive;

//            List<int> lockedRooms = new List<int>();
//            List<int> keys = new List<int>();

//            int minX, minY, maxX, maxY;
//            minX = Constants.MATRIXOFFSET;
//            minY = Constants.MATRIXOFFSET;
//            maxX = -Constants.MATRIXOFFSET;
//            maxY = -Constants.MATRIXOFFSET;

//            foreach (Room room in dun.RoomList)
//            {
//                if (room.Type == Type.key)
//                    keys.Add(room.KeyToOpen);
//                if (room.Type == Type.locked)
//                    lockedRooms.Add(room.KeyToOpen);

//                if (room.X < minX)
//                    minX = room.X;
//                if (room.Y < minY)
//                    minY = room.Y;
//                if (room.X > maxX)
//                    maxX = room.X;
//                if (room.Y > maxY)
//                    maxY = room.Y;
//            }

//            //The starting location is room (0,0)
//            start = new Location { X = -2 * minX, Y = -2 * minY };
//            //List of visited rooms that are not closed yet
//            var openList = new List<Location>();
//            //List of closed rooms. They were visited and all neighboors explored.
//            var closedList = new List<Location>();
//            //Size of the new grid
//            sizeX = maxX - minX + 1;
//            sizeY = maxY - minY + 1;
//            int[,] map = new int[2 * sizeX, 2 * sizeY];

//            //101 is EMPTY
//            for (int i = 0; i < 2 * sizeX; ++i)
//            {
//                for (int j = 0; j < 2 * sizeY; ++j)
//                {
//                    map[i, j] = 101;
//                }
//            }
//            //Fill the new grid
//            for (int i = minX; i < maxX + 1; ++i)
//            {
//                for (int j = minY; j < maxY + 1; ++j)
//                {
//                    //Converts the original coordinates (may be negative) to positive
//                    iPositive = i - minX;
//                    jPositive = j - minY;
//                    actualRoom = grid[i, j];
//                    //If the position has a room, check its type and fill the grid accordingly
//                    if (actualRoom != null)
//                    {
//                        type = actualRoom.Type;
//                        //0 is a NORMAL ROOM
//                        if (type == Type.normal)
//                        {
//                            map[iPositive * 2, jPositive * 2] = 0;
//                        }
//                        //The sequential, positivie index of the key is its representation
//                        else if (type == Type.key)
//                        {
//                            map[iPositive * 2, jPositive * 2] = keys.IndexOf(actualRoom.KeyToOpen) + 1;
//                        }
//                        //If the room is locked, the room is a normal room, only the corridor is locked. But is the lock is the last one in the sequential order, than the room is the objective
//                        else if (type == Type.locked)
//                        {
//                            if (lockedRooms.IndexOf(actualRoom.KeyToOpen) == lockedRooms.Count - 1)
//                            {
//                                map[iPositive * 2, jPositive * 2] = 102;
//                                target = new Location { X = iPositive * 2, Y = jPositive * 2 };
//                            }
//                            else
//                                map[iPositive * 2, jPositive * 2] = 0;
//                        }
//                        else
//                        {
//                            Console.WriteLine("Something went wrong printing the tree!\n");
//                            Console.WriteLine("This Room type does not exist!\n\n");
//                        }
//                        parent = actualRoom.Parent;
//                        //If the actual room is a locked room and has a parent, then the connection between then is locked and is represented by the negative value of the index of the key that opens the lock
//                        if (parent != null)
//                        {

//                            x = parent.X - actualRoom.X + 2 * iPositive;
//                            y = parent.Y - actualRoom.Y + 2 * jPositive;
//                            if (type == Type.locked)
//                            {
//                                locksLocation.Add(new Location { X = x, Y = y, Parent = new Location { X = 2 * (parent.X - actualRoom.X) + 2 * iPositive, Y = 2 * (parent.Y - actualRoom.Y) + 2 * jPositive } });
//                                map[x, y] = -(keys.IndexOf(actualRoom.KeyToOpen) + 1);
//                            }
//                            //If the connection is open, 100 represents a normal corridor
//                            else
//                                map[x, y] = 100;
//                        }
//                    }
//                }
//            }
//            //Add all the locks location to the list that will hold their values through the execution of the algorithm
//            foreach (var locked in locksLocation)
//            {
//                allLocksLocation.Add(locked);
//            }

//            openList.Add(start);
//            path.Add(start);

//            while (openList.Count > 0)
//            {
//                // get the first
//                current = openList.First();

//                //teste(map, current, locksLocation, allLocksLocation, openList, closedList, dun);
//                //if the current is a key, change the locked door status in the map
//                if (map[current.X, current.Y] > 0 && map[current.X, current.Y] < 100)
//                {
//                    //If there is still a lock to be open (there may be more keys than locks in the level, so the verification is necessary) find its location and check if the key to open it is the one found
//                    if (locksLocation.Count > 0)
//                    {
//                        foreach (var room in locksLocation)
//                        {
//                            //If the key we found is the one that open the room we are checking now, change the lock to an open corridor and update the algorithm's knowledge
//                            if (map[room.X, room.Y] == -map[current.X, current.Y])
//                            {
//                                map[room.X, room.Y] = 100;
//                                //remove the lock from the unopenned locks location list
//                                locksLocation.Remove(room);
//                                //Check if the parent room of the locked room was already closed by the algorithm (if it was in the closed list)
//                                foreach (var closedRoom in closedList)
//                                {
//                                    //If it was already closed, reopen it. Remove from the closed list and add to the open list
//                                    if (closedRoom.X == room.Parent.X && closedRoom.Y == room.Parent.Y)
//                                    {
//                                        closedList.Remove(closedRoom);
//                                        openList.Add(closedRoom);
//                                        //If the closed room was a locked one, also remove one of the needed locks, as it is now reopen and will be revisited
//                                        foreach (var locked in allLocksLocation)
//                                        {
//                                            if (locked.X == closedRoom.X && locked.Y == closedRoom.Y)
//                                            {
//                                                dun.neededLocks--;
//                                                break;
//                                            }
//                                        }
//                                        break;
//                                    }
//                                }
//                                break;
//                            }
//                        }
//                    }
//                }

//                // add the current square to the closed list
//                closedList.Add(current);
//                //Check if the actual room is a locked one. If it is, add 1 to the number of locks needed to reach the goal
//                foreach (var locked in allLocksLocation)
//                {
//                    if (locked.X == current.X && locked.Y == current.Y)
//                    {
//                        //Console.WriteLine("NEED A LOCK");
//                        dun.neededLocks++;
//                        break;
//                    }
//                }

//                // remove it from the open list
//                openList.Remove(current);

//                // if we added the destination to the closed list, we've found a path
//                if (closedList.FirstOrDefault(l => l.X == target.X && l.Y == target.Y) != null)
//                    break;

//                var adjacentSquares = PathFinding.GetWalkableAdjacentSquares(current.X, current.Y, sizeX, sizeY, map);

//                foreach (var adjacentSquare in adjacentSquares)
//                {
//                    if (current.Parent == adjacentSquare)
//                    {
//                        adjacentSquares.Remove(adjacentSquare);
//                        adjacentSquares.Add(adjacentSquare);
//                        break;
//                    }
//                }

//                foreach (var adjacentSquare in adjacentSquares)
//                {
//                    // if this adjacent square is already in the closed list, ignore it
//                    if (closedList.FirstOrDefault(l => l.X == adjacentSquare.X
//                            && l.Y == adjacentSquare.Y) != null)
//                        continue;

//                    // if it's not in the open list...
//                    if (openList.FirstOrDefault(l => l.X == adjacentSquare.X
//                            && l.Y == adjacentSquare.Y) == null)
//                    {
//                        adjacentSquare.Parent = current;

//                        // and add it to the open list and add to your path
//                        openList.Insert(0, adjacentSquare);
//                        path.Add(adjacentSquare);
//                    }
//                    else
//                    {
//                        adjacentSquare.Parent = current;
//                    }
//                }
//            }
//            // end

//            Interface.PrintPathFound(current);
//            Interface.PrintPathFinding(path);
//        }
//    }

//}
