using Game.LevelManager;
using System;
using System.Collections;
using System.Collections.Generic;
using static Enums;

namespace LevelGenerator
{
    public class Dungeon
    {

        public int nKeys, nLocks, neededLocks;
        public float neededRooms;
        private int desiredKeys;
        private float avgChildren;
        public Boundaries boundaries;
        public Dimensions dimensions;
        //Queue with the notes that need to be visited
        public Queue toVisit;
        //List of Nodes (easier to add neighbors)
        private List<Room> roomList;
        //Room Grid, where a reference to all the existing room will be maintained for quick access when creating nodes
        public RoomGrid roomGrid;
        public JSonWriter.ParametersMonsters parametersMonsters;
        public JSonWriter.ParametersItems parametersItems;
        public JSonWriter.ParametersNpcs parametersNpcs;
        private string playerProfile;
        private string narrativeName;

        public List<Room> RoomList
        {
            get
            {
                return roomList;
            }

            set
            {
                roomList = value;
            }
        }
        public int DesiredKeys
        {
            get
            {
                return desiredKeys;
            }
            set
            {
                desiredKeys = value;
            }
        }
        public float AvgChildren { get => avgChildren; set => avgChildren = value; }
        public string PlayerProfile { get => playerProfile; set => playerProfile = value; }
        public string NarrativeName { get => narrativeName; set => narrativeName = value; }

        public float fitness;

        public Dungeon()
        {
            toVisit = new Queue();
            RoomList = new List<Room>();
            Room root = RoomFactory.CreateRoot();
            RoomList.Add(root);
            toVisit.Enqueue(root);
            roomGrid = new RoomGrid();
            roomGrid[root.X, root.Y] = root;
            neededRooms = 0;
            neededLocks = 0;
            fitness = -1;
        }

        /*
         * Makes a deep copy of the dungeon, also sets right the parent, children and neighbors of the copied rooms
         * now that grid information is available
         */
        public Dungeon Copy()
        {
            Dungeon copyDungeon = new Dungeon();
            copyDungeon.toVisit = new Queue();
            copyDungeon.roomList = new List<Room>();
            copyDungeon.roomGrid = new RoomGrid();
            copyDungeon.nKeys = nKeys;
            copyDungeon.nLocks = nLocks;
            copyDungeon.desiredKeys = desiredKeys;
            copyDungeon.avgChildren = avgChildren;
            copyDungeon.fitness = fitness;
            Room aux;
            foreach (Room oldRoom in roomList)
            {
                aux = oldRoom.Copy();
                copyDungeon.roomList.Add(aux);
                copyDungeon.roomGrid[oldRoom.X, oldRoom.Y] = aux;
            }
            //Need to use the grid to copy the neighboors, children and parent
            //Checks the position of the node in the grid and then substitutes the old room with the copied one
            foreach (Room newRoom in copyDungeon.roomList)
            {
                if (newRoom.Parent != null)
                {
                    newRoom.Parent = copyDungeon.roomGrid[newRoom.Parent.X, newRoom.Parent.Y];
                }
                if (newRoom.RightChild != null)
                {
                    newRoom.RightChild = copyDungeon.roomGrid[newRoom.RightChild.X, newRoom.RightChild.Y];
                }
                if (newRoom.BottomChild != null)
                {
                    newRoom.BottomChild = copyDungeon.roomGrid[newRoom.BottomChild.X, newRoom.BottomChild.Y];
                }
                if (newRoom.LeftChild != null)
                {
                    newRoom.LeftChild = copyDungeon.roomGrid[newRoom.LeftChild.X, newRoom.LeftChild.Y];
                }
            }
            return copyDungeon;
        }

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

        public void CalcAvgChildren()
        {
            avgChildren = 0.0f;
            int childCount;
            int childLess = 0;
            foreach (Room room in RoomList)
            {
                childCount = 0;
                if (room.RightChild != null && room.RightChild.Parent != null)
                    childCount += 1;
                if (room.LeftChild != null && room.LeftChild.Parent != null)
                    childCount += 1;
                if (room.BottomChild != null && room.BottomChild.Parent != null)
                    childCount += 1;
                if (childCount == 0)
                    childLess++;
                avgChildren += childCount;
            }
            avgChildren /= (RoomList.Count - childLess);
        }

