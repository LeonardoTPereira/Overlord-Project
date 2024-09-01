using System.Linq;
using Util;

namespace Game.LevelGenerator
{
    class DFS : PathFinding
    {
        public DFS(Dungeon _dungeon)
            : base(_dungeon) {}

        /// The DFS Algorithm.
        public int FindRoute() {
            openList.Add(start);
            path.Add(start);
            while (openList.Count > 0)
            {
                // Get the first
                Location current = openList.First();

                ValidateKeyRoom(current);

                // Add the current square to the closed list
                ClosedList.Add(current);
                if (((map[current.X, current.Y] >= 0) && (map[current.X, current.Y] < 100)) || (map[current.X, current.Y] == 102))
                {
                    NVisitedRooms++;
                }
                // Check if the actual room is a locked one. If it is, add 1 to the number of locks needed to reach the goal
                foreach (var locked in allLocksLocation)
                {
                    if (locked.X == current.X && locked.Y == current.Y)
                    {
                        NeededLocks++;
                        break;
                    }
                }
                // Remove it from the open list
                openList.Remove(current);
                // If we added the destination to the closed list, we've found a path
                if (ClosedList.Count > 0 && ClosedList.FirstOrDefault(l => l.X == target.X && l.Y == target.Y) != null)
                {
                    break;
                }

                // Check all adjacent squares from the current node
                var adjacentSquares = GetWalkableAdjacentSquares(current.X, current.Y, sizeX, sizeY, map);

                int value = RandomSingleton.GetInstance().Random.Next();
                adjacentSquares = adjacentSquares.OrderBy(X => value).ToList();

                foreach (var adjacentSquare in adjacentSquares)
                {
                    if (current.Parent == adjacentSquare)
                    {
                        adjacentSquares.Remove(adjacentSquare);
                        break;
                    }
                }

                foreach (var adjacentSquare in adjacentSquares)
                {
                    // If this adjacent square is already in the closed list, ignore it
                    if (ClosedList.FirstOrDefault(l => l.X == adjacentSquare.X
                            && l.Y == adjacentSquare.Y) != null)
                    {
                        continue;
                    }

                    // If it's not in the open list...
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

            return NeededLocks;
        }
    }

}
