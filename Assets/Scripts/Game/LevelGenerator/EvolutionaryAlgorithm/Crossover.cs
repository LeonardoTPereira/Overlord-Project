using System;
using System.Collections.Generic;
using System.Diagnostics;
using Game.LevelGenerator.EvolutionaryAlgorithm;
using Util;

namespace Game.LevelGenerator
{
    /// This class holds the crossover operator.
    public static class Crossover
    {
        /// Choose a random room to switch between the parents and arrange
        /// every aspect of the room needed after the change. Including the
        /// grid, and also the exceptions where the new nodes overlap the old
        /// ones.
        public static Individual[] Apply(Individual _parent1, Individual _parent2) {
            // Initialize the two new individuals
            Individual[] individuals = new Individual[0];
            Dungeon dungeon1;
            Dungeon dungeon2;

            // Cut points
            Room roomCut1;
            Room roomCut2;
            // Tabu List: list of rooms that were the root of the branch and
            // led to an impossible crossover
            List<Room> failedRooms;
            // List of mission rooms (rooms with keys and locked doors) in the
            // branch to be traded of each parent
            List<int> missions1 = new List<int>();
            List<int> missions2 = new List<int>();
            // List of mission rooms (rooms with keys and locked doors) in the
            // traded branch after the crossover (i.e., in the new individuals)
            List<int> newMissions1 = new List<int>();
            List<int> newMissions2 = new List<int>();
            // Total number of rooms in each branch that will be traded
            int nRooms1 = 0;
            int nRooms2 = 0;
            // If the trade is possible or not
            bool isImpossible = false;

            do
            {
                dungeon1 = _parent1.dungeon.Clone();
                dungeon2 = _parent2.dungeon.Clone();
                // Get a random node from the parent as the root of the branch
                // that will be traded
                int index = RandomSingleton.GetInstance().Next(1, dungeon1.Rooms.Count);
                roomCut1 = dungeon1.Rooms[index];
                // Calculate the number of keys, locks and rooms in the branch
                // of the cut point in dungeon 1
                CalculateBranchRooms(ref nRooms1, ref missions1, roomCut1);
                failedRooms = new List<Room>();

                // While the number of keys and locks from a branch is greater
                // than the number of rooms of the other branch, redraw the cut
                // point (the root of the branch)
                do
                {
                    do
                    {
                        index = RandomSingleton.GetInstance().Next(1, dungeon2.Rooms.Count);
                        roomCut2 = dungeon2.Rooms[index];
                    } while (failedRooms.Contains(roomCut2));
                    // Add the cut room in the list of failed rooms
                    failedRooms.Add(roomCut2);
                    // If no room can be the cut point, then the crossover
                    // operation is impossible
                    if (failedRooms.Count == dungeon2.Rooms.Count - 1)
                    {
                        isImpossible = true;
                    }
                    // Calculate the number of keys, locks and rooms in the
                    // branch of the cut point in dungeon 2
                    CalculateBranchRooms(ref nRooms2, ref missions2, roomCut2);
                } while ((missions2.Count > nRooms1 ||
                          missions1.Count > nRooms2) &&
                          !isImpossible);

                // If the crossover is possible, then perform it
                if (!isImpossible)
                {
                    // Swap the branchs
                    SwapBranch(ref roomCut1, ref roomCut2);
                    SwapBranch(ref roomCut2, ref roomCut1);

                    // Change the parent of each node
                    Room auxRoom = roomCut1.Parent;
                    roomCut1.Parent = roomCut2.Parent;
                    roomCut2.Parent = auxRoom;

                    // Remove the original nodes from the old level grid
                    dungeon1.RemoveFromGrid(roomCut1);
                    dungeon2.RemoveFromGrid(roomCut2);

                    // Swap the nodes attributes
                    int x = roomCut1.X;
                    int y = roomCut1.Y;
                    Common.Direction dir = roomCut1.ParentDirection;
                    int rotation = roomCut1.Rotation;
                    roomCut1.X = roomCut2.X;
                    roomCut1.Y = roomCut2.Y;
                    roomCut1.ParentDirection = roomCut2.ParentDirection;
                    roomCut1.Rotation = roomCut2.Rotation;
                    roomCut2.X = x;
                    roomCut2.Y = y;
                    roomCut2.ParentDirection = dir;
                    roomCut2.Rotation = rotation;

                    // Update the grid of the two new dungeons
                    // If any conflicts arise here, they will be handled in the
                    // creation of child nodes; that is, any overlap will make
                    // the node and its children cease to exist
                    dungeon1.RefreshGrid(ref roomCut2);
                    dungeon2.RefreshGrid(ref roomCut1);

                    // Calculate the number of keys, locks and rooms in the
                    // newly switched branchs
                    newMissions1 = new List<int>();
                    newMissions2 = new List<int>();
                    CalculateBranchRooms(
                        ref nRooms2, ref newMissions2, roomCut2
                    );
                    CalculateBranchRooms(
                        ref nRooms1, ref newMissions1, roomCut1
                    );
                }
            // If mission rooms are missing in the new branchs or the number
            // of rooms in the branchs is greater than the number of total
            // rooms, retry apply the crossover
            } while ((newMissions1.Count != missions1.Count ||
                      newMissions2.Count != missions2.Count ||
                      missions1.Count > nRooms2 ||
                      missions2.Count > nRooms1) &&
                      !isImpossible);

            if (!isImpossible)
            {
                roomCut2.FixBranch(missions1);
                roomCut1.FixBranch(missions2);
                individuals = new Individual[2];
                individuals[0] = new Individual(_parent1.Fitness.DesiredParameters);
                individuals[1] = new Individual(_parent1.Fitness.DesiredParameters);
                individuals[0].dungeon = dungeon1;
                individuals[1].dungeon = dungeon2;
            }

            return individuals;
        }