        /*
         *  Instantiates a room and tries to add it as a child of the actual room, considering its direction and position
         *  If there is not a room in the grid at the given coordinates, create the room, add it to the room list
         *  and also enqueue it so it can be explored later
         */
        public void InstantiateRoom(ref Room child, ref Room actualRoom, Util.Direction dir)
        {
            if (actualRoom.ValidateChild(dir, roomGrid))
            {
                child = RoomFactory.CreateRoom();
                //System.Console.WriteLine("Created! ID = " + child.RoomId);
                actualRoom.InsertChild(dir, ref child, ref roomGrid);
                //System.Console.WriteLine("Inserted! ID = " + child.RoomId);
                child.ParentDirection = dir;
                toVisit.Enqueue(child);
                RoomList.Add(child);
                roomGrid[child.X, child.Y] = child;
            }
        }

        /*
         * Removes the nodes that will be taken out of the dungeon from the dungeon's grid
         */
        public void RemoveFromGrid(Room cut)
        {
            if (cut != null)
            {
                roomGrid[cut.X, cut.Y] = null;
                roomList.Remove(cut);
                if (cut.LeftChild != null && cut.LeftChild.Parent != null && cut.LeftChild.Parent.Equals(cut))
                {
                    RemoveFromGrid(cut.LeftChild);
                }
                if (cut.BottomChild != null && cut.BottomChild.Parent != null && cut.BottomChild.Parent.Equals(cut))
                {
                    RemoveFromGrid(cut.BottomChild);
                }

                if (cut.RightChild != null && cut.RightChild.Parent != null && cut.RightChild.Parent.Equals(cut))
                {
                    RemoveFromGrid(cut.RightChild);
                }
            }
        }
        /*
         * Updates the grid from the dungeon with the position of all the new rooms based on the new rotation of the traded room
         * If a room already exists in the grid, "ignores" all the children node of this room
         */
        public void RefreshGrid(ref Room newRoom)
        {
            bool hasInserted;
            if (newRoom != null)
            {
                roomGrid[newRoom.X, newRoom.Y] = newRoom;
                roomList.Add(newRoom);
                Room aux = newRoom.LeftChild;
                if (aux != null && aux.Parent != null && aux.Parent.Equals(newRoom))
                {
                    hasInserted = newRoom.ValidateChild(Util.Direction.left, roomGrid);
                    if (hasInserted)
                    {
                        newRoom.InsertChild(Util.Direction.left, ref aux, ref roomGrid);
                        RefreshGrid(ref aux);
                    }
                    else
                    {
                        newRoom.LeftChild = null;
                    }
                }
                aux = newRoom.BottomChild;
                if (aux != null && aux.Parent != null && aux.Parent.Equals(newRoom))
                {
                    hasInserted = newRoom.ValidateChild(Util.Direction.down, roomGrid);
                    if (hasInserted)
                    {
                        newRoom.InsertChild(Util.Direction.down, ref aux, ref roomGrid);
                        RefreshGrid(ref aux);
                    }
                    else
                    {
                        newRoom.BottomChild = null;
                    }
                }
                aux = newRoom.RightChild;
                if (aux != null && aux.Parent != null && aux.Parent.Equals(newRoom))
                {
                    hasInserted = newRoom.ValidateChild(Util.Direction.right, roomGrid);
                    if (hasInserted)
                    {
                        newRoom.InsertChild(Util.Direction.right, ref aux, ref roomGrid);
                        RefreshGrid(ref aux);
                    }
                    else
                        newRoom.RightChild = null;
                }
            }
        }

        /*
         * While a node (room) has not been visited, or while the max depth of the tree has not been reached, visit each node and create its children
         */
        public void GenerateRooms()
        {
            int prob;
            while (toVisit.Count > 0)
            {
                Room actualRoom = toVisit.Dequeue() as Room;
                int actualDepth = actualRoom.Depth;
                //If max depth allowed has been reached, stop creating children
                if (actualDepth > Constants.MAX_DEPTH)
                {
                    toVisit.Clear();
                    break;
                }
                //Check how many children the node will have, if any.
                prob = Util.rnd.Next(100);
                //Console.WriteLine(prob);
                //The parent node has 100% chance to have children, then, at each height, 10% of chance to NOT have children is added.
                //If a node has a child, create it with the RoomFactory, insert it as a child of the actual node in the right place
                //Also enqueue it to be visited later and add it to the list of the rooms of this dungeon
                if (prob <= (Constants.PROB_HAS_CHILD * (1 - actualDepth / (Constants.MAX_DEPTH + 1))))
                {
                    Room child = null;
                    Util.Direction dir = (Util.Direction)Util.rnd.Next(3);
                    prob = Util.rnd.Next(101);

                    if (prob < Constants.PROB_1_CHILD)
                    {
                        InstantiateRoom(ref child, ref actualRoom, dir);
                    }
                    else if (prob < (Constants.PROB_1_CHILD + Constants.PROB_2_CHILD))
                    {
                        InstantiateRoom(ref child, ref actualRoom, dir);
                        Util.Direction dir2;
                        do
                        {
                            dir2 = (Util.Direction)Util.rnd.Next(3);
                        } while (dir == dir2);
                        InstantiateRoom(ref child, ref actualRoom, dir2);
                    }
                    else
                    {
                        InstantiateRoom(ref child, ref actualRoom, Util.Direction.right);
                        InstantiateRoom(ref child, ref actualRoom, Util.Direction.down);
                        InstantiateRoom(ref child, ref actualRoom, Util.Direction.left);
                    }
                }
            }
            nKeys = RoomFactory.AvailableLockId.Count + RoomFactory.UsedLockId.Count;
            nLocks = RoomFactory.UsedLockId.Count;
        }
        /*
         * Recreates the room list by visiting all the rooms in the tree and adding them to the list while also counting the number of locks and keys
         **/
        public void FixRoomList()
        {
            Queue<Room> toVisit = new Queue<Room>();
            toVisit.Enqueue(roomList[0]);
            nKeys = 0;
            nLocks = 0;
            roomList.Clear();
            while (toVisit.Count > 0)
            {
                Room actualRoom = toVisit.Dequeue() as Room;
                roomList.Add(actualRoom);
                if (actualRoom.Type == Type.key)
                    nKeys++;
                else if (actualRoom.Type == Type.locked)
                    nLocks++;
                if (actualRoom.LeftChild != null)
                    toVisit.Enqueue(actualRoom.LeftChild);
                if (actualRoom.BottomChild != null)
                    toVisit.Enqueue(actualRoom.BottomChild);
                if (actualRoom.RightChild != null)
                    toVisit.Enqueue(actualRoom.RightChild);
            }
        }
        /*
         * Fixes a dungeons after crossover and mutation
         * Just edit the room types using a breadth-first search algorithm with a similar algorithm as the one used
         * to create the rooms
         */
        public void FixIndividual()
        {
            Room actualRoom;
            Room child;
            actualRoom = roomList[0];
            toVisit.Clear();
            toVisit.Enqueue(actualRoom);
            RoomFactory.AvailableLockId.Clear();
            RoomFactory.UsedLockId.Clear();

            while (toVisit.Count > 0)
            {
                actualRoom = toVisit.Dequeue() as Room;

                child = actualRoom.LeftChild;
                if (child != null && actualRoom.Equals(child.Parent))
                {
                    RoomFactory.RecreateRoom(ref child, desiredKeys);
                    toVisit.Enqueue(child);
                }
                child = actualRoom.BottomChild;
                if (child != null && actualRoom.Equals(child.Parent))
                {
                    RoomFactory.RecreateRoom(ref child, desiredKeys);
                    toVisit.Enqueue(child);
                }
                child = actualRoom.RightChild;
                if (child != null && actualRoom.Equals(child.Parent))
                {
                    RoomFactory.RecreateRoom(ref child, desiredKeys);
                    toVisit.Enqueue(child);
                }

            }
            nKeys = RoomFactory.AvailableLockId.Count + RoomFactory.UsedLockId.Count;
            nLocks = RoomFactory.UsedLockId.Count;
        }

        /*
         * Add lock and key
         */
        public void AddLockAndKey()
        {
            Room actualRoom;
            Room child;
            actualRoom = roomList[0];
            toVisit.Clear();
            toVisit.Enqueue(actualRoom);
            bool hasKey = false;
            bool hasLock = false;
            int lockId = -1;

            while (toVisit.Count > 0 && !hasLock)
            {
                actualRoom = toVisit.Dequeue() as Room;
                if (actualRoom.Type == Type.normal && !actualRoom.Equals(roomList[0]) && (Util.rnd.Next(101) <= (Constants.PROB_KEY_ROOM + Constants.PROB_LOCKER_ROOM)))
                {
                    if (!hasKey)
                    {
                        actualRoom.Type = Type.key;
                        actualRoom.RoomId = Util.getNextId();
                        actualRoom.KeyToOpen = actualRoom.RoomId;
                        lockId = actualRoom.RoomId;
                        hasKey = true;
                        roomGrid[actualRoom.X, actualRoom.Y] = actualRoom;
                    }
                    else
                    {
                        actualRoom.Type = Type.locked;
                        actualRoom.KeyToOpen = lockId;
                        hasLock = true;
                        roomGrid[actualRoom.X, actualRoom.Y] = actualRoom;
                    }
                }
                child = actualRoom.LeftChild;
                if (child != null && actualRoom.Equals(child.Parent))
                {
                    toVisit.Enqueue(child);
                }
                child = actualRoom.BottomChild;
                if (child != null && actualRoom.Equals(child.Parent))
                {
                    toVisit.Enqueue(child);
                }
                child = actualRoom.RightChild;
                if (child != null && actualRoom.Equals(child.Parent))
                {
                    toVisit.Enqueue(child);
                }

            }
        }

        /*
         * Add lock and key
         */
        public void RemoveLockAndKey()
        {
            int removeKey = Util.rnd.Next(nKeys);
            int removeLock = removeKey;
            Room actualRoom;
            Room child;
            actualRoom = roomList[0];
            toVisit.Clear();
            toVisit.Enqueue(actualRoom);
            bool hasKey = false;
            bool hasLock = false;
            int lockId = -1;
            int keyCount = 0;

            foreach (Room r in roomList)
            {
                if (r.Type == Type.key)
                {
                    if (removeKey == keyCount)
                        lockId = r.RoomId;
                    keyCount++;
                }
            }
            //Console.WriteLine("Searching Id:" + lockId);
            while ((toVisit.Count > 0) && (!hasLock || !hasKey))
            {
                actualRoom = toVisit.Dequeue() as Room;
                if (actualRoom.Type == Type.key && actualRoom.RoomId == lockId)
                {
                    actualRoom.Type = Type.normal;
                    actualRoom.KeyToOpen = -1;
                    lockId = actualRoom.RoomId;
                    roomGrid[actualRoom.X, actualRoom.Y] = actualRoom;
                    hasKey = true;
                }
                child = actualRoom.LeftChild;
                if (child != null && actualRoom.Equals(child.Parent))
                {
                    toVisit.Enqueue(child);
                }
                child = actualRoom.BottomChild;
                if (child != null && actualRoom.Equals(child.Parent))
                {
                    toVisit.Enqueue(child);
                }
                child = actualRoom.RightChild;
                if (child != null && actualRoom.Equals(child.Parent))
                {
                    toVisit.Enqueue(child);
                }

            }

            actualRoom = roomList[0];
            toVisit.Clear();
            toVisit.Enqueue(actualRoom);

            while ((toVisit.Count > 0) && !hasLock)
            {
                actualRoom = toVisit.Dequeue() as Room;
                if (actualRoom.Type == Type.locked && actualRoom.KeyToOpen == lockId)
                {
                    actualRoom.Type = Type.normal;
                    actualRoom.KeyToOpen = -1;
                    hasLock = true;
                }
                child = actualRoom.LeftChild;
                if (child != null && actualRoom.Equals(child.Parent))
                {
                    toVisit.Enqueue(child);
                }
                child = actualRoom.BottomChild;
                if (child != null && actualRoom.Equals(child.Parent))
                {
                    toVisit.Enqueue(child);
                }
                child = actualRoom.RightChild;
                if (child != null && actualRoom.Equals(child.Parent))
                {
                    toVisit.Enqueue(child);
                }

            }

            nKeys -= Convert.ToInt32(hasKey);
            nLocks -= Convert.ToInt32(hasLock);
        }

        public void SetFitness(float fitness_)
        {
            fitness = fitness_;
        }

        public void SetBoundariesFromRoomList()
        {
            int minX, minY, maxX, maxY;
            minX = Constants.MATRIXOFFSET;
            minY = Constants.MATRIXOFFSET;
            maxX = -Constants.MATRIXOFFSET;
            maxY = -Constants.MATRIXOFFSET;

            foreach (Room room in roomList)
            {
                if (room.X < minX)
                {
                    minX = room.X;
                }

                if (room.Y < minY)
                {
                    minY = room.Y;
                }

                if (room.X > maxX)
                {
                    maxX = room.X;
                }

                if (room.Y > maxY)
                {
                    maxY = room.Y;
                }
            }
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
    }

}