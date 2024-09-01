using System.Collections.Generic;
using Game.LevelGenerator.EvolutionaryAlgorithm;

namespace Game.LevelGenerator
{
    /// This class holds the dungeon levels measurement-related functions.
    class Metric
    {
        /// Calculate and return the leniency.
        public static float Leniency( Individual _individual ) 
        {
            Dungeon dungeon = _individual.dungeon;
            Queue<Room> unvisited = new Queue<Room>();
            unvisited.Enqueue(dungeon.GetStart());
            // Calculate the number of safe rooms
            int safe = 0;
            while (unvisited.Count > 0)
            {
                Room current = unvisited.Dequeue();
                if (current.Enemies == 0) { safe++; }
                foreach (Room neighbor in current.GetChildren())
                {
                    if (neighbor != null)
                    {
                        unvisited.Enqueue(neighbor);
                    }
                }
            }
            // Calculate and return the dungeon leniency
            return (float)safe / (float)dungeon.Rooms.Count;
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
            Room[] keys = new Room[dungeon.KeyIds.Count];
            Room[] locks = new Room[dungeon.KeyIds.Count];
            foreach (Room room in dungeon.Rooms)
            {
                // Place the key
                if (room.Type == RoomType.Key)
                {
                    int ki = dungeon.KeyIds.IndexOf(room.Key);
                    if (ki != -1)
                    {
                        keys[ki] = room;
                    }
                }
                // Place the lock at the same index as its the key
                if (room.Type == RoomType.Locked)
                {
                    int li = dungeon.KeyIds.IndexOf(room.Key);
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