        /// Find the number of rooms of a branch and the mission rooms (rooms
        /// with keys and locked doors) in the branch. The key rooms are saved
        /// in the list with its positive ID, while the locked rooms with its
        /// negative value of the ID.
        private static void CalculateBranchRooms(
            ref int _rooms,
            ref List<int> _missions,
            Room _root
        ) {
            // Reset the list of mission rooms and the number of rooms
            _missions = new List<int>();
            _rooms = 0;
            // Search for the mission rooms in the dungeon
            Queue<Room> toVisit = new Queue<Room>();
            toVisit.Enqueue(_root);
            while (toVisit.Count > 0)
            {
                _rooms++;
                Room current = toVisit.Dequeue();
                if (current.Type1 == RoomType.Key)
                {
                    if(_missions.Count > 0)
                    {
                        int lockIndex = _missions.IndexOf(-current.Key);
                        if (lockIndex != -1)
                        {
                            _missions.Insert(lockIndex, current.Key);
                        }
                        else
                        {
                            _missions.Add(current.Key);
                        }
                    }
                    else
                    {
                        _missions.Add(current.Key);
                    }
                }
                else if (current.Type1 == RoomType.Locked)
                {
                    _missions.Add(-current.Key);
                }
                foreach (Room room in current.GetChildren())
                {
                    if (room != null)
                    {
                        toVisit.Enqueue(room);
                    }
                }
            }
        }

        /// Swap the entered rooms by assigning `_room2` as the child of the
        /// parent of `_room1` in the respective direction.
        private static void SwapBranch(
            ref Room _room1,
            ref Room _room2
        )
        {
            // No room involved in this operation can be null
            Debug.Assert(
                _room1 != null && _room2 != null && _room1.Parent != null,
                Common.PROBLEM_IN_THE_DUNGEON
            );
            // Set `_room2` as a child of the parent of `_room1`
            if (_room1 == null) return;
            switch (_room1.ParentDirection)
            {
                case Common.Direction.Left:
                    _room1.Parent.Left = _room2;
                    break;
                case Common.Direction.Down:
                    _room1.Parent.Bottom = _room2;
                    break;
                case Common.Direction.Right:
                    _room1.Parent.Right = _room2;
                    break;
            }
        }
    }
}