using System.Collections.Generic;

namespace LevelGenerator
{
    /*
     * The three types a room can have
     * Key is a room containing a key
     * Locker is a room that has a lock
     */
    public enum Type{
        normal = 1 << 0,
        key = 1 << 1,
        locked = 1 << 2
    };
  
    public class Room
    {

        //Room's children
        private Room leftChild = null;
        private Room rightChild = null;
        private Room bottomChild = null;

        private int roomId; //Id of the node based on the sequential identifier
        private int keyToOpen = -1;
        private Type type; //Type of the room

        private int x = 0;
        private int y = 0;
        private int depth = 0;
        //Rotation of the individual's parent position related to the normal cartesian orientation 
        //(with 0 meaning the parent is in the North of the child (Above), 90 being in the East (Right) and so on)
        //Will be later used to construct the grid of the room
        private int rotation = 0;

        private Room parent = null;
        //Saves the direction of the parent (if it is right, bottom or left child)
        //Reduces operations at crossover
        private Util.Direction parentDirection = Util.Direction.down;

        /*
         * Room constructor. The default is a normal room, without a lock so, without a key to open
         * and without a predefined id
         * If a key room defines the key to open as its id, and if a locked one, uses the id of the room which has the key that opens it
         * 
         */
        public Room(Type roomType = Type.normal, int keyToOpen = -1, int id = -1)
        {
            if (id == -1)
                RoomId = Util.getNextId();
            else
                RoomId = id;
            Type = roomType;
            if (Type == Type.key)
                this.KeyToOpen = roomId;
            else if (Type == Type.locked)
                this.KeyToOpen = keyToOpen;
        }

