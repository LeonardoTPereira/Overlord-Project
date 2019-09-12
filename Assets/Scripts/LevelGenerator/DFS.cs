using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelGenerator
{
    class DFS : PathFinding
    {
        
        public DFS(Dungeon dun)
            : base(dun)
        {
        }

        //The DFS Algorithm
        public int FindRoute()
        {
            openList.Add(start);
            path.Add(start);
            while (openList.Count > 0)
            {
                // get the first
                current = openList.First();

                validateKeyRoom(current);

                // add the current square to the closed list
                ClosedList.Add(current);
                if (((map[current.X, current.Y] >= 0) && (map[current.X, current.Y] < 100)) || (map[current.X, current.Y] == 102))
                {
                    //Console.SetCursorPosition(0, 15+auxoffset);
                    //auxoffset += 1;
                    NVisitedRooms++;
                    //Console.WriteLine("NVisitedRooms:" + NVisitedRooms);
                }
                //Check if the actual room is a locked one. If it is, add 1 to the number of locks needed to reach the goal
                foreach (var locked in allLocksLocation)
                {
                    if (locked.X == current.X && locked.Y == current.Y)
                    {
                        //Console.WriteLine("NEED A LOCK");
                        NeededLocks++;
                        break;
                    }
                }
                // show current square on the map
                /*Console.SetCursorPosition(60+current.Y, current.X);
                Console.Write('.');
                Console.SetCursorPosition(60+current.Y, current.X);
                System.Threading.Thread.Sleep(2);*/

                // remove it from the open list
                openList.Remove(current);

                // if we added the destination to the closed list, we've found a path
                if (ClosedList.Count > 0)
                    if(ClosedList.FirstOrDefault(l => l.X == target.X && l.Y == target.Y) != null)
                        break;

                var adjacentSquares = GetWalkableAdjacentSquares(current.X, current.Y, sizeX, sizeY, map);

                Random rand = new Random();

                adjacentSquares = adjacentSquares.OrderBy(X => rand.Next()).ToList();
 
                foreach (var adjacentSquare in adjacentSquares)
                {
                    if (current.Parent == adjacentSquare)
                    {
                        adjacentSquares.Remove(adjacentSquare);
                        adjacentSquares.Add(adjacentSquare);
                        break;
                    }
                }

                foreach (var adjacentSquare in adjacentSquares)
                {
                    // if this adjacent square is already in the closed list, ignore it
                    if (ClosedList.FirstOrDefault(l => l.X == adjacentSquare.X
                            && l.Y == adjacentSquare.Y) != null)
                        continue;

                    // if it's not in the open list...
                    if (openList.FirstOrDefault(l => l.X == adjacentSquare.X
                            && l.Y == adjacentSquare.Y) == null)
                    {
                        adjacentSquare.Parent = current;

                        // and add it to the open list and add to your path
                        openList.Insert(0, adjacentSquare);
                        path.Add(adjacentSquare);
                    }
                    else
                    {
                        adjacentSquare.Parent = current;
                    }
                }
            }

            /*while (current != null)
            {
                Console.SetCursorPosition(60+current.Y + 20, current.X);
                Console.Write('_');
                Console.SetCursorPosition(60+current.Y + 20, current.X);
                current = current.Parent;
                System.Threading.Thread.Sleep(2);
            }*/

            // PrintPathFound(current);
            //PrintPathFinding(path, 200);
            return NeededLocks;
        }
    }

}
