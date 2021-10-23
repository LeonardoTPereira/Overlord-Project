using Game.LevelManager;
using System;
using System.Collections;
using System.Collections.Generic;
using static Enums;

namespace LevelGenerator
{
    public class Dungeon
    {
        /// The probability of generating a new room as a child.
        public static readonly float PROB_HAS_CHILD = 100f;
        /// The probability of generating a single child.
        public static readonly float PROB_CHILD = 100f / 3;
        /// The max depth of a dungeon tree.
        public static readonly int MAX_DEPTH = 5;

        /// Room Grid, where a reference to all the existing room will be maintained for quick access when creating nodes.
        public RoomGrid grid;
        /// The list of rooms (easier to add neighbors).
        public List<Room> rooms;
        /// The list of dungeon key IDs.
        public List<int> keyIds;
        /// The list of locked room IDs.
        public List<int> lockIds;
        /// The lower limit of the x-axis of the grid.
        public int minX;
        /// The lower limit of the y-axis of the grid.
        public int minY;
        /// The upper limit of the x-axis of the grid.
        public int maxX;
        /// The upper limit of the y-axis of the grid.
        public int maxY;

        public List<Room> Rooms { get => rooms; }

        public Boundaries boundaries;
        public Dimensions dimensions;

        /// Dungeon constructor.
        ///
        /// Create and return a new dungeon with the starting room.
        public Dungeon()
        {
            rooms = new List<Room>();
            Room root = RoomFactory.CreateRoot();
            rooms.Add(root);
            grid = new RoomGrid();
            grid[root.x, root.y] = root;
            lockIds = new List<int>();
            keyIds = new List<int>();
            minX = RoomGrid.LEVEL_GRID_OFFSET;
            minY = RoomGrid.LEVEL_GRID_OFFSET;
            maxX = -RoomGrid.LEVEL_GRID_OFFSET;
            maxY = -RoomGrid.LEVEL_GRID_OFFSET;
        }

        /// Return a clone this dungeon.
        public Dungeon Clone()
        {
            Dungeon dungeon = new Dungeon();
            dungeon.rooms = new List<Room>();
            dungeon.grid = new RoomGrid();
            foreach (Room old in rooms)
            {
                Room aux = old.Clone();
                dungeon.rooms.Add(aux);
                dungeon.grid[old.x, old.y] = aux;
            }
            dungeon.keyIds = new List<int>(keyIds);
            dungeon.lockIds = new List<int>(lockIds);
            dungeon.minX = minX;
            dungeon.minY = minY;
            dungeon.maxX = maxX;
            dungeon.maxY = maxY;
            // Need to use the grid to copy the neighboors, children and parent
            // Check the position of the node in the grid and then substitute the old room with the copied one
            foreach (Room room in dungeon.rooms)
            {
                if (room.parent != null)
                {
                    room.parent = dungeon.grid[room.parent.x, room.parent.y];
                }
                if (room.right != null)
                {
                    room.right = dungeon.grid[room.right.x, room.right.y];
                }
                if (room.bottom != null)
                {
                    room.bottom = dungeon.grid[room.bottom.x, room.bottom.y];
                }
                if (room.left != null)
                {
                    room.left = dungeon.grid[room.left.x, room.left.y];
                }
            }
            return dungeon;
        }

        /// Return the number of rooms of the dungeon.
        public int GetNumberOfRooms()
        {
            return rooms.Count;
        }

        /// Return the number of enemies of the dungeon.
        public int GetNumberOfEnemies()
        {
            int sum = 0;
            foreach (Room room in rooms)
            {
                sum += room.enemies;
            }
            return sum;
        }

        /// Return the dungeon start room.
        public Room GetStart()
        {
            return rooms[0];
        }

        /// Return the dungeon goal room.
        public Room GetGoal()
        {
            Room goal = null;
            foreach (Room room in rooms)
            {
                if (room.type != RoomType.Locked) { continue; }
                int _lock = lockIds.IndexOf(room.key);
                if (_lock == lockIds.Count - 1)
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
            Common.Direction dir,
            ref Random rand
        ) {
            if (current.ValidateChild(dir, grid))
            {
                child = RoomFactory.CreateRoom(ref rand);
                current.InsertChild(ref grid, ref child, dir);
                child.parentDirection = dir;
                rooms.Add(child);
                grid[child.x, child.y] = child;
            }
        }

        /// While a node (room) has not been visited, or while the max depth of the tree has not been reached, visit each node and create its children.
        public void GenerateRooms(
            ref Random _rand
        ) {
            Queue<Room> toVisit = new Queue<Room>();
            toVisit.Enqueue(rooms[0]);
            int prob;
            while (toVisit.Count > 0)
            {
                Room current = toVisit.Dequeue();
                int actualDepth = current.depth;
                //If max depth allowed has been reached, stop creating children
                if (actualDepth > MAX_DEPTH)
                {
                    toVisit.Clear();
                    break;
                }
                //Check how many children the node will have, if any.
                prob = Common.RandomPercent(ref _rand);
                //Console.WriteLine(prob);
                //The parent node has 100% chance to have children, then, at each height, 10% of chance to NOT have children is added.
                //If a node has a child, create it with the RoomFactory, insert it as a child of the actual node in the right place
                //Also enqueue it to be visited later and add it to the list of the rooms of this dungeon
                if (prob <= (PROB_HAS_CHILD * (1 - actualDepth / (MAX_DEPTH + 1))))
                {
                    Room child = null;
                    Common.Direction dir = Common.RandomElementFromArray(Common.AllDirections(), ref _rand);
                    prob = Common.RandomPercent(ref _rand);

                    if (prob < PROB_CHILD)
                    {
                        InstantiateRoom(ref child, ref current, dir, ref _rand);
                    }
                    else if (prob < PROB_CHILD * 2)
                    {
                        InstantiateRoom(ref child, ref current, dir, ref _rand);
                        Common.Direction dir2;
                        do
                        {
                            dir2 = Common.RandomElementFromArray(Common.AllDirections(), ref _rand);
                        } while (dir == dir2);
                        InstantiateRoom(ref child, ref current, dir2, ref _rand);
                    }
                    else
                    {
                        InstantiateRoom(ref child, ref current, Common.Direction.Right, ref _rand);
                        InstantiateRoom(ref child, ref current, Common.Direction.Down, ref _rand);
                        InstantiateRoom(ref child, ref current, Common.Direction.Left, ref _rand);
                    }
                }
                foreach (Room child in current.GetChildren())
                {
                    if (child != null && current.Equals(child.parent))
                    {
                        toVisit.Enqueue(child);
                    }
                }
            }
            FixLocksAndKeys();
        }

        /// Place enemies in random rooms.
        public void PlaceEnemies(
            int _enemies,
            ref Random _rand
        ) {
            while (_enemies > 0)
            {
                int index = Common.RandomInt((1, rooms.Count - 1), ref _rand);
                if (!rooms[index].Equals(GetGoal()))
                {
                    rooms[index].enemies++;
                    _enemies--;
                }
            }
        }

        /// Add a lock and a key.
        public void AddLockAndKey(
            ref Random _rand
        ) {
            Queue<Room> toVisit = new Queue<Room>();
            toVisit.Enqueue(rooms[0]);
            bool hasKey = false;
            bool hasLock = false;
            int lockId = -1;
            while (toVisit.Count > 0 && !hasLock)
            {
                Room current = toVisit.Dequeue();
                if (current.type == RoomType.Normal &&
                    !current.Equals(rooms[0])
                ) {
                    float prob = Common.RandomPercent(ref _rand);
                    float chance = RoomFactory.PROB_KEY_ROOM +
                        RoomFactory.PROB_LOCKER_ROOM;
                    if (chance >= prob)
                    {
                        if (!hasKey)
                        {
                            current.id = Room.GetNextId();
                            current.key = current.id;
                            current.type = RoomType.Key;
                            lockId = current.id;
                            grid[current.x, current.y] = current;
                            hasKey = true;
                        }
                        else
                        {
                            current.type = RoomType.Locked;
                            current.key = lockId;
                            grid[current.x, current.y] = current;
                            hasLock = true;
                        }
                    }
                }
                foreach (Room child in current.GetChildren())
                {
                    if (child != null && current.Equals(child.parent))
                    {
                        toVisit.Enqueue(child);
                    }
                }
            }
        }

        /// Remove a lock and a key.
        public void RemoveLockAndKey(
            ref Random _rand
        ) {
            // Choose a random key to remove and find its lock
            int keyId = Common.RandomInt((0, keyIds.Count - 1), ref _rand);
            int lockId = -1;
            int keyCount = 0;
            foreach (Room room in rooms)
            {
                if (room.type == RoomType.Key)
                {
                    if (keyId == keyCount)
                    {
                        lockId = room.id;
                    }
                    keyCount++;
                }
            }
            // Remove the key
            bool hasKey = false;
            Queue<Room> toVisit = new Queue<Room>();
            toVisit.Enqueue(rooms[0]);
            while (toVisit.Count > 0 && !hasKey)
            {
                Room current = toVisit.Dequeue();
                if (current.type == RoomType.Key)
                {
                    if (current.id == lockId)
                    {
                        current.type = RoomType.Normal;
                        current.key = -1;
                        grid[current.x, current.y] = current;
                        hasKey = true;
                    }
                }
                foreach (Room child in current.GetChildren())
                {
                    if (child != null && current.Equals(child.parent))
                    {
                        toVisit.Enqueue(child);
                    }
                }
            }
            // Remove the lock
            bool hasLock = false;
            toVisit.Clear();
            toVisit.Enqueue(rooms[0]);
            while (toVisit.Count > 0 && !hasLock)
            {
                Room current = toVisit.Dequeue();
                if (current.type == RoomType.Locked)
                {
                    if (current.key == lockId)
                    {
                        current.type = RoomType.Normal;
                        current.key = -1;
                        grid[current.x, current.y] = current;
                        hasLock = true;
                    }
                }
                foreach (Room child in current.GetChildren())
                {
                    if (child != null && current.Equals(child.parent))
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
                grid[cut.x, cut.y] = null;
                rooms.Remove(cut);
                if (cut.left != null && cut.left.parent != null && cut.left.parent.Equals(cut))
                {
                    RemoveFromGrid(cut.left);
                }
                if (cut.bottom != null && cut.bottom.parent != null && cut.bottom.parent.Equals(cut))
                {
                    RemoveFromGrid(cut.bottom);
                }

                if (cut.right != null && cut.right.parent != null && cut.right.parent.Equals(cut))
                {
                    RemoveFromGrid(cut.right);
                }
            }
        }

        /// Update the grid from the dungeon with the position of all the new rooms based on the new rotation of the traded room. If a room already exists in the grid, "ignores" all the children node of this room.
        public void RefreshGrid(ref Room room)
        {
            bool hasInserted;
            if (room != null)
            {
                grid[room.x, room.y] = room;
                rooms.Add(room);
                Room aux = room.left;
                if (aux != null && aux.parent != null && aux.parent.Equals(room))
                {
                    hasInserted = room.ValidateChild(Common.Direction.Left, grid);
                    if (hasInserted)
                    {
                        room.InsertChild(ref grid, ref aux, Common.Direction.Left);
                        RefreshGrid(ref aux);
                    }
                    else
                    {
                        room.left = null;
                    }
                }
                aux = room.bottom;
                if (aux != null && aux.parent != null && aux.parent.Equals(room))
                {
                    hasInserted = room.ValidateChild(Common.Direction.Down, grid);
                    if (hasInserted)
                    {
                        room.InsertChild(ref grid, ref aux, Common.Direction.Down);
                        RefreshGrid(ref aux);
                    }
                    else
                    {
                        room.bottom = null;
                    }
                }
                aux = room.right;
                if (aux != null && aux.parent != null && aux.parent.Equals(room))
                {
                    hasInserted = room.ValidateChild(Common.Direction.Right, grid);
                    if (hasInserted)
                    {
                        room.InsertChild(ref grid, ref aux, Common.Direction.Right);
                        RefreshGrid(ref aux);
                    }
                    else
                        room.right = null;
                }
            }
        }

        /// Recreate the room list by visiting all the rooms in the tree and
        /// adding them to the list while also counting the number of locks and
        /// keys.
        public void Fix(
            Parameters _prs,
            ref Random _rand
        ) {
            FixRooms();
            FixMissions(ref _rand);
            FixLocksAndKeys();
            FixEnemies(_prs);
        }

        /// Fix the list of rooms.
        private void FixRooms()
        {
            Queue<Room> toVisit = new Queue<Room>();
            toVisit.Enqueue(rooms[0]);
            rooms.Clear();
            while (toVisit.Count > 0)
            {
                Room current = toVisit.Dequeue();
                rooms.Add(current);
                foreach (Room child in current.GetChildren())
                {
                    if (child != null && current.Equals(child.parent))
                    {
                        toVisit.Enqueue(child);
                    }
                }
            }
        }

        /// Add a lock if the dungeon has none.
        private void FixMissions(
            ref Random _rand
        ) {
            if (lockIds.Count == 0)
            {
                if (keyIds.Count != 0)
                {
                    RemoveLockAndKey(ref _rand);
                }
                AddLockAndKey(ref _rand);
            }
        }

        /// Update the lists of keys and locks, and the grid limits.
        private void FixLocksAndKeys()
        {
            keyIds.Clear();
            lockIds.Clear();
            minX = RoomGrid.LEVEL_GRID_OFFSET;
            minY = RoomGrid.LEVEL_GRID_OFFSET;
            maxX = -RoomGrid.LEVEL_GRID_OFFSET;
            maxY = -RoomGrid.LEVEL_GRID_OFFSET;
            foreach (Room room in rooms)
            {
                // Update grid bounds
                minX = minX > room.x ? room.x : minX;
                minY = minY > room.y ? room.y : minY;
                maxX = room.x > maxX ? room.x : maxX;
                maxY = room.y > maxY ? room.y : maxY;
                // Find the keys and locked doors in the level
                if (room.type == RoomType.Key)
                {
                    keyIds.Add(room.key);
                }
                if (room.type == RoomType.Locked)
                {
                    lockIds.Add(room.key);
                }
            }
        }

        /// Fix the number of enemies and enemy distribution.
        private void FixEnemies(
            Parameters _prs
        ) {
            // Remove enemies from the goal room and place them in other rooms
            Room goal = GetGoal();
            if (goal != null && goal.enemies > 0)
            {
                goal.enemies = 0;
            }
            // Get the total number of enemies
            int tEnemies = GetNumberOfEnemies();
            if (_prs.enemies > tEnemies)
            {
                Redistribute(_prs.enemies, _prs.enemies - tEnemies);
            }
            else if (tEnemies > _prs.enemies)
            {
                RemoveEnemies(_prs.enemies, tEnemies - _prs.enemies);
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
            int mean = _enemies / rooms.Count;
            // Get the rooms with less enemies than the mean
            List<int> easy = new List<int>();
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].Equals(rooms[0]) || rooms[i].Equals(GetGoal()))
                {
                    continue;
                }
                if (mean >= rooms[i].enemies && rooms[i].enemies > 0)
                {
                    easy.Add(i);
                }
            }
            // If the list of easy rooms is empty, then add all non-empty rooms
            if (easy.Count == 0)
            {
                for (int i = 0; i < rooms.Count; i++)
                {
                    if (rooms[i].Equals(rooms[0]) || rooms[i].Equals(GetGoal()))
                    {
                        continue;
                    }
                    if (rooms[i].enemies > 0)
                    {
                        easy.Add(i);
                    }
                }
            }
            // If the list of easy rooms is empty, then add all empty rooms
            if (easy.Count == 0)
            {
                for (int i = 0; i < rooms.Count; i++)
                {
                    if (rooms[i].Equals(rooms[0]) || rooms[i].Equals(GetGoal()))
                    {
                        continue;
                    }
                    if (rooms[i].enemies == 0)
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
                Room room = rooms[index];
                room.enemies++;
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
            int mean = _enemies / rooms.Count;
            // Get the harder rooms (rooms with more enemies than the mean)
            List<int> hard = new List<int>();
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].enemies > mean)
                {
                    hard.Add(i);
                }
            }
            // If the list of hard rooms is empty, then add all non-empty rooms
            if (hard.Count == 0)
            {
                for (int i = 0; i < rooms.Count; i++)
                {
                    if (rooms[i].enemies > 0)
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
                Room room = rooms[index];
                if (room.enemies > 0)
                {
                    room.enemies--;
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
            minBoundaries = new Coordinates(minX, minY);
            maxBoundaries = new Coordinates(maxX, maxY);
            boundaries = new Boundaries(minBoundaries, maxBoundaries);
        }

        public void SetDimensionsFromBoundaries()
        {
            int width = boundaries.MaxBoundaries.X - boundaries.MinBoundaries.X + 1;
            int height = boundaries.MaxBoundaries.Y - boundaries.MinBoundaries.Y + 1;
            dimensions = new Dimensions(width, height);
        }

        // # Code snippet intended only for writing the generated level

        public JSonWriter.ParametersMonsters parametersMonsters;
        public JSonWriter.ParametersItems parametersItems;
        public JSonWriter.ParametersNpcs parametersNpcs;
        private string playerProfile;
        private string narrativeName;
        public string PlayerProfile { get => playerProfile; set => playerProfile = value; }
        public string NarrativeName { get => narrativeName; set => narrativeName = value; }

        public void SetNarrativeParameters(JSonWriter.ParametersMonsters parametersMonsters,
            JSonWriter.ParametersNpcs parametersNpcs,
            JSonWriter.ParametersItems parametersItems,
            string playerProfile, string narrativeName)
        {
            this.parametersItems = parametersItems;
            this.parametersMonsters = parametersMonsters;
            this.parametersNpcs = parametersNpcs;
            PlayerProfile = playerProfile;
            NarrativeName = narrativeName;
        }
    }
}