        /*
         * Makes a deep copy of a room.
         * The parent, children and neighboors must be replaced for the right ones in the dungeon's copy method
         */
        public Room Copy()
        {
            Room newRoom = new Room(type, keyToOpen, roomId);
            newRoom.bottomChild = bottomChild;
            newRoom.leftChild = leftChild;
            newRoom.rightChild = rightChild;
            newRoom.depth = depth;
            newRoom.parent = parent;
            newRoom.parentDirection = parentDirection;
            newRoom.rotation = rotation;
            newRoom.x = x;
            newRoom.y = y;

            return newRoom;
        }
        //Fix the newly inserted branch by reinserting in it the special rooms that were in the old branch while maintaining their order of appearence to guarantee the feasibility
        public void FixBranch(List<int> specialRooms)
        {
            Queue<Room> toVisit = new Queue<Room>();
            Queue<Room> visited = new Queue<Room>();
            Queue<int> newSpecialRooms = new Queue<int>();
            int specialId;
            Room actualRoom;
            Room child;
            //The actual room is the root of the branch
            actualRoom = this;
            toVisit.Enqueue(actualRoom);
            //System.Console.WriteLine("Start Conversion");
            //If both lock and keys are in the branch, give them new ids also, add all the special rooms in the new special rooms list
            for(int i = 0; i < specialRooms.Count-1; ++i)
            {
                for(int j = i+1; j < specialRooms.Count; ++j)
                {
                    if(specialRooms[i] == -specialRooms[j])
                    {
                        int aux = Util.getNextId();
                        if (specialRooms[i] > 0)
                            specialRooms[i] = aux;
                        else
                            specialRooms[i] = -aux;
                        specialRooms[j] = -specialRooms[i];
                    }
                }
                newSpecialRooms.Enqueue(specialRooms[i]);
            }
            //Add the last special room, which normally wouldn't be added, but only if it exists
            if(specialRooms.Count > 0)
                newSpecialRooms.Enqueue(specialRooms[specialRooms.Count-1]);

            //Enqueue all the rooms
            while (toVisit.Count > 0)
            {
                actualRoom = toVisit.Dequeue();
                visited.Enqueue(actualRoom);
                child = actualRoom.LeftChild;
                if (child != null)
                    if (actualRoom.Equals(child.Parent))
                    {
                        toVisit.Enqueue(child);
                    }
                child = actualRoom.BottomChild;
                if (child != null)
                    if (actualRoom.Equals(child.Parent))
                    {
                        toVisit.Enqueue(child);
                    }
                child = actualRoom.RightChild;
                if (child != null)
                    if (actualRoom.Equals(child.Parent))
                    {
                        toVisit.Enqueue(child);
                    }
            }

            //try to place all the special rooms in the branch randomly. If the number of remaining rooms is the same as the number of special rooms, every room must be a special one, so we finish this while loop.
            while(visited.Count > newSpecialRooms.Count)
            {
                actualRoom = visited.Dequeue();

                int prob = Util.rnd.Next(101);
                
                //If there is a special room left, check the random number and see if it will be placed in the actual room or not
                if (newSpecialRooms.Count > 0)
                {
                    if (prob < (Constants.PROB_NORMAL_ROOM))
                    {
                        actualRoom.Type = Type.normal;
                        actualRoom.KeyToOpen = -1;
                    }
                    else
                    {
                        /*
                         * If the room has a key, then the key must have an id and this id is added to the list of available keys
                         */
                        specialId = newSpecialRooms.Dequeue();
                        if (specialId > 0)
                        {
                            actualRoom.Type = Type.key;
                            actualRoom.RoomId = specialId;
                            actualRoom.KeyToOpen = specialId;
                        }
                        else
                        {
                            //Creates a locked room with the id of the room that contains the key to open it
                            actualRoom.Type = Type.locked;
                            actualRoom.KeyToOpen = -specialId;
                        }
                    }
                }
                else
                {
                    actualRoom.Type = Type.normal;
                    actualRoom.KeyToOpen = -1;
                }
            }
            //If there are rooms not visited, then all the next rooms must be special ones
            while(visited.Count > 0)
            {
                actualRoom = visited.Dequeue();
                specialId = newSpecialRooms.Dequeue();
                if (specialId > 0)
                {
                    actualRoom.Type = Type.key;
                    actualRoom.RoomId = specialId;
                    actualRoom.KeyToOpen = specialId;
                }
                else
                {
                    //Creates a locked room with the id of the room that contains the key to open it
                    actualRoom.Type = Type.locked;
                    actualRoom.KeyToOpen = -specialId;
                }
            }
            if (newSpecialRooms.Count > 0)
                System.Console.WriteLine("STOOOOOP");
        }
    /*
     * Validates if a child node can be created in the given position or not
     */
    public bool ValidateChild(Util.Direction dir, RoomGrid roomGrid)
        {
            int X, Y;
            Room roomInGrid;
            switch (dir)
            {
                case Util.Direction.right:
                    //Calculates the X and Y based on the parent's rotation
                    if (((this.Rotation / 90) % 2) != 0)
                    {
                        X = this.X;
                        if (this.Rotation == 90)
                            Y = this.Y + 1;
                        else
                            Y = this.Y - 1;
                    }
                    else
                    {
                        if (this.Rotation == 0)
                            X = this.X + 1;
                        else
                            X = this.X - 1;
                        Y = this.Y;
                    }
                    //Checks the grid of room if the room is there, if not, create the room, add it in the grid and
                    //as the actual room's child, returning true
                    //System.Console.WriteLine("X = " + X + " Y = " + Y + "\n");
                    roomInGrid = roomGrid[X, Y];
                    if (roomInGrid == null)
                    {
                        return true;
                    }
                    //If it is in the grid, tries to make a shortcut between the actual room and the existing one
                    //Does not change the child's parent and return false
                    else
                    {
                        return false;
                    }
                case Util.Direction.down:
                    //Calculates the X and Y based on the parent's rotation
                    if (((this.Rotation / 90) % 2) != 0)
                    {
                        Y = this.Y;
                        if (this.Rotation == 90)
                            X = this.X + 1;
                        else
                            X = this.X - 1;
                    }
                    else
                    {
                        if (this.Rotation == 0)
                            Y = this.Y - 1;
                        else
                            Y = this.Y + 1;
                        X = this.X;
                    }
                    //If it is in the grid, tries to make a shortcut between the actual room and the existing one
                    //Does not change the child's parent and return false
                    //System.Console.WriteLine("X = " + X + " Y = " + Y + "\n");
                    roomInGrid = roomGrid[X, Y];
                    if (roomInGrid == null)
                    {
                        return true;
                    }
                    //If it is in the grid, tries to make a shortcut between the actual room and the existing one
                    //Does not change the child's parent and return false
                    else
                    {
                        return false;
                    }

                case Util.Direction.left:
                    //Calculates the X and Y based on the parent's rotation
                    if (((this.Rotation / 90) % 2) != 0)
                    {
                        X = this.X;
                        if (this.Rotation == 90)
                            Y = this.Y - 1;
                        else
                            Y = this.Y + 1;
                    }
                    else
                    {
                        if (this.Rotation == 0)
                            X = this.X - 1;
                        else
                            X = this.X + 1;
                        Y = this.Y;
                    }
                    //If it is in the grid, tries to make a shortcut between the actual room and the existing one
                    //Does not change the child's parent and return false
                    //System.Console.WriteLine("X = " + X + " Y = " + Y + "\n");
                    roomInGrid = roomGrid[X, Y];
                    if (roomInGrid == null)
                    {
                        return true;
                    }
                    //If it is in the grid, tries to make a shortcut between the actual room and the existing one
                    //Does not change the child's parent and return false
                    else
                    {
                        return false;
                    }
                default:
                    System.Console.WriteLine("Something went wrong Creating a Child!\n");
                    System.Console.WriteLine("Direction not supported:\n\tOnly Right, Down and Left are allowed.\n\n");
                    break;
            }
            return false;
        }

