using System.Collections.Generic;
using Util;

namespace Game.LevelGenerator
{
    /// This class is responsible for create rooms of dungeons.
    public static class RoomFactory
    {
        /// Probability of normal rooms to be created.
        public static readonly float PROB_NORMAL_ROOM = 70f;
        /// Probability of rooms with keys to be created.
        public static readonly float PROB_KEY_ROOM = 15f;
        /// Probability of locked rooms to be created.
        public static readonly float PROB_LOCKER_ROOM = 15f;

        /// List of IDs of the available keys.
        private static List<int> availableKeys = new List<int>();
        /// List of IDs of the used keys.
        private static List<int> usedKeys = new List<int>();

        /// Create and return the root room of a dungeon.
        ///
        /// The root room is a normal room.
        public static Room CreateRoot()
        {
            availableKeys.Clear();
            usedKeys.Clear();
            return new Room();
        }

        /// Create and return a new random room of a dungeon.
        ///
        /// The created room will have one of the following types: a normal
        /// room, a room with a key, or a locked room. Besides, locks and keys
        /// are placed in the dungeon without bound one to the other.
        public static Room CreateRoom() {
            // Probability penalty for levels with exceding number of locks
            float penalty = 0.0f;
            // The more keys without locks higher the chances to create a lock
            if (availableKeys.Count > 0)
            {
                penalty = availableKeys.Count * 0.1f;
            }
            // Create a random room
            Room room = null;
            int prob = RandomSingleton.GetInstance().RandomPercent();
            if (PROB_NORMAL_ROOM - penalty > prob)
            {
                // Create a normal room
                room = new Room();
            }
            else if (PROB_NORMAL_ROOM + PROB_KEY_ROOM - penalty > prob ||
                // A lock can only exist if a room with a key has already been
                // created, else, the lock room is turned into a key room
                availableKeys.Count == 0
            ) {
                // Create a room with a key with room ID
                room = new Room(RoomType.Key);
                availableKeys.Add(room.RoomID);
            }
            else
            {
                // Create a locked room with the key ID that open the lock
                int key = availableKeys[0];
                availableKeys.RemoveAt(0);
                room = new Room(RoomType.Locked, key);
                usedKeys.Add(room.RoomID);
            }
            return room;
        }
    }
}