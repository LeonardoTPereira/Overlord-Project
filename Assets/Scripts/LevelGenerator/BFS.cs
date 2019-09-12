using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelGenerator
{
    class BFS : PathFinding
    {
        public BFS(Dungeon dun)
            : base(dun)
        {
        }

        //The DFS Algorithm
        public void FindRoute(Dungeon dun)
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
                //Check if the actual room is a locked one. If it is, add 1 to the number of locks needed to reach the goal
                foreach (var locked in allLocksLocation)
                {
                    if (locked.X == current.X && locked.Y == current.Y)
                    {
                        //Console.WriteLine("NEED A LOCK");
                        dun.neededLocks++;
                        break;
                    }
                }

                // remove it from the open list
                openList.Remove(current);

                // if we added the destination to the closed list, we've found a path
                if (ClosedList.FirstOrDefault(l => l.X == target.X && l.Y == target.Y) != null)
                    break;

                var adjacentSquares = GetWalkableAdjacentSquares(current.X, current.Y, sizeX, sizeY, map);

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
                        openList.Add(adjacentSquare);
                        path.Add(adjacentSquare);
                    }
                    else
                    {
                        adjacentSquare.Parent = current;
                    }
                }
            }

            // PrintPathFound(current, 200);
            PrintPathFinding(path, 200);
        }
    }

}