        /*
         * For each direction, calculates the X,Y position of the child room based on its rotation and the parent's coordinates
         * and checks if a room already exists in that position. 
         * 
         * If it does, stop creating a room and gives a small chance 
         * for the parent room to adopt the existing room as async shortcut child, 
         * but does not change the child room's original parent. In this case, returns false, 
         * so the existing room will not be added to the visiting rooms queue.
         *
         * If the grid position is empty, create the room in the desired position, with the right position and rotation,
         * set the current room as its parent and returns true so that the new room is added 
         * to the room list and visiting queue
         */
        public void InsertChild(Util.Direction dir, ref Room child, ref RoomGrid roomGrid)
        {
            Room roomInGrid;
            int shortcutChance;
            
            switch (dir)
            {
                case Util.Direction.right:
                    //Calculates the X and Y based on the parent's rotation
                    if (((this.Rotation / 90) % 2)!=0)
                    {
                        child.X = this.X;
                        if (this.Rotation == 90)
                            child.Y = this.Y + 1;
                        else
                            child.Y = this.Y - 1;
                    }
                    else
                    {
                        if (this.Rotation == 0)
                            child.X = this.X + 1;
                        else
                            child.X = this.X - 1;
                        child.Y = this.Y;
                    }
                    //Checks the grid of room if the room is there, if not, create the room, add it in the grid and
                    //as the actual room's child, returning true
                    //System.Console.WriteLine("X = " + child.X + " Y = " + child.Y + "\n");
                    roomInGrid = roomGrid[child.X, child.Y];
                    if (roomInGrid == null)
                    {
                        child.Rotation = (this.Rotation + 90) % 360;
                        RightChild = child;
                        RightChild.SetParent(this);
                    }
                    break;
                case Util.Direction.down:
                    //Calculates the X and Y based on the parent's rotation
                    if (((this.Rotation / 90) % 2) != 0)
                    {
                        child.Y = this.Y;
                        if (this.Rotation == 90)
                            child.X = this.X + 1;
                        else
                            child.X = this.X - 1;
                    }
                    else
                    {
                        if (this.Rotation == 0)
                            child.Y = this.Y - 1;
                        else
                            child.Y = this.Y + 1;
                        child.X = this.X;
                    }
                    //If it is in the grid, tries to make a shortcut between the actual room and the existing one
                    //Does not change the child's parent and return false
                    //System.Console.WriteLine("X = " + child.X + " Y = " + child.Y + "\n");
                    roomInGrid = roomGrid[child.X, child.Y];
                    if (roomInGrid == null)
                    {
                        child.Rotation = (this.Rotation + 90) % 360;
                        BottomChild = child;
                        BottomChild.SetParent(this);
                    }
                    break;
                case Util.Direction.left:
                    //Calculates the X and Y based on the parent's rotation
                    if (((this.Rotation / 90) % 2) != 0)
                    {
                        child.X = this.X;
                        if (this.Rotation == 90)
                            child.Y = this.Y - 1;
                        else
                            child.Y = this.Y + 1;
                    }
                    else
                    {
                        if (this.Rotation == 0)
                            child.X = this.X - 1;
                        else
                            child.X = this.X + 1;
                        child.Y = this.Y;
                    }
                    //If it is in the grid, tries to make a shortcut between the actual room and the existing one
                    //Does not change the child's parent and return false
                    //System.Console.WriteLine("X = "+ child.X+" Y = "+child.Y+"\n");
                    roomInGrid = roomGrid[child.X, child.Y];
                    if (roomInGrid == null)
                    {
                        child.Rotation = (this.Rotation + 90) % 360;
                        LeftChild = child;
                        LeftChild.SetParent(this);
                    }
                    break;
                default:
                    System.Console.WriteLine("Something went wrong Creating a Child!\n");
                    System.Console.WriteLine("Direction not supported:\n\tOnly Right, Down and Left are allowed.\n\n");
                    break;
            }
        }

