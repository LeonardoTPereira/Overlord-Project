using System.Collections.Generic;
using System.Linq;
using Game.LevelManager;
using MyBox;
using UnityEngine;
using Util;

namespace Game.LevelGenerator
{
    public class Dungeon
    {
        private const float InitialProbability = 1.0f;

        /// The probability of generating a new room as a child.
        public static readonly float PROB_HAS_CHILD = 100f;
        /// The probability of generating a single child.
        public static readonly float PROB_CHILD = 100f / 3;
        /// The max depth of a dungeon tree.
        public static readonly int MAX_DEPTH = 9;

        /// Room Grid, where a reference to all the existing room will be maintained for quick access when creating nodes.
        public RoomGrid DungeonGrid { get; set; }

        /// The list of dungeon key IDs.
        public List<int> KeyIds { get; set; }

        /// The list of locked room IDs.
        public List<int> LockIds { get; set; }

        /// The lower limit of the x-axis of the grid.
        public int MinX { get; set; }

        /// The lower limit of the y-axis of the grid.
        public int MinY { get; set; }

        /// The upper limit of the x-axis of the grid.
        public int MaxX { get; set; }

        /// The upper limit of the y-axis of the grid.
        public int MaxY { get; set; }

        /// The list of rooms (easier to add neighbors).
        public List<Room> Rooms { get; set; }

        public Boundaries DungeonBoundaries { get; set; }
        public Dimensions DungeonDimensions { get; set; }

        /// Dungeon constructor.
        ///
        /// Create and return a new dungeon with the starting room.
        public Dungeon()
        {
            Rooms = new List<Room>();
            var root = RoomFactory.CreateRoot();
            Rooms.Add(root);
            DungeonGrid = new RoomGrid();
            DungeonGrid[root.X, root.Y] = root;
            LockIds = new List<int>();
            KeyIds = new List<int>();
            MinX = RoomGrid.LEVEL_GRID_OFFSET;
            MinY = RoomGrid.LEVEL_GRID_OFFSET;
            MaxX = -RoomGrid.LEVEL_GRID_OFFSET;
            MaxY = -RoomGrid.LEVEL_GRID_OFFSET;
        }

        /// Return a clone this dungeon.
        public Dungeon Clone()
        {
            var dungeon = new Dungeon();
            dungeon.Rooms = new List<Room>();
            dungeon.DungeonGrid = new RoomGrid();
            foreach (var old in Rooms)
            {
                var aux = old.Clone();
                dungeon.Rooms.Add(aux);
                dungeon.DungeonGrid[old.X, old.Y] = aux;
            }
            dungeon.KeyIds = new List<int>(KeyIds);
            dungeon.LockIds = new List<int>(LockIds);
            dungeon.MinX = MinX;
            dungeon.MinY = MinY;
            dungeon.MaxX = MaxX;
            dungeon.MaxY = MaxY;
            // Need to use the grid to copy the neighboors, children and parent
            // Check the position of the node in the grid and then substitute the old room with the copied one
            foreach (var room in dungeon.Rooms)
            {
                if (room.Parent != null)
                {
                    room.Parent = dungeon.DungeonGrid[room.Parent.X, room.Parent.Y];
                }
                if (room.Right != null)
                {
                    room.Right = dungeon.DungeonGrid[room.Right.X, room.Right.Y];
                }
                if (room.Bottom != null)
                {
                    room.Bottom = dungeon.DungeonGrid[room.Bottom.X, room.Bottom.Y];
                }
                if (room.Left != null)
                {
                    room.Left = dungeon.DungeonGrid[room.Left.X, room.Left.Y];
                }
            }
            return dungeon;
        }

        /// Return the number of rooms of the dungeon.
        public int GetNumberOfRooms()
        {
            return Rooms.Count;
        }

        /// Return the number of enemies of the dungeon.
        public int GetNumberOfEnemies()
        {
            var sum = 0;
            foreach (var room in Rooms)
            {
                sum += room.Enemies;
            }
            return sum;
        }

        /// Return the dungeon start room.
        public Room GetStart()
        {
            return Rooms[0];
        }

