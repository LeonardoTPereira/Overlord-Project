using System;
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
        private const float ProbabilityToHaveChildren = 100f;

        /// The probability of generating a single child.
        private const float ProbabilityToHaveSingleChild = 100f / 3;

        /// The max depth of a dungeon tree.
        private const int MaxDepth = 9;

        /// Room Grid, where a reference to all the existing room will be maintained for quick access when creating nodes.
        public RoomGrid DungeonGrid { get; private set; }

        /// The list of dungeon key IDs.
        public List<int> KeyIds { get; private set; }

        /// The list of locked room IDs.
        public List<int> LockIds { get; private set; }

        /// The lower limit of the x-axis of the grid.
        public int MinX { get; private set; }

        /// The lower limit of the y-axis of the grid.
        public int MinY { get; private set; }

        /// The upper limit of the x-axis of the grid.
        public int MaxX { get; private set; }

        /// The upper limit of the y-axis of the grid.
        public int MaxY { get; private set; }

        /// The list of rooms (easier to add neighbors).
        public List<Room> Rooms { get; private set; }

        public Boundaries DungeonBoundaries { get; private set; }
        public Dimensions DungeonDimensions { get; private set; }

        public int TotalEnemies { get; set; }

        /// Dungeon constructor.
        ///
        /// Create and return a new dungeon with the starting room.
        public Dungeon()
        {
            Rooms = new List<Room>();
            var root = RoomFactory.CreateRoot();
            Rooms.Add(root);
            DungeonGrid = new RoomGrid
            {
                [root.X, root.Y] = root
            };
            LockIds = new List<int>();
            KeyIds = new List<int>();
            MinX = RoomGrid.LEVEL_GRID_OFFSET;
            MinY = RoomGrid.LEVEL_GRID_OFFSET;
            MaxX = -RoomGrid.LEVEL_GRID_OFFSET;
            MaxY = -RoomGrid.LEVEL_GRID_OFFSET;
        }

        /// Return a clone this dungeon.
        public Dungeon (Dungeon originalDungeon)
        {
            Rooms = new List<Room>();
            DungeonGrid = new RoomGrid();
            KeyIds = new List<int>(originalDungeon.KeyIds);
            LockIds = new List<int>(originalDungeon.LockIds);
            MinX = originalDungeon.MinX;
            MinY = originalDungeon.MinY;
            MaxX = originalDungeon.MaxX;
            MaxY = originalDungeon.MaxY;
            var toVisit = new Queue<Room>();
            var cloneToVisit = new Queue<Room>();
            toVisit.Enqueue(originalDungeon.Rooms[0]);
            var aux = new Room(originalDungeon.Rooms[0]);
            Rooms.Add(aux);
            DungeonGrid[aux.X, aux.Y] = aux;
            cloneToVisit.Enqueue(aux);
            while (toVisit.Count > 0)
            {
                var current = toVisit.Dequeue();
                var cloneCurrent = cloneToVisit.Dequeue();
                var children = current.GetChildren();
                for (var i = 0; i < children.Length; ++i)
                {
                    if (children[i] == null) continue;
                    var child = new Room(children[i]);
                    Rooms.Add(child);
                    DungeonGrid[child.X, child.Y] = child;
                    toVisit.Enqueue(children[i]);
                    cloneToVisit.Enqueue(child);
                    child.Parent = cloneCurrent;
                    switch (i)
                    {
                        case 0:
                            cloneCurrent.Left = child;
                            break;
                        case 1:
                            cloneCurrent.Bottom = child;
                            break;
                        default:
                            cloneCurrent.Right = child;
                            break;
                    }
                }
            }
        }

        /// Return the number of rooms of the dungeon.
        public int GetNumberOfRooms()
        {
            return Rooms.Count;
        }

        /// Return the number of enemies of the dungeon.
        public int GetNumberOfEnemies()
        {
            var toVisit = new Queue<Room>();
            toVisit.Enqueue(Rooms[0]);
            var totalEnemies = 0;
            while (toVisit.Count > 0)
            {
                var current = toVisit.Dequeue();
                totalEnemies += current.Enemies;
                current.EnqueueChildrenRooms(toVisit);
            }

            TotalEnemies = totalEnemies;
            return totalEnemies;
            //return Rooms.Sum(room => room.Enemies);
        }

        /// Return the dungeon start room.
        public Room GetStart()
        {
            return Rooms[0];
        }

        /// Return the dungeon goal room.
        public Room GetGoal()
        {
            return Rooms.FirstOrDefault(room => room.IsGoal);
        }

        /// Instantiate a room and tries to add it as a child of the actual
        /// room, considering its direction and position. If there is not a
        /// room in the grid at the entered coordinates, create the room, add
        /// it to the room list and also enqueue it so it can be explored later.
        private void InstantiateRoom(ref Room child, ref Room current, Common.Direction dir)
        {
            var dungeonGrid = DungeonGrid;
            if (!current.ValidateChild(dir, dungeonGrid)) return;
            child = RoomFactory.CreateRoom();
            current.InsertChild(ref dungeonGrid, ref child, dir);
            child.ParentDirection = dir;
            Rooms.Add(child);
            dungeonGrid[child.X, child.Y] = child;
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
                if (currentDepth > MaxDepth)
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

                    if (prob < ProbabilityToHaveSingleChild)
                    {
                        InstantiateRoom(ref child, ref current, dir);
                    }
                    else if (prob < ProbabilityToHaveSingleChild * 2)
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
            return ProbabilityToHaveChildren * (InitialProbability - (currentDepth-1) / (float)(MaxDepth + 1));
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
                if (current.Type == RoomType.Normal &&
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
                            current.Type = RoomType.Key;
                            lockId = current.RoomID;
                            DungeonGrid[current.X, current.Y] = current;
                            hasKey = true;
                        }
                        else
                        {
                            current.Type = RoomType.Locked;
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
            foreach (var room in Rooms.Where(room => room.Type == RoomType.Key))
            {
                if (keyId == keyCount)
                {
                    lockId = room.RoomID;
                }
                keyCount++;
            }
            // Remove the key
            var hasKey = false;
            var toVisit = new Queue<Room>();
            toVisit.Enqueue(Rooms[0]);
            while (toVisit.Count > 0 && !hasKey)
            {
                var current = toVisit.Dequeue();
                if (current.Type == RoomType.Key && current.RoomID == lockId)
                {
                    current.Type = RoomType.Normal;
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
                if (current.Type == RoomType.Locked && current.Key == lockId)
                {
                    current.Type = RoomType.Normal;
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
            if (cut == null) return;
            DungeonGrid[cut.X, cut.Y] = null;
            Rooms.Remove(cut);
            if (cut.Left?.Parent != null && cut.Left.Parent.Equals(cut))
            {
                RemoveFromGrid(cut.Left);
            }
            if (cut.Bottom?.Parent != null && cut.Bottom.Parent.Equals(cut))
            {
                RemoveFromGrid(cut.Bottom);
            }

            if (cut.Right?.Parent != null && cut.Right.Parent.Equals(cut))
            {
                RemoveFromGrid(cut.Right);
            }
        }

        /// Update the grid from the dungeon with the position of all the new rooms based on the new rotation of the traded room. If a room already exists in the grid, "ignores" all the children node of this room.
        public void RefreshGrid(ref Room room)
        {
            bool hasInserted;
            if (room == null) return;
            var dungeonGrid = DungeonGrid;
            dungeonGrid[room.X, room.Y] = room;
            Rooms.Add(room);
            var aux = room.Left;
            if (aux?.Parent != null && aux.Parent.Equals(room))
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
            if (aux?.Parent != null && aux.Parent.Equals(room))
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
            if (aux?.Parent == null || !aux.Parent.Equals(room)) return;
            hasInserted = room.ValidateChild(Common.Direction.Right, dungeonGrid);
            if (hasInserted)
            {
                room.InsertChild(ref dungeonGrid, ref aux, Common.Direction.Right);
                RefreshGrid(ref aux);
            }
            else
                room.Right = null;
        }

        /// Recreate the room list by visiting all the rooms in the tree and
        /// adding them to the list while also counting the number of locks and
        /// keys.
        public void Fix(int enemies) {
            if (GetNumberOfEnemies() != enemies)
            {
                Debug.LogError($"Requested {enemies} Enemies, found {GetNumberOfEnemies()}");
            }
            FixRooms();
            if (GetNumberOfEnemies() != enemies)
            {
                Debug.LogError($"Requested {enemies} Enemies, found {GetNumberOfEnemies()}");
            }
            FixMissions();
            if (GetNumberOfEnemies() != enemies)
            {
                Debug.LogError($"Requested {enemies} Enemies, found {GetNumberOfEnemies()}");
            }
            FixLocksAndKeys();
            if (GetNumberOfEnemies() != enemies)
            {
                Debug.LogError($"Requested {enemies} Enemies, found {GetNumberOfEnemies()}");
            }
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
                current.IsGoal = false;
                Rooms.Add(current);
                current.EnqueueChildrenRooms(toVisit);
            }
        }

        /// Add a lock if the dungeon has none.
        private void FixMissions()
        {
            if (LockIds.Count != 0) return;
            if (KeyIds.Count != 0)
            {
                RemoveLockAndKey();
            }
            AddLockAndKey();
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
            Room lastLockedRoom = null;
            foreach (var room in Rooms)
            {
                // Update grid bounds
                MinX = MinX > room.X ? room.X : MinX;
                MinY = MinY > room.Y ? room.Y : MinY;
                MaxX = room.X > MaxX ? room.X : MaxX;
                MaxY = room.Y > MaxY ? room.Y : MaxY;
                switch (room.Type)
                {
                    // Find the keys and locked doors in the level
                    case RoomType.Key:
                        KeyIds.Add(room.Key);
                        break;
                    case RoomType.Locked:
                        lastLockedRoom = room;
                        LockIds.Add(room.Key);
                        break;
                    case RoomType.Normal:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (lastLockedRoom == null)
            {
                SetNewGoal(Rooms[^1]);
                return;
            }
            SetNewGoal(lastLockedRoom);
        }

        private void SetNewGoal(Room goalRoom)
        {
            goalRoom.IsGoal = true;
            var enemiesToDistribute = goalRoom.Enemies;
            if (enemiesToDistribute <= 0) return;
            for (var i = Rooms.Count-1; i > 0; i--)
            {
                if (Rooms[i].Enemies != 0) continue;
                Rooms[i].Enemies = enemiesToDistribute;
                goalRoom.Enemies = 0;
                return;
            }

            while (enemiesToDistribute > 0)
            {
                var index = RandomSingleton.GetInstance().Next(1, Rooms.Count);
                if(Rooms[index].IsGoal) continue;
                Rooms[index].Enemies++;
                enemiesToDistribute--;
            }
            goalRoom.Enemies = 0;
        }

        public void SetBoundariesFromRoomList()
        {
            var minBoundaries = new Coordinates(MinX, MinY);
            var maxBoundaries = new Coordinates(MaxX, MaxY);
            DungeonBoundaries = new Boundaries(minBoundaries, maxBoundaries);
        }

        public void SetDimensionsFromBoundaries()
        {
            var width = DungeonBoundaries.MaxBoundaries.X - DungeonBoundaries.MinBoundaries.X + 1;
            var height = DungeonBoundaries.MaxBoundaries.Y - DungeonBoundaries.MinBoundaries.Y + 1;
            DungeonDimensions = new Dimensions(width, height);
        }
    }
}