        public void SetParent(Room parent)
        {
            this.Parent = parent;
            if (parent != null)
            {
                this.depth = ++parent.depth;
            }
        }
        /*
         * Adds all the existing children of a room in a list, and recursively, the children of each child
         */
        public void FindChildren(ref List<Room> roomList)
        {
            if(rightChild!=null)
            {
                if (rightChild.parent!= null && rightChild.parent.Equals(this))
                {
                    roomList.Add(rightChild);
                    rightChild.FindChildren(ref roomList);
                }
            }
            if (bottomChild != null)
            {
                if (bottomChild.parent != null && bottomChild.parent.Equals(this))
                {
                    roomList.Add(bottomChild);
                    bottomChild.FindChildren(ref roomList);
                }
            }
            if (leftChild != null)
            {
                if (leftChild.parent != null && leftChild.parent.Equals(this))
                {
                    roomList.Add(leftChild);
                    leftChild.FindChildren(ref roomList);
                }
            }
        }

        /*
         * Get the child of the room in the given direction
         */
        public Room GetChildByDirection(Util.Direction dir)
        {
            switch (dir)
            {
                case Util.Direction.down:
                    return BottomChild;
                case Util.Direction.left:
                    return LeftChild;
                case Util.Direction.right:
                    return RightChild;
            }
            return null;
        }


        /// <summary>
        /// Check if room is a leaf node by checking if it has any children.
        /// </summary>
        /// <returns>Bool is true if it is a leaf node.</returns>
        public bool IsLeafNode()
        {
            if (bottomChild == null)
                if (leftChild == null)
                    if (rightChild == null)
                        return true;
            return false;
        }

        public int Depth { get => depth; set => depth = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int RoomId { get => roomId; set => roomId = value; }
        public Room Parent { get => parent; set => parent = value; }
        public Room LeftChild { get => leftChild; set => leftChild = value; }
        public Room RightChild { get => rightChild; set => rightChild = value; }
        public Room BottomChild { get => bottomChild; set => bottomChild = value; }
        public Util.Direction ParentDirection { get => parentDirection; set => parentDirection = value; }
        public int Rotation { get => rotation; set => rotation = value; }
        public Type Type { get => type; set => type = value; }
        public int KeyToOpen { get => keyToOpen; set => keyToOpen = value; }
    }
}