        /// Return the dungeon goal room.
        public Room GetGoal()
        {
            Room goal = null;
            foreach (var room in Rooms)
            {
                if (room.Type1 != RoomType.Locked) { continue; }
                var _lock = LockIds.IndexOf(room.Key);
                if (_lock == LockIds.Count - 1)
                {
                    goal = room;
                }
            }
            return goal;
        }

        /// Instantiate a room and tries to add it as a child of the actual
        /// room, considering its direction and position. If there is not a
        /// room in the grid at the entered coordinates, create the room, add
        /// it to the room list and also enqueue it so it can be explored later.
        private void InstantiateRoom(
            ref Room child,
            ref Room current,
            Common.Direction dir)
        {
            var dungeonGrid = DungeonGrid;
            if (current.ValidateChild(dir, dungeonGrid))
            {
                child = RoomFactory.CreateRoom();
                current.InsertChild(ref dungeonGrid, ref child, dir);
                child.ParentDirection = dir;
                Rooms.Add(child);
                dungeonGrid[child.X, child.Y] = child;
            }
        }

        /// While a node (room) has not been visited, or while the max depth of the tree has not been reached, visit each node and create its children.
        public void GenerateRooms() {
            var toVisit = new Queue<Room>();
            toVisit.Enqueue(Rooms[0]);
            while (toVisit.Count > 0)
            {
                var current = toVisit.Dequeue();
                var currentDepth = current.Depth;
                //If max depth allowed has been reached, stop creating children
                if (currentDepth > MAX_DEPTH)
                {
                    toVisit.Clear();
                    break;
                }
                //Check how many children the node will have, if any.
                float prob = RandomSingleton.GetInstance().RandomPercent();
                var probabilityOfHavingChildren = ProbHasChild(currentDepth);
                if (prob <= probabilityOfHavingChildren)
                {
                    Room child = null;
                    var dir = RandomSingleton.GetInstance().RandomElementFromArray(Common.AllDirections());
                    prob = RandomSingleton.GetInstance().RandomPercent();

                    if (prob < PROB_CHILD)
                    {
                        InstantiateRoom(ref child, ref current, dir);
                    }
                    else if (prob < PROB_CHILD * 2)
                    {
                        InstantiateRoom(ref child, ref current, dir);
                        Common.Direction dir2;
                        do
                        {
                            dir2 = RandomSingleton.GetInstance().RandomElementFromArray(Common.AllDirections());
                        } while (dir == dir2);
                        InstantiateRoom(ref child, ref current, dir2);
                    }
                    else
                    {
                        InstantiateRoom(ref child, ref current, Common.Direction.Right);
                        InstantiateRoom(ref child, ref current, Common.Direction.Down);
                        InstantiateRoom(ref child, ref current, Common.Direction.Left);
                    }
                }
                foreach (var child in current.GetChildren())
                {
                    if (child != null && current.Equals(child.Parent))
                    {
                        toVisit.Enqueue(child);
                    }
                }
            }
            FixLocksAndKeys();
        }

        private static float ProbHasChild(int currentDepth)
        {
            //CurrentDepth -1 enforces 100% chance to create at least 3 rooms
            //(root: depth = 0, first level: depth =1, second level: depth = 2)
            return PROB_HAS_CHILD * (InitialProbability - (currentDepth-1) / (float)(MAX_DEPTH + 1));
        }

        /// Place enemies in random rooms.
        public void PlaceEnemies(int enemies) {
            if (Rooms.Count < 3)
            {
                Debug.LogError("Only 2 rooms");
            }
            while (enemies > 0)
            {
                var index = RandomSingleton.GetInstance().Next(1, Rooms.Count);
                if (Rooms[index].IsGoal) continue;
                Rooms[index].Enemies++;
                enemies--;
            }
        }

        /// Add a lock and a key.
        public void AddLockAndKey() {
            var toVisit = new Queue<Room>();
            toVisit.Enqueue(Rooms[0]);
            var hasKey = false;
            var hasLock = false;
            var lockId = -1;
            while (toVisit.Count > 0 && !hasLock)
            {
                var current = toVisit.Dequeue();
                if (current.Type1 == RoomType.Normal &&
                    !current.Equals(Rooms[0])
                ) {
                    float prob = RandomSingleton.GetInstance().RandomPercent();
                    var chance = RoomFactory.PROB_KEY_ROOM +
                                 RoomFactory.PROB_LOCKER_ROOM;
                    if (chance >= prob)
                    {
                        if (!hasKey)
                        {
                            current.RoomID = Room.GetNextId();
                            current.Key = current.RoomID;
                            current.Type1 = RoomType.Key;
                            lockId = current.RoomID;
                            DungeonGrid[current.X, current.Y] = current;
                            hasKey = true;
                        }
                        else
                        {
                            current.Type1 = RoomType.Locked;
                            current.Key = lockId;
                            DungeonGrid[current.X, current.Y] = current;
                            hasLock = true;
                        }
                    }
                }
                foreach (var child in current.GetChildren())
                {
                    if (child != null && current.Equals(child.Parent))
                    {
                        toVisit.Enqueue(child);
                    }
                }
            }
        }

        /// Remove a lock and a key.
        public void RemoveLockAndKey() {
            // Choose a random key to remove and find its lock
            var keyId = RandomSingleton.GetInstance().Next(0, KeyIds.Count);
            var lockId = -1;
            var keyCount = 0;
            foreach (var room in Rooms)
            {
                if (room.Type1 == RoomType.Key)
                {
                    if (keyId == keyCount)
                    {
                        lockId = room.RoomID;
                    }
                    keyCount++;
                }
            }
            // Remove the key
            var hasKey = false;
            var toVisit = new Queue<Room>();
            toVisit.Enqueue(Rooms[0]);
            while (toVisit.Count > 0 && !hasKey)
            {
                var current = toVisit.Dequeue();
                if (current.Type1 == RoomType.Key && current.RoomID == lockId)
                {
                    current.Type1 = RoomType.Normal;
                    current.Key = -1;
                    DungeonGrid[current.X, current.Y] = current;
                    hasKey = true;
                }
                foreach (var child in current.GetChildren())
                {
                    if (child != null && current.Equals(child.Parent))
                    {
                        toVisit.Enqueue(child);
                    }
                }
            }
            // Remove the lock
            var hasLock = false;
            toVisit.Clear();
            toVisit.Enqueue(Rooms[0]);
            while (toVisit.Count > 0 && !hasLock)
            {
                var current = toVisit.Dequeue();
                if (current.Type1 == RoomType.Locked && current.Key == lockId)
                {
                    current.Type1 = RoomType.Normal;
                    current.Key = -1;
                    DungeonGrid[current.X, current.Y] = current;
                    hasLock = true;
                }
                foreach (var child in current.GetChildren())
                {
                    if (child != null && current.Equals(child.Parent))
                    {
                        toVisit.Enqueue(child);
                    }
                }
            }
        }

        /// Remove the nodes that will be taken out of the dungeon from the dungeon's grid.
        public void RemoveFromGrid(Room cut)
        {
            if (cut != null)
            {
                DungeonGrid[cut.X, cut.Y] = null;
                Rooms.Remove(cut);
                if (cut.Left != null && cut.Left.Parent != null && cut.Left.Parent.Equals(cut))
                {
                    RemoveFromGrid(cut.Left);
                }
                if (cut.Bottom != null && cut.Bottom.Parent != null && cut.Bottom.Parent.Equals(cut))
                {
                    RemoveFromGrid(cut.Bottom);
                }

                if (cut.Right != null && cut.Right.Parent != null && cut.Right.Parent.Equals(cut))
                {
                    RemoveFromGrid(cut.Right);
                }
            }
        }

        /// Update the grid from the dungeon with the position of all the new rooms based on the new rotation of the traded room. If a room already exists in the grid, "ignores" all the children node of this room.
        public void RefreshGrid(ref Room room)
        {
            bool hasInserted;
            if (room != null)
            {
                var dungeonGrid = DungeonGrid;
                dungeonGrid[room.X, room.Y] = room;
                Rooms.Add(room);
                var aux = room.Left;
                if (aux != null && aux.Parent != null && aux.Parent.Equals(room))
                {
                    hasInserted = room.ValidateChild(Common.Direction.Left, dungeonGrid);
                    if (hasInserted)
                    {
                        room.InsertChild(ref dungeonGrid, ref aux, Common.Direction.Left);
                        RefreshGrid(ref aux);
                    }
                    else
                    {
                        room.Left = null;
                    }
                }
                aux = room.Bottom;
                if (aux != null && aux.Parent != null && aux.Parent.Equals(room))
                {
                    hasInserted = room.ValidateChild(Common.Direction.Down, dungeonGrid);
                    if (hasInserted)
                    {
                        room.InsertChild(ref dungeonGrid, ref aux, Common.Direction.Down);
                        RefreshGrid(ref aux);
                    }
                    else
                    {
                        room.Bottom = null;
                    }
                }
                aux = room.Right;
                if (aux != null && aux.Parent != null && aux.Parent.Equals(room))
                {
                    hasInserted = room.ValidateChild(Common.Direction.Right, dungeonGrid);
                    if (hasInserted)
                    {
                        room.InsertChild(ref dungeonGrid, ref aux, Common.Direction.Right);
                        RefreshGrid(ref aux);
                    }
                    else
                        room.Right = null;
                }
            }
        }

        /// Recreate the room list by visiting all the rooms in the tree and
        /// adding them to the list while also counting the number of locks and
        /// keys.
        public void Fix() {
            FixRooms();
            FixMissions();
            FixLocksAndKeys();
            //FixEnemies(totalEnemies);
        }

        /// Fix the list of rooms.
        private void FixRooms()
        {
            var toVisit = new Queue<Room>();
            toVisit.Enqueue(Rooms[0]);
            Rooms.Clear();
            while (toVisit.Count > 0)
            {
                var current = toVisit.Dequeue();
                Rooms.Add(current);
                foreach (var child in current.GetChildren())
                {
                    if (child != null && current.Equals(child.Parent))
                    {
                        toVisit.Enqueue(child);
                    }
                }
            }
        }

        /// Add a lock if the dungeon has none.
        private void FixMissions() {
            if (LockIds.Count == 0)
            {
                if (KeyIds.Count != 0)
                {
                    RemoveLockAndKey();
                }
                AddLockAndKey();
            }
        }

        /// Update the lists of keys and locks, and the grid limits.
        private void FixLocksAndKeys()
        {
            KeyIds.Clear();
            LockIds.Clear();
            MinX = RoomGrid.LEVEL_GRID_OFFSET;
            MinY = RoomGrid.LEVEL_GRID_OFFSET;
            MaxX = -RoomGrid.LEVEL_GRID_OFFSET;
            MaxY = -RoomGrid.LEVEL_GRID_OFFSET;
            foreach (var room in Rooms)
            {
                // Update grid bounds
                MinX = MinX > room.X ? room.X : MinX;
                MinY = MinY > room.Y ? room.Y : MinY;
                MaxX = room.X > MaxX ? room.X : MaxX;
                MaxY = room.Y > MaxY ? room.Y : MaxY;
                // Find the keys and locked doors in the level
                if (room.Type1 == RoomType.Key)
                {
                    KeyIds.Add(room.Key);
                }
                if (room.Type1 == RoomType.Locked)
                {
                    LockIds.Add(room.Key);
                }
            }
        }

        /// Fix the number of enemies and enemy distribution.
        private void FixEnemies( int totalEnemies ) 
        {
            // Remove enemies from the goal room and place them in other rooms
            Room goal = GetGoal();
            if (goal != null && goal.Enemies > 0)
            {
                goal.Enemies = 0;
            }
            // Get the total number of enemies
            int tEnemies = GetNumberOfEnemies();
            if (totalEnemies > tEnemies)
            {
                Redistribute(totalEnemies, totalEnemies - tEnemies);
            }
            else if (tEnemies > totalEnemies)
            {
                RemoveEnemies(totalEnemies, tEnemies - totalEnemies);
            }
        }

        /// Redistribute enemies from hard rooms to easier rooms.
        ///
        /// This method may change the dungeon leniency.
        private void Redistribute(
            int _enemies,
            int _redistribute
        ) {
            // Calculate the mean number of enemies by room
            int mean = _enemies / Rooms.Count;
            // Get the rooms with less enemies than the mean
            List<int> easy = new List<int>();
            for (int i = 0; i < Rooms.Count; i++)
            {
                if (Rooms[i].Equals(Rooms[0]) || Rooms[i].Equals(GetGoal()))
                {
                    continue;
                }
                if (mean >= Rooms[i].Enemies && Rooms[i].Enemies > 0)
                {
                    easy.Add(i);
                }
            }
            // If the list of easy rooms is empty, then add all non-empty rooms
            if (easy.Count == 0)
            {
                for (int i = 0; i < Rooms.Count; i++)
                {
                    if (Rooms[i].Equals(Rooms[0]) || Rooms[i].Equals(GetGoal()))
                    {
                        continue;
                    }
                    if (Rooms[i].Enemies > 0)
                    {
                        easy.Add(i);
                    }
                }
            }
            // If the list of easy rooms is empty, then add all empty rooms
            if (easy.Count == 0)
            {
                for (int i = 0; i < Rooms.Count; i++)
                {
                    if (Rooms[i].Equals(Rooms[0]) || Rooms[i].Equals(GetGoal()))
                    {
                        continue;
                    }
                    if (Rooms[i].Enemies == 0)
                    {
                        easy.Add(i);
                    }
                }
            }
            // Redistribute enemies in easier rooms
            int r = 0;
            while (_redistribute > 0)
            {
                // Add an enemy in the room
                int index = easy[r++];
                Room room = Rooms[index];
                room.Enemies++;
                _redistribute--;
                // If enemies are remaining and the last room have been
                // reached, then back to the first room
                if (r == easy.Count) {
                    r = 0;
                }
            }
        }

        /// Remove enemies from random rooms.
        ///
        /// This method does not change the dungeon leniency.
        public void RemoveEnemies(
            int _enemies,
            int _remove
        ) {
            // Calculate the mean number of enemies by room
            int mean = _enemies / Rooms.Count;
            // Get the harder rooms (rooms with more enemies than the mean)
            List<int> hard = new List<int>();
            for (int i = 0; i < Rooms.Count; i++)
            {
                if (Rooms[i].Enemies > mean)
                {
                    hard.Add(i);
                }
            }
            // If the list of hard rooms is empty, then add all non-empty rooms
            if (hard.Count == 0)
            {
                for (int i = 0; i < Rooms.Count; i++)
                {
                    if (Rooms[i].Enemies > 0)
                    {
                        hard.Add(i);
                    }
                }
            }
            // Remove enemies from the harder rooms
            int r = 0;
            while (_remove > 0)
            {
                // Remove an enemy from the room
                int index = hard[r++];
                Room room = Rooms[index];
                if (room.Enemies > 0)
                {
                    room.Enemies--;
                    _remove--;
                }
                // If enemies are remaining and the last room have been
                // reached, then back to the first room
                if (r == hard.Count)
                {
                    r = 0;
                }
            }
        }

        public void SetBoundariesFromRoomList()
        {
            Coordinates minBoundaries, maxBoundaries;
            minBoundaries = new Coordinates(MinX, MinY);
            maxBoundaries = new Coordinates(MaxX, MaxY);
            DungeonBoundaries = new Boundaries(minBoundaries, maxBoundaries);
        }

        public void SetDimensionsFromBoundaries()
        {
            var width = DungeonBoundaries.MaxBoundaries.X - DungeonBoundaries.MinBoundaries.X + 1;
            var height = DungeonBoundaries.MaxBoundaries.Y - DungeonBoundaries.MinBoundaries.Y + 1;
            DungeonDimensions = new Dimensions(width, height);
        }

        // # Code snippet intended only for writing the generated level

        public string PlayerProfile { get; set; }

        public string NarrativeName { get; set; }
    }
}
