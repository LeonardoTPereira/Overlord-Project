using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    /// This class holds the crossover operator.
    public static class Crossover
    {
        /// Choose a random room to switch between the parents and arrange
        /// every aspect of the room needed after the change. Including the
        /// grid, and also the exceptions where the new nodes overlap the old
        /// ones.
        public static Individual[] Apply(Individual parent1, Individual parent2) {
            // Initialize the two new individuals
            Individual[] individuals = new Individual[0];
            Dungeon dungeon1;
            Dungeon dungeon2;

            // Cut points
            Room roomCut1;
            Room roomCut2;
            // Tabu List: list of rooms that were the root of the branch and
            // led to an impossible crossover
            // List of mission rooms (rooms with keys and locked doors) in the
            // branch to be traded of each parent
            List<int> missions1;
            List<int> missions2;
            // List of mission rooms (rooms with keys and locked doors) in the
            // traded branch after the crossover (i.e., in the new individuals)
            List<int> newMissions1 = new List<int>();
            List<int> newMissions2 = new List<int>();
            // Total number of rooms in each branch that will be traded
            int nRooms1;
            int nRooms2;
            // If the trade is possible or not
            bool isImpossible;
            var enemiesPerRoomCut1 = new Queue<int>();
            var enemiesPerRoomCut2 = new Queue<int>();
            do
            {
                dungeon1 = new Dungeon(parent1.dungeon);
                dungeon2 = new Dungeon(parent2.dungeon);
                // Get a random node from the parent as the root of the branch
                // that will be traded
                int index = RandomSingleton.GetInstance().Next(1, dungeon1.Rooms.Count);
                roomCut1 = dungeon1.Rooms[index];
                // Calculate the number of keys, locks and rooms in the branch
                // of the cut point in dungeon 1
                missions1 = new List<int>();
                nRooms1 = CalculateBranchRooms(missions1, roomCut1);
                var failedRooms = new List<Room>();

                // While the number of keys and locks from a branch is greater
                // than the number of rooms of the other branch, redraw the cut
                // point (the root of the branch)
                do
                {
                    roomCut2 = GetRoomCutPoint(dungeon2, failedRooms);
                    // Add the cut room in the list of failed rooms
                    failedRooms.Add(roomCut2);
                    // If no room can be the cut point, then the crossover
                    // operation is impossible
                    isImpossible = CheckIfCrossoverIsImpossible(failedRooms, dungeon2);
                    // Calculate the number of keys, locks and rooms in the
                    // branch of the cut point in dungeon 2
                    missions2 = new List<int>();
                    nRooms2 = CalculateBranchRooms(missions2, roomCut2);
                } while ((missions2.Count > nRooms1 ||
                          missions1.Count > nRooms2) &&
                          !isImpossible);

                // If the crossover is possible, then perform it
                if (isImpossible) continue;
                enemiesPerRoomCut1 = roomCut1.GetEnemiesPerRoom();
                enemiesPerRoomCut2 = roomCut2.GetEnemiesPerRoom();
                // Swap the branchs
                SwapBranch(ref roomCut1, ref roomCut2);
                SwapBranch(ref roomCut2, ref roomCut1);

                // Change the parent of each node
                (roomCut1.Parent, roomCut2.Parent) = (roomCut2.Parent, roomCut1.Parent);

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
                nRooms2 = CalculateBranchRooms(newMissions2, roomCut2);
                nRooms1 = CalculateBranchRooms(newMissions1, roomCut1);
                // If mission rooms are missing in the new branchs or the number
            // of rooms in the branchs is greater than the number of total
            // rooms, retry apply the crossover
            } while ((newMissions1.Count != missions1.Count ||
                      newMissions2.Count != missions2.Count ||
                      missions1.Count > nRooms2 ||
                      missions2.Count > nRooms1) &&
                      !isImpossible);

            if (isImpossible) return individuals;
            roomCut2.FixBranch(missions1);
            roomCut1.FixBranch(missions2);
            roomCut2.FixEnemies(enemiesPerRoomCut2);
            roomCut1.FixEnemies(enemiesPerRoomCut1);
            individuals = new Individual[2];
            individuals[0] = new Individual(parent1);
            individuals[1] = new Individual(parent2);
            var enemies = individuals[0].dungeon.GetNumberOfEnemies();
            if (enemies != parent1.Fitness.DesiredInput.DesiredEnemies)
            {
                Debug.LogError($"Requested {parent1.Fitness.DesiredInput.DesiredEnemies} Enemies, found {enemies}");
            }
            enemies = individuals[1].dungeon.GetNumberOfEnemies();
            if (enemies != parent1.Fitness.DesiredInput.DesiredEnemies)
            {
                Debug.LogError($"Requested {parent1.Fitness.DesiredInput.DesiredEnemies} Enemies, found {enemies}");
            }
            return individuals;
        }

        private static bool CheckIfCrossoverIsImpossible(List<Room> failedRooms, Dungeon dungeon2)
        {
            return failedRooms.Count == dungeon2.Rooms.Count - 1;
        }

        private static Room GetRoomCutPoint(Dungeon dungeon2, List<Room> failedRooms)
        {
            Room roomCut2;
            do
            {
                var index = RandomSingleton.GetInstance().Next(1, dungeon2.Rooms.Count);
                roomCut2 = dungeon2.Rooms[index];
            } while (failedRooms.Contains(roomCut2));

            return roomCut2;
        }

        /// Find the number of rooms of a branch and the mission rooms (rooms
        /// with keys and locked doors) in the branch. The key rooms are saved
        /// in the list with its positive ID, while the locked rooms with its
        /// negative value of the ID.
        private static int CalculateBranchRooms(List<int> missions, Room root) {
            var rooms = 0;
            // Search for the mission rooms in the dungeon
            Queue<Room> toVisit = new Queue<Room>();
            toVisit.Enqueue(root);
            while (toVisit.Count > 0)
            {
                rooms++;
                Room current = toVisit.Dequeue();
                switch (current.Type)
                {
                    case RoomType.Key:
                        AddKeyRoom(missions, current);
                        break;
                    case RoomType.Locked:
                        missions.Add(-current.Key);
                        break;
                }
                current.EnqueueChildrenRooms(toVisit);
            }
            return rooms;
        }

        private static void AddKeyRoom(List<int> missions, Room current)
        {
            if (missions.Count > 0)
            {
                int lockIndex = missions.IndexOf(-current.Key);
                if (lockIndex != -1)
                {
                    missions.Insert(lockIndex, current.Key);
                }
                else
                {
                    missions.Add(current.Key);
                }
            }
            else
            {
                missions.Add(current.Key);
            }
        }

        /// Swap the entered rooms by assigning `_room2` as the child of the
        /// parent of `_room1` in the respective direction.
        private static void SwapBranch( ref Room room1,  ref Room room2)
        {
            // Set `_room2` as a child of the parent of `_room1`
            if (room1 == null) return;
            switch (room1.ParentDirection)
            {
                case Common.Direction.Left:
                    room1.Parent.Left = room2;
                    break;
                case Common.Direction.Down:
                    room1.Parent.Bottom = room2;
                    break;
                case Common.Direction.Right:
                    room1.Parent.Right = room2;
                    break;
            }
        }
    }
}