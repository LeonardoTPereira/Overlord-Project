using System;
using System.Collections.Generic;

namespace LevelGenerator
{
    public class RoomFactory
    {
        //List of currently available keys
        private static List<int> availableLockId = new List<int>();
        private static List<int> usedLockId = new List<int>();
        
        public static List<int> AvailableLockId { get => availableLockId; set => availableLockId = value; }
        public static List<int> UsedLockId { get => usedLockId; set => usedLockId = value; }
        
        /*
         * Creates the root room of the dungeon
         */
        public static Room CreateRoot()
        {
            availableLockId.Clear();
            usedLockId.Clear();
            //The root room is always a normal one
            Room root = new Room();

            return root;
        }
        /*
         *  Creates a Room for the dungeon, defining if it is a normal room, a room with a key, or a locked room 
         */
        public static Room CreateRoom()
        {
            float nodeProbPenalty = 0.0f;
            int keyToOpen;
            //int lockId;
            Room room;
            
            int prob = Util.rnd.Next(101);
            //If there are too many keys without locks, raises the chance to create a lock
            if (AvailableLockId.Count > 0)
            {
                nodeProbPenalty = (AvailableLockId.Count) * 0.1f;
            }
            if (prob < (Constants.PROB_NORMAL_ROOM - nodeProbPenalty))
            {
                room = new Room();
            }
            else if (prob < (Constants.PROB_NORMAL_ROOM + Constants.PROB_KEY_ROOM - nodeProbPenalty))
            {
                /*
                 * If the room has a key, then the key must have an id and this id is added to the list of available keys
                 */
                room = new Room(Type.key);
                AvailableLockId.Add(room.RoomId);
                //Console.WriteLine("KeyId:" + room.RoomId);
            }
            else
            {
                /*
                 * A lock can only exist if a room with a key has already been created, else, the lock room is turned into a key room
                 */
                if (AvailableLockId.Count == 0)
                {
                    //lockId = parentLockID;
                    //Creates a room containing a key
                    room = new Room(Type.key);
                    AvailableLockId.Add(room.RoomId);

                    //Console.WriteLine("KeyId:" + room.RoomId);
                }
                else
                {
                    //LockId = availableLockId[0];
                    //lockId = (int)Math.Pow(2, usedLockId.Count) + parentLockID;
                    keyToOpen = AvailableLockId[0];
                    AvailableLockId.RemoveAt(0);
                    //Creates a locked room with the id of the room that contains the key to open it
                    room = new Room(Type.locked, keyToOpen);
                    UsedLockId.Add(room.RoomId);

                    //Console.WriteLine("LockId:" + keyToOpen);
                }
            }
            return room;
        }
        /*
         * Recreates a room, used after a crossover to fix the level
         * Only redefines the types of the room in order to make the new level a feasible one even with the changes
         */
        public static void RecreateRoom(ref Room room, int desiredKeys)
        {
            float nodeProbPenalty = 0.0f;
            float desiredPenalty = 0.0f;
            int keyToOpen;

            int prob = Util.rnd.Next(101);
            desiredPenalty = (desiredKeys - availableLockId.Count + usedLockId.Count)*0.1f;
            //If there are too many keys without locks, raises the chance to create a lock
            if (AvailableLockId.Count > 0)
            {
                nodeProbPenalty = (AvailableLockId.Count) * 0.1f;
            }
            if (prob < (Constants.PROB_NORMAL_ROOM - nodeProbPenalty - desiredPenalty))
            {
                room.Type = Type.normal;
                room.KeyToOpen = -1;
            }
            else if (prob < (Constants.PROB_NORMAL_ROOM + Constants.PROB_KEY_ROOM - nodeProbPenalty - desiredPenalty))
            {
                /*
                 * If the room has a key, then the key must have an id and this id is added to the list of available keys
                 */
                room.Type = Type.key;
                room.RoomId = Util.getNextId();
                room.KeyToOpen = room.RoomId;
                AvailableLockId.Add(room.RoomId);
            }
            else
            {
                /*
                 * A lock can only exist if a room with a key has already been created, else, the lock room is turned into a key room
                 */
                if (AvailableLockId.Count == 0)
                {
                    //lockId = parentLockID;
                    //Creates a room containing a key
                    room.Type = Type.key;
                    room.KeyToOpen = room.RoomId;
                    AvailableLockId.Add(room.RoomId);
                }
                else
                {
                    //LockId = availableLockId[0];
                    //lockId = (int)Math.Pow(2, usedLockId.Count) + parentLockID;
                    keyToOpen = AvailableLockId[0];
                    AvailableLockId.RemoveAt(0);
                    //Creates a locked room with the id of the room that contains the key to open it
                    room.Type = Type.locked;
                    room.KeyToOpen = keyToOpen;
                    UsedLockId.Add(room.RoomId);
                }
            }
        }
    }
}