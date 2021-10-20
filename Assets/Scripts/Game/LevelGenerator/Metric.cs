using System;
using System.Collections.Generic;

namespace LevelGenerator
{
    /// This class holds the dungeon levels measurement-related functions.
    class Metric
    {
        /// Calculate and return the leniency.
        public static float Leniency(
            Individual _individual
        ) {
            Dungeon dungeon = _individual.dungeon;
            Queue<Room> unvisited = new Queue<Room>();
            unvisited.Enqueue(dungeon.rooms[0]);
            // Calculate the number of safe rooms
            int safe = 0;
            while (unvisited.Count > 0)
            {
                Room current = unvisited.Dequeue();
                if (current.enemies == 0) { safe++; }
                foreach (Room neighbor in current.GetChildren())
                {
                    if (neighbor != null)
                    {
                        unvisited.Enqueue(neighbor);
                    }
                }
            }
            // Calculate and return the dungeon leniency
            return (float) safe / dungeon.rooms.Count;
        }

        /// Calculate and return the coefficient of exploration.
        public static float CoefficientOfExploration(
            Individual _individual
        ) {
            // Get the coefficient of exploration between start and goal rooms
            Dungeon dungeon = _individual.dungeon;
            Room start = dungeon.GetStart();
            Room target = dungeon.GetGoal();
            List<Room> reached = FloodFill(dungeon, start, target);
            float sum = (float) reached.Count / dungeon.GetNumberOfRooms();
            // Get the all the rooms with keys and locks
            Room[] keys = new Room[dungeon.keyIds.Count];
            Room[] locks = new Room[dungeon.keyIds.Count];
            foreach (Room room in dungeon.rooms)
            {
                // Place the key
                if (room.type == RoomType.Key)
                {
                    int ki = dungeon.keyIds.IndexOf(room.key);
                    if (ki != -1)
                    {
                        keys[ki] = room;
                    }
                }
                // Place the lock at the same index as its the key
                if (room.type == RoomType.Locked)
                {
                    int li = dungeon.keyIds.IndexOf(room.key);
                    if (li != -1)
                    {
                        locks[li] = room;
                    }
                }
            }
            // Get the coefficient of exploration between keys and locks and
            // calculate the mean of all the coefficient of exploration
            int count = 1;
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i] != null && locks[i] != null)
                {
                    List<Room> r = FloodFill(dungeon, keys[i], locks[i]);
                    sum += (float) r.Count / dungeon.GetNumberOfRooms();
                    count++;
                }
            }
            return sum / count;
        }

        /// Return all the reached room in the search for the target room.
        ///
        /// To do so, this method performs the flood-fill approach to search
        /// from the starting room for the target room.
        public static List<Room> FloodFill(
            Dungeon _dungeon,
            Room _start,
            Room _target
        ) {
            List<Room> reached = new List<Room>();
            Queue<Room> unvisited = new Queue<Room>();
            unvisited.Enqueue(_start);
            while (unvisited.Count > 0)
            {
                Room current = unvisited.Dequeue();
                if (reached.Contains(current)) { continue; }
                reached.Add(current);
                if (current.Equals(_target) || _target == null) { break; }
                foreach (Room neighbor in current.GetNeighbors())
                {
                    if (neighbor != null)
                    {
                        unvisited.Enqueue(neighbor);
                    }
                }
            }
            return reached;
        }
    }
}