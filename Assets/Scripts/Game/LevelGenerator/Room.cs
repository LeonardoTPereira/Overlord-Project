using System;
using System.Collections.Generic;

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
        public int id = -1;
        /// The type of the room.
        public RoomType type = RoomType.Normal;
        /// The ID of the key that opens this room. The ID is equal to -1 when
        /// the room is not locked and does not have a key.
        public int key = -1;
        /// The number of enemies in this room.
        public int enemies = 0;
        /// The depth of the room in the tree. This is used to control the
        /// depth of the dungeon level.
        public int depth = 0;
        /// The x position of the room in the grid.
        public int x = 0;
        /// The y position of the room in the grid.
        public int y = 0;
        /// The rotation of the individual's parent position is related to the
        /// normal cartesian orientation. 0 means that the parent is in the
        /// North (above) of the child, 90 the parent is in the East (right),
        /// and so on. This is used to build the dungeon grid.
        public int rotation = 0;

        /// The room's left child.
        public Room left = null;
        /// The room's bottom child.
        public Room bottom = null;
        /// The room's right child.
        public Room right = null;
        /// The room's parent.
        public Room parent = null;
        /// The direction from what the parent connects with this room.
        /// This attribute reduces operations at crossover.
        public Common.Direction parentDirection = Common.Direction.Down;

        /// Room constructor.
        ///
        /// The default is a normal room without a predefined room ID. The key
        /// room defines its key to open as its room ID. The locked room
        /// defines its key to open as the entered key to open.
        public Room(
            RoomType _type = RoomType.Normal,
            int _key = -1,
            int _id = -1
        ) {
            type = _type;
            id = _id == -1 ? Room.GetNextId() : _id;
            key = type == RoomType.Key ? id : key;
            key = type == RoomType.Locked ? _key : key;
        }

        /// Return a clone this room.
        public Room Clone()
        {
            Room room = new Room(type, key, id);
            room.enemies = enemies;
            room.depth = depth;
            room.x = x;
            room.y = y;
            room.rotation = rotation;
            room.left = left;
            room.bottom = bottom;
            room.right = right;
            room.parent = parent;
            room.parentDirection = parentDirection;
            return room;
        }

        /// Return true if the room is a leaf node.
        public bool IsLeafNode()
        {
            return bottom == null && left == null && right == null;
        }

        /// Return an array with the left, bottom, and right children.
        public Room[] GetChildren()
        {
            return new Room[] {
                    left,
                    bottom,
                    right,
                };
        }

        /// Return an array with all the neighbors (parent and children).
        public Room[] GetNeighbors()
        {
            return new Room[] {
                    parent,
                    left,
                    bottom,
                    right,
                };
        }

        /// Return a tuple corresponding to the position in the dungeon grid of
        /// the child of a parent in the entered direction.
        private (int, int) GetChildPositionInGrid(
            Common.Direction _dir
        ) {
            int cx = 0;
            int cy = 0;
            int rot = (rotation / DEGREE_90) % 2;
            switch (_dir)
            {
                case Common.Direction.Right:
                    if (rot != 0)
                    {
                        cx = x;
                        cy = rotation == DEGREE_90 ? y + 1 : y - 1;
                    }
                    else
                    {
                        cx = rotation == 0 ? x + 1 : x - 1;
                        cy = y;
                    }
                    break;

                case Common.Direction.Down:
                    if (rot != 0)
                    {
                        cx = rotation == DEGREE_90 ? x + 1 : x - 1;
                        cy = y;
                    }
                    else
                    {
                        cx = x;
                        cy = rotation == 0 ? y - 1 : y + 1;
                    }
                    break;

                case Common.Direction.Left:
                    if (rot != 0)
                    {
                        cx = x;
                        cy = rotation == DEGREE_90 ? y - 1 : y + 1;
                    }
                    else
                    {
                        cx = rotation == 0 ? x - 1 : x + 1;
                        cy = y;
                    }
                    break;
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
            (int x, int y) = GetChildPositionInGrid(_dir);
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
            (int x, int y) = GetChildPositionInGrid(_dir);
            _child.x = x;
            _child.y = y;
            _child.rotation = (rotation + DEGREE_90) % DEGREE_360;
            Room room = _grid[x, y];
            if (room != null) { return; }
            switch (_dir)
            {
                case Common.Direction.Right:
                    right = _child;
                    right.parent = this;
                    right.depth = depth + 1;
                    break;
                case Common.Direction.Down:
                    bottom = _child;
                    bottom.parent = this;
                    bottom.depth = depth + 1;
                    break;
                case Common.Direction.Left:
                    left = _child;
                    left.parent = this;
                    left.depth = depth + 1;
                    break;
            }
        }

        /// Fix the branch that starts in this node.
        ///
        /// This method must be called after a successful crossover. This fix
        /// reinserts the mission rooms in the old branch to maintain the
        /// occurring order of missions to guarantee feasibility.
        public void FixBranch(
            List<int> _missions,
            ref Random _rand
        ) {
            // If both lock and key are in the branch, assign to them new IDs,
            // and add all the missions in the new missions list
            Queue<int> newMissions = new Queue<int>();
            for (int i = 0; i < _missions.Count - 1; i++)
            {
                for (int j = i + 1; j < _missions.Count; j++)
                {
                    if (_missions[i] == -_missions[j])
                    {
                        int newId = Room.GetNextId();
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
            Queue<Room> branch = new Queue<Room>();
            Queue<Room> toVisit = new Queue<Room>();
            toVisit.Enqueue(this);
            while (toVisit.Count > 0)
            {
                Room current = toVisit.Dequeue();
                branch.Enqueue(current);
                foreach (Room child in current.GetChildren())
                {
                    if (child != null && current.Equals(child.parent))
                    {
                        toVisit.Enqueue(child);
                    }
                }
            }

            // Place missions in the branch randomly while there are more rooms than missions; otherwise, this while stops
            while (branch.Count > newMissions.Count)
            {
                Room current = branch.Dequeue();
                int prob = Common.RandomPercent(ref _rand);
                // If there are no missions left, then assign the remaining
                // rooms as normal rooms; otherwise, check if the current room
                // will not receive a mission
                if (newMissions.Count == 0 ||
                    RoomFactory.PROB_NORMAL_ROOM > prob
                ) {
                    current.type = RoomType.Normal;
                    current.key = -1;
                }
                else
                {
                    // The current room will receive a mission
                    int missionId = newMissions.Dequeue();
                    // Assign the mission ID to the current room
                    FixMission(ref current, missionId);
                }
            }

            // If new missions are remaining, it means that the number of
            // remaining rooms is the same as the number of mission rooms;
            // thus, place the missions in those rooms
            while (branch.Count > 0 && newMissions.Count > 0)
            {
                // The current room will receive a mission
                Room current = branch.Dequeue();
                int missionId = newMissions.Dequeue();
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
                _room.type = RoomType.Key;
                _room.id = _mission;
                _room.key = _mission;
            }
            else
            {
                _room.type = RoomType.Locked;
                _room.key = -_mission;
            }
        }

        public RoomType Type { get => type; set => type = value; }
        public int Key { get => key; set => key = value; }
        public Room Parent { get => parent; set => parent = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
    }
}