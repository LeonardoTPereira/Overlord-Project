using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Game.LevelGenerator
{
    /// The types of rooms that a dungeon may have.
    ///
    /// A normal room is a free access room that does not contain items.
    /// A key room is a free access room that contains a key.
    /// A locked room is a room that requires a key to be accessed by a player.
    public enum RoomType
    {
        Normal = 0, // Normal room
        Key = 1,    // Room with a key
        Locked = 2, // Locked room
    };

    /// This class represents a room of a dungeon.
    public class Room
    {
        /// Counter of IDs (a sequential identifier) of rooms.
        private static int ID = 0;

        /// Calculate and return the next room ID.
        public static int GetNextId()
        {
            return ID++;
        }

        /// 90 degree rotation.
        private static readonly int DEGREE_90 = 90;
        /// 360 degree rotation.
        private static readonly int DEGREE_360 = 360;

        /// The room ID.
        public int RoomID { get; set; }

        /// The type of the room.
        public RoomType Type { get; set; }

        /// The ID of the key that opens this room. The ID is equal to -1 when
        /// the room is not locked and does not have a key.
        public int Key { get; set; } = -1;

        /// The number of enemies in this room.
        public int Enemies { get; set; }

        public bool IsGoal { get; set; }

        /// The depth of the room in the tree. This is used to control the
        /// depth of the dungeon level.
        public int Depth { get; set; }

        /// The rotation of the individual's parent position is related to the
        /// normal cartesian orientation. 0 means that the parent is in the
        /// North (above) of the child, 90 the parent is in the East (right),
        /// and so on. This is used to build the dungeon grid.
        public int Rotation { get; set; } = 0;

        /// The room's left child.
        public Room Left { get; set; } = null;

        /// The room's bottom child.
        public Room Bottom { get; set; } = null;

        /// The room's right child.
        public Room Right { get; set; } = null;

        /// The room's parent.
        public Room Parent { get; set; } = null;

        /// The direction from what the parent connects with this room.
        /// This attribute reduces operations at crossover.
        public Common.Direction ParentDirection { get; set; } = Common.Direction.Down;
        /// The x position of the room in the grid.
        public int X { get; set; }

        /// The y position of the room in the grid.
        public int Y { get; set; }
        /// Room constructor.
        ///
        /// The default is a normal room without a predefined room ID. The key
        /// room defines its key to open as its room ID. The locked room
        /// defines its key to open as the entered key to open.
        public Room(RoomType _type = RoomType.Normal, int _key = -1, int _id = -1)
        {
            Depth = 0;
            Type = _type;
            RoomID = _id == -1 ? Room.GetNextId() : _id;
            Key = Type == RoomType.Key ? RoomID : Key;
            Key = Type == RoomType.Locked ? _key : Key;
            IsGoal = false;
            Enemies = 0;
        }

        /// Return a clone this room.
        public Room(Room copiedRoom)
        {
            Enemies = copiedRoom.Enemies;
            Depth = copiedRoom.Depth;
            X = copiedRoom.X;
            Y = copiedRoom.Y;
            Rotation = copiedRoom.Rotation;
            Left = null;
            Bottom = null;
            Right = null;
            Parent = null;
            ParentDirection = copiedRoom.ParentDirection;
            IsGoal = copiedRoom.IsGoal;
            Depth = copiedRoom.Depth;
            Type = copiedRoom.Type;
            RoomID = copiedRoom.RoomID;
            Key = copiedRoom.Key;
        }

        /// Return true if the room is a leaf node.
        public bool IsLeafNode()
        {
            return Bottom == null && Left == null && Right == null;
        }

        /// Return an array with the left, bottom, and right children.
        public Room[] GetChildren()
        {
            return new [] {
                    Left,
                    Bottom,
                    Right,
                };
        }

        /// Return an array with all the neighbors (parent and children).
        public Room[] GetNeighbors()
        {
            return new Room[] {
                    Parent,
                    Left,
                    Bottom,
                    Right,
                };
        }

        /// Return a tuple corresponding to the position in the dungeon grid of
        /// the child of a parent in the entered direction.
        public (int, int) GetChildPositionInGrid(
            Common.Direction _dir
        ) {
            var cx = 0;
            var cy = 0;
            var rot = (Rotation / DEGREE_90) % 2;
            switch (_dir)
            {
                case Common.Direction.Right:
                    if (rot != 0)
                    {
                        cx = X;
                        cy = Rotation == DEGREE_90 ? Y + 1 : Y - 1;
                    }
                    else
                    {
                        cx = Rotation == 0 ? X + 1 : X - 1;
                        cy = Y;
                    }
                    break;

                case Common.Direction.Down:
                    if (rot != 0)
                    {
                        cx = Rotation == DEGREE_90 ? X + 1 : X - 1;
                        cy = Y;
                    }
                    else
                    {
                        cx = X;
                        cy = Rotation == 0 ? Y - 1 : Y + 1;
                    }
                    break;

                case Common.Direction.Left:
                    if (rot != 0)
                    {
                        cx = X;
                        cy = Rotation == DEGREE_90 ? Y - 1 : Y + 1;
                    }
                    else
                    {
                        cx = Rotation == 0 ? X - 1 : X + 1;
                        cy = Y;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_dir), _dir, null);
            }
            return (cx, cy);
        }

        /// Check if a room can have a new child in the entered direction.
        ///
        /// Return true if a room can be placed as a child of the entered room
        /// parent and in the entered position (i.e., the position in the grid
        /// is empty), and false, otherwise.
        public bool ValidateChild(
            Common.Direction _dir,
            RoomGrid _grid
        ) {
            (var x, var y) = GetChildPositionInGrid(_dir);
            return _grid[x, y] is null;
        }

        /// Insert the entered room in the dungeon (ternary heap and grid).
        ///
        /// First, this method calculates both X and Y positions of the entered
        /// child room based on the parent rotation and coordinates, and on the
        /// direction of insertion. Then, it checks the position is empty in
        /// the dungeon grid, if so, then, it places the entered child room in
        /// the calculated position and rotation.
        public void InsertChild(
            ref RoomGrid _grid,
            ref Room _child,
            Common.Direction _dir
        ) {
            (var x, var y) = GetChildPositionInGrid(_dir);
            _child.X = x;
            _child.Y = y;
            
            var room = _grid[x, y];
            if (room != null) { return; }
            switch (_dir)
            {
                case Common.Direction.Right:
                    Right = _child;
                    Right.Parent = this;
                    Right.Depth = Depth + 1;
                    _child.Rotation = RotationModulo(Rotation + DEGREE_90);
                    break;
                case Common.Direction.Down:
                    Bottom = _child;
                    Bottom.Parent = this;
                    Bottom.Depth = Depth + 1;
                    _child.Rotation = Rotation;
                    break;
                case Common.Direction.Left:
                    Left = _child;
                    Left.Parent = this;
                    Left.Depth = Depth + 1;
                    _child.Rotation = RotationModulo(Rotation - DEGREE_90);
                    break;
            }
        }

        private int RotationModulo(int newRotation)
        {
            var result = newRotation % DEGREE_360;
            return result < 0 ? result + DEGREE_360 : result;
        }

        /// Fix the branch that starts in this node.
        ///
        /// This method must be called after a successful crossover. This fix
        /// reinserts the mission rooms in the old branch to maintain the
        /// occurring order of missions to guarantee feasibility.
        public void FixBranch(List<int> _missions) {
            // If both lock and key are in the branch, assign to them new IDs,
            // and add all the missions in the new missions list
            var newMissions = new Queue<int>();
            for (var i = 0; i < _missions.Count - 1; i++)
            {
                for (var j = i + 1; j < _missions.Count; j++)
                {
                    if (_missions[i] == -_missions[j])
                    {
                        var newId = GetNextId();
                        _missions[i] = _missions[i] > 0 ? newId : -newId;
                        _missions[j] = -_missions[i];
                    }
                }
                newMissions.Enqueue(_missions[i]);
            }
            // Also add the last mission, if it exists
            if (_missions.Count > 0)
            {
                newMissions.Enqueue(_missions[_missions.Count - 1]);
            }

            // Gather all the rooms of the branch
            var branch = new Queue<Room>();
            var toVisit = new Queue<Room>();
            toVisit.Enqueue(this);
            while (toVisit.Count > 0)
            {
                var current = toVisit.Dequeue();
                branch.Enqueue(current);
                foreach (var child in current.GetChildren())
                {
                    if (child != null && current.Equals(child.Parent))
                    {
                        toVisit.Enqueue(child);
                    }
                }
            }

            // Place missions in the branch randomly while there are more rooms than missions; otherwise, this while stops
            while (branch.Count > newMissions.Count)
            {
                var current = branch.Dequeue();
                var prob = RandomSingleton.GetInstance().RandomPercent();
                // If there are no missions left, then assign the remaining
                // rooms as normal rooms; otherwise, check if the current room
                // will not receive a mission
                if (newMissions.Count == 0 ||
                    RoomFactory.PROB_NORMAL_ROOM > prob
                ) {
                    current.Type = RoomType.Normal;
                    current.Key = -1;
                }
                else
                {
                    // The current room will receive a mission
                    var missionId = newMissions.Dequeue();
                    // Assign the mission ID to the current room
                    FixMission(ref current, missionId);
                }
            }

            // If new missions are remaining, it means that the number of
            // Remaining rooms is the same as the number of mission rooms
            // thus, place the missions in those rooms
            while (branch.Count > 0 && newMissions.Count > 0)
            {
                // The current room will receive a mission
                var current = branch.Dequeue();
                var missionId = newMissions.Dequeue();
                // Assign the mission ID to the current room
                FixMission(ref current, missionId);
            }
        }

        /// Auxiliary method for fixing the branch; it assigns the entered
        /// mission to the entered room.
        private void FixMission(
            ref Room _room,
            int _mission
        ) {
            if (_mission > 0)
            {
                _room.Type = RoomType.Key;
                _room.RoomID = _mission;
                _room.Key = _mission;
            }
            else
            {
                _room.Type = RoomType.Locked;
                _room.Key = -_mission;
            }
        }

        public Queue<int> GetEnemiesPerRoom()
        {
            var toVisit = new Queue<Room>();
            var enemiesInRoom = new Queue<int>();
            toVisit.Enqueue(this);
            while (toVisit.Count > 0)
            {
                var current = toVisit.Dequeue();
                if (current.Enemies > 0)
                {
                    enemiesInRoom.Enqueue(current.Enemies);
                }
                current.EnqueueChildrenRooms(toVisit);
            }
            return enemiesInRoom;
        }
        
        public void FixEnemies(Queue<int> enemiesPerRoom)
        {
            if (enemiesPerRoom.Count == 0) return;
            // Gather all the rooms of the branch
            var branch = new Queue<Room>();
            var toVisit = new Queue<Room>();
            toVisit.Enqueue(this);
            var existingEnemies = 0;
            while (toVisit.Count > 0)
            {
                var current = toVisit.Dequeue();
                existingEnemies += current.Enemies;
                current.Enemies = 0;
                current.EnqueueChildrenRooms(toVisit);
                branch.Enqueue(current);
            }

            var oldEnemies = 0;
            foreach (var enemies in enemiesPerRoom)
            {
                oldEnemies += enemies;
            }
            enemiesPerRoom = RedistributeEnemiesIfNotEnoughRooms(enemiesPerRoom, branch.Count);
            var newEnemies = 0;
            foreach (var enemies in enemiesPerRoom)
            {
                newEnemies += enemies;
            }

            if (newEnemies != oldEnemies)
            {
                Debug.LogError($"Different enemies in Queue: old={oldEnemies}, new={newEnemies}");
            }
            while (branch.Count > enemiesPerRoom.Count)
            {
                var current = branch.Dequeue();
                var prob = RandomSingleton.GetInstance().RandomPercent();
                if (prob > 50) continue;
                current.Enemies = enemiesPerRoom.Dequeue();
                if (enemiesPerRoom.Count == 0) return;
            }

            while (branch.Count > 0)
            {
                var current = branch.Dequeue();
                current.Enemies = enemiesPerRoom.Dequeue();
            }
            
            toVisit.Clear();
            toVisit.Enqueue(this);
            var newEnemiesInBranch = 0;
            while (toVisit.Count > 0)
            {
                var current = toVisit.Dequeue();
                current.EnqueueChildrenRooms(toVisit);
                newEnemiesInBranch += current.Enemies;
            }
        }

        private static Queue<int> RedistributeEnemiesIfNotEnoughRooms(Queue<int> enemiesPerRoom, int totalRooms)
        {
            if (totalRooms >= enemiesPerRoom.Count) return enemiesPerRoom;
            
            var excessEnemies = 0;

            while (totalRooms < enemiesPerRoom.Count)
            {
                excessEnemies += enemiesPerRoom.Dequeue();
            }

            var newEnemiesPerRoom = excessEnemies / enemiesPerRoom.Count;
            var extraEnemies = excessEnemies % enemiesPerRoom.Count;
            var enemiesToDistribute = new Queue<int>();
            enemiesToDistribute.Enqueue(enemiesPerRoom.Dequeue() + newEnemiesPerRoom + extraEnemies);
            while (enemiesPerRoom.Count > 0)
            {
                enemiesToDistribute.Enqueue(enemiesPerRoom.Dequeue() + newEnemiesPerRoom);
            }

            return enemiesToDistribute;
        }

        public void EnqueueChildrenRooms(Queue<Room> toVisit)
        {
            foreach (var room in GetChildren())
            {
                if (room != null)
                {
                    toVisit.Enqueue(room);
                }
            }
        }
    }
}