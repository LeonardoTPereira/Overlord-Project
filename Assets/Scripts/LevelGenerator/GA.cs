using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace LevelGenerator
{
    class GA
    {
        private readonly object dfsLock = new object();
        private readonly object avgRoomLock = new object();
        public enum MutationOp
        {
            insertChild = 0,
            removeLeaf = 1
        };
        /**
         * Search the tree of rooms to find the number of special rooms. 
         * The key room is saved in the list with its positive ID, while the locked room with its negative value of the ID
         */
        static private void FindNKLR(ref int nRooms, ref List<int> specialRooms, Room root)
        {
            Queue<Room> toVisit = new Queue<Room>();
            specialRooms = new List<int>();
            toVisit.Enqueue(root);
            nRooms = 0;
            while (toVisit.Count > 0)
            {
                nRooms++;
                Room actualRoom = toVisit.Dequeue() as Room;
                Type type;
                type = actualRoom.Type;
                if (type == Type.key)
                    specialRooms.Add(actualRoom.KeyToOpen);
                else if (type == Type.locked)
                    specialRooms.Add(-actualRoom.KeyToOpen);
                if (actualRoom.LeftChild != null)
                    toVisit.Enqueue(actualRoom.LeftChild);
                if (actualRoom.BottomChild != null)
                    toVisit.Enqueue(actualRoom.BottomChild);
                if (actualRoom.RightChild != null)
                    toVisit.Enqueue(actualRoom.RightChild);
            }
        }

        /**
         * Changes the selected rooms between the parent dungeons
         * To do so, changes in the parent who is their child to the correspondingo node
         */
        static public void ChangeChildren(ref Room cut1, ref Room cut2)
        {
            Room parent = cut1.Parent;
            //Check which child is the cut Room (Right, Bottom, Left)
            try
            {
                switch (cut1.ParentDirection)
                {
                    case Util.Direction.right:
                        parent.RightChild = cut2;
                        break;
                    case Util.Direction.down:
                        parent.BottomChild = cut2;
                        break;
                    case Util.Direction.left:
                        parent.LeftChild = cut2;
                        break;
                    default:
                        Debug.Log("Something went wrong in crossover!.\n");
                        Debug.Log("Direction not supported:\n\tOnly Right, Down and Left are allowed.\n\n");
                        break;
                }
            }
            catch(System.Exception e)
            {
                Debug.Log("Something went wrong while changing the children!");
                throw e;
            }
        }
        /*
         * Choose a random room to switch between the parents and arrange every aspect of the room needed after the change
         * Including the grid, and also the exceptions where the new nodes overlap the old ones
         */
        //static public void Crossover(ref Dungeon ind1, ref Dungeon ind2, ref Dungeon child1, ref Dungeon child2)
        static public void Crossover(ref Dungeon indOriginal1, ref Dungeon indOriginal2)
        {
            Dungeon ind1, ind2;
            //The root of the branch that will be traded
            Room roomCut1, roomCut2;
            //List of rooms that were the root of the branch and led to an impossible crossover (Tabu List)
            List<Room> failedRooms;
            int prob = Util.rnd.Next(100);
            //List of special rooms in the branche to be traded of each parent
            List<int> specialRooms1 = new List<int>(), specialRooms2 = new List<int>();
            //List of special rooms in the traded brach after the crossover
            List<int> newSpecial1 = new List<int>(), newSpecial2 = new List<int>();
            //Total number of rooms in each branch that will be traded
            int nRooms1 = 0, nRooms2 = 0;
            //Answers if the trade is possible or not
            bool isImpossible = false;
            if (prob < Constants.CROSSOVER_RATE)
            {
                //System.Console.WriteLine("StartCross");
                do
                {
                    ind1 = indOriginal1.Copy();
                    ind2 = indOriginal2.Copy();
                    //Not used anymore
                    //ind1.DesiredKeys = (ind1.nKeys + ind2.nKeys) / 2;
                    //ind2.DesiredKeys = (ind1.nKeys + ind2.nKeys) / 2;
                    //Get a random node from each parent
                                        /*System.Console.WriteLine("Ind1 size:" + ind1.RoomList.Count);
                    System.Console.WriteLine("Ind1 Room1:" + aux.RoomId);
                    if(aux.RightChild != null)
                        System.Console.WriteLine("Ind1 RoomRC:" + aux.RightChild.RoomId);
                    if (aux.BottomChild != null)
                        System.Console.WriteLine("Ind1 RoomBC:" + aux.BottomChild.RoomId);
                    if (aux.LeftChild != null)
                        System.Console.WriteLine("Ind1 RoomLC:" + aux.LeftChild.RoomId);*/

                    //Get a random node from the parent, find the number of keys, locks and rooms and add it to the list of future failed rooms
                    roomCut1 = ind1.RoomList[Util.rnd.Next(1, ind1.RoomList.Count)];
                    FindNKLR(ref nRooms1, ref specialRooms1, roomCut1);
                    failedRooms = new List<Room>();
                    //System.Console.WriteLine("Ind2 size:" + ind2.RoomList.Count);

                    //While the number of Keys and Locks from a branch is greater than the number of rooms of the other branch,
                    //Redraw the cut point (root of the branch).
                    //System.Console.WriteLine("STARTFINDINGCUT");
                    do
                    {
                        do
                        {
                            roomCut2 = ind2.RoomList[Util.rnd.Next(1, ind2.RoomList.Count)];
                        } while (failedRooms.Contains(roomCut2));
                        failedRooms.Add(roomCut2);
                        if (failedRooms.Count == ind2.RoomList.Count - 1)
                            isImpossible = true;
                        FindNKLR(ref nRooms2, ref specialRooms2, roomCut2);
                    } while ((specialRooms2.Count > nRooms1 || specialRooms1.Count > nRooms2) && !isImpossible);
                    //System.Console.WriteLine("STOPFINDINGCUT");
                    //System.Console.WriteLine("Crossed Over");
                    
                    //Changes the children of the parent's and neighbor's nodes to the node of the other branch if it is not an impossible trade
                    if (!isImpossible)
                    {
                        //System.Console.WriteLine("NOTIMPOSSIBLE");
                        /*System.Console.WriteLine("CUT1: "+roomCut1.RoomId);
                        System.Console.WriteLine("CUT2: " + roomCut2.RoomId);
                        System.Console.WriteLine("PRE CUT");
                        System.Console.WriteLine("Parent1 Children:");
                        if(roomCut1.Parent.BottomChild!=null)
                            System.Console.WriteLine("\tBottom: "+roomCut1.Parent.BottomChild.RoomId);
                        if (roomCut1.Parent.LeftChild != null)
                            System.Console.WriteLine("\tLeft: " + roomCut1.Parent.LeftChild.RoomId);
                        if (roomCut1.Parent.RightChild != null)
                            System.Console.WriteLine("\tRight: " + roomCut1.Parent.RightChild.RoomId);
                        System.Console.WriteLine("Parent2 Children:");
                        if (roomCut2.Parent.BottomChild != null)
                            System.Console.WriteLine("\tBottom: " + roomCut2.Parent.BottomChild.RoomId);
                        if (roomCut2.Parent.LeftChild != null)
                            System.Console.WriteLine("\tLeft: " + roomCut2.Parent.LeftChild.RoomId);
                        if (roomCut2.Parent.RightChild != null)
                            System.Console.WriteLine("\tRight: " + roomCut2.Parent.RightChild.RoomId);*/

                        try
                        {
                            ChangeChildren(ref roomCut1, ref roomCut2);
                            ChangeChildren(ref roomCut2, ref roomCut1);
                        }
                        catch (System.Exception e)
                        {
                            throw e;
                        }


                        /*System.Console.WriteLine("POST CUT");
                        System.Console.WriteLine("Parent1 Children:");
                        if (roomCut1.Parent.BottomChild != null)
                            System.Console.WriteLine("\tBottom: " + roomCut1.Parent.BottomChild.RoomId);
                        if (roomCut1.Parent.LeftChild != null)
                            System.Console.WriteLine("\tLeft: " + roomCut1.Parent.LeftChild.RoomId);
                        if (roomCut1.Parent.RightChild != null)
                            System.Console.WriteLine("\tRight: " + roomCut1.Parent.RightChild.RoomId);
                        System.Console.WriteLine("Parent2 Children:");
                        if (roomCut2.Parent.BottomChild != null)
                            System.Console.WriteLine("\tBottom: " + roomCut2.Parent.BottomChild.RoomId);
                        if (roomCut2.Parent.LeftChild != null)
                            System.Console.WriteLine("\tLeft: " + roomCut2.Parent.LeftChild.RoomId);
                        if (roomCut2.Parent.RightChild != null)
                            System.Console.WriteLine("\tRight: " + roomCut2.Parent.RightChild.RoomId);*/

                        //Change the parent of each node
                        Room auxRoom;
                        //Changes the parents of the chosen nodes
                        /*System.Console.WriteLine("\nPRE CUT");
                        System.Console.WriteLine("CUT1 Parent:" + roomCut1.Parent.RoomId);
                        System.Console.WriteLine("CUT2 Parent:" + roomCut2.Parent.RoomId);*/
                        auxRoom = roomCut1.Parent;
                        roomCut1.Parent = roomCut2.Parent;
                        roomCut2.Parent = auxRoom;


                        /*System.Console.WriteLine("POST CUT");
                        System.Console.WriteLine("CUT1 Parent:" + roomCut1.Parent.RoomId);
                        System.Console.WriteLine("CUT2 Parent:" + roomCut2.Parent.RoomId);*/

                        /*System.Console.WriteLine("\nPRE CUT");
                        System.Console.WriteLine("DUNGEON1:");
                        Interface.PrintNumericalGridWithConnections(ind1);
                        System.Console.WriteLine("DUNGEON2");
                        Interface.PrintNumericalGridWithConnections(ind2);*/
                        
                        //Remove the node and their children from the grid of the old dungeon
                        ind1.RemoveFromGrid(roomCut1);
                        ind2.RemoveFromGrid(roomCut2);

                        //TODO: REMOVE THIS LATER!!!
                        //ind1.FixRoomList();
                        //ind2.FixRoomList();

                        /*System.Console.WriteLine("\nPOST CUT");
                        System.Console.WriteLine("DUNGEON1:");
                        Interface.PrintNumericalGridWithConnections(ind1);
                        System.Console.WriteLine("DUNGEON2");
                        Interface.PrintNumericalGridWithConnections(ind2);*/

                        //Update the position, parent's direction and rotation of both nodes that are switched
                        int x = roomCut1.X;
                        int y = roomCut1.Y;
                        Util.Direction dir = roomCut1.ParentDirection;
                        int rotation = roomCut1.Rotation;
                        roomCut1.X = roomCut2.X;
                        roomCut1.Y = roomCut2.Y;
                        roomCut1.ParentDirection = roomCut2.ParentDirection;
                        roomCut1.Rotation = roomCut2.Rotation;
                        roomCut2.X = x;
                        roomCut2.Y = y;
                        roomCut2.ParentDirection = dir;
                        roomCut2.Rotation = rotation;

                        //Updates the grid with all the new nodes. If any conflicts arise, handle them as in the child creation.
                        //That is, any overlap will make the node and its children cease to exist 
                        ind1.RefreshGrid(ref roomCut2);
                        ind2.RefreshGrid(ref roomCut1);

                        //ind1.FixRoomList();
                        //ind2.FixRoomList();

                        /*System.Console.WriteLine("\nPOST REFRESH");
                        System.Console.WriteLine("DUNGEON1:");
                        Interface.PrintNumericalGridWithConnections(ind1);
                        System.Console.WriteLine("DUNGEON2");
                        Interface.PrintNumericalGridWithConnections(ind2);*/
                        //System.Console.WriteLine("REFRESHEDGRIDS");

                        //Find the number of keys, locks and rooms in the newly switched branches
                        newSpecial1 = new List<int>();
                        newSpecial2 = new List<int>();
                        FindNKLR(ref nRooms2, ref newSpecial2, roomCut2);
                        FindNKLR(ref nRooms1, ref newSpecial1, roomCut1);
                    }
                    //If in the new branches there are special rooms missing or the number of special rooms is greater then the number of total rooms, retry
                } while ((newSpecial1.Count != specialRooms1.Count || newSpecial2.Count != specialRooms2.Count || specialRooms1.Count > nRooms2 || specialRooms2.Count > nRooms1) && !isImpossible);
                //System.Console.WriteLine("CUTSOK");
                //If the crossover can be done, do it. If not, don't.
                //System.Console.WriteLine("Fixing");
                if (!isImpossible)
                {
                    //Replace locks and keys in the new branches
                    roomCut2.FixBranch(specialRooms1);
                    roomCut1.FixBranch(specialRooms2);
                    //System.Console.WriteLine("FIXEDBRANCHES");
                    //Fix the list of rooms
                    ind1.FixRoomList();
                    ind2.FixRoomList();

                    //System.Console.WriteLine("FIXEDROOMLIST");
                    //Make a copy of the individual and finish the crossover

                    
                }
                indOriginal1 = ind1.Copy();
                indOriginal2 = ind2.Copy();
                //System.Console.WriteLine("COPIED");
                //System.Console.WriteLine("Fixed Branch");
            }
        }
        
        public static void Mutation(ref Dungeon individual)
        {
            try
            {
                //Mutate keys, adding or removing a pair
                bool willMutate = Util.rnd.Next(101) <= Constants.MUTATION_RATE;
                MutationOp op;
                if (willMutate)
                {
                    op = Util.rnd.Next(101) <= Constants.MUTATION0_RATE ? MutationOp.insertChild : MutationOp.removeLeaf;
                    switch (op)
                    {
                        case MutationOp.insertChild:
                            individual.AddLockAndKey();
                            break;
                        case MutationOp.removeLeaf:
                            individual.RemoveLockAndKey();
                            break;  
                    }
                    individual.FixRoomList();
                }
                //foreach(Room room in individual.RoomList){
                /*for (int i = 0; i < individual.RoomList.Count; ++i)
                {
                    Room room = individual.RoomList[i];

                    willMutate = Util.rnd.Next(101) <= Constants.MUTATION_RATE;
                    if (willMutate)
                    {
                        op = Util.rnd.Next(101) <= Constants.MUTATION0_RATE ? MutationOp.insertChild : MutationOp.removeLeaf;
                        switch (op)
                        {
                            case MutationOp.insertChild:
                                if (room.LeftChild != null && room.BottomChild != null && room.RightChild != null) continue;
                                bool found = false;
                                Util.Direction dir;
                                do
                                {
                                    dir = (Util.Direction)Util.rnd.Next(3);
                                    if (dir == Util.Direction.left && room.LeftChild == null) found = true;
                                    else if (dir == Util.Direction.down && room.BottomChild == null) found = true;
                                    else if (dir == Util.Direction.right && room.RightChild == null) found = true;
                                } while (!found);

                                if (room.ValidateChild(dir, individual.roomGrid))
                                {
                                    //System.Console.WriteLine("Insertion!");
                                    Room child = new Room();
                                    room.InsertChild(dir, ref child, ref individual.roomGrid);
                                    child.ParentDirection = dir;
                                    individual.RoomList.Add(child);
                                    individual.roomGrid[child.X, child.Y] = child;
                                }
                                break;
                            case MutationOp.removeLeaf:
                                if (room.LeftChild == null && room.RightChild == null && room.BottomChild == null && room.Type == Type.normal) // It's a leaf node, remove it from the dungeon
                                {
                                    dir = room.ParentDirection;
                                    switch (dir)
                                    {
                                        case Util.Direction.right:
                                            room.Parent.RightChild = null;
                                            break;
                                        case Util.Direction.left:
                                            room.Parent.LeftChild = null;
                                            break;
                                        case Util.Direction.down:
                                            room.Parent.BottomChild = null;
                                            break;
                                    }
                                    individual.RemoveFromGrid(room);
                                    individual.RoomList.Remove(room);
                                }
                                break;
                        }
                    }
                }*/
            
            }
            catch (System.Exception e)
            {
                throw e;
            }



            //Mutation(ref individual.LeftChild);
            //Mutation(ref individual.BottomChild);
            //Mutation(ref individual.RightChild);
        }
        static public void Tournament(List<Dungeon>pop, ref int parent1, ref int parent2)
        {
            HashSet<int> posHash = new HashSet<int>();
            List<int> parentPosL = new List<int>();
            do
            {
                int pos = Util.rnd.Next(pop.Count);
                if (posHash.Add(pos))
                {
                    parentPosL.Add(pos);
                }
            } while (posHash.Count != 4);
            parent1 = pop[parentPosL[0]].fitness < pop[parentPosL[1]].fitness ? parentPosL[0] : parentPosL[1];
            parent2 = pop[parentPosL[2]].fitness < pop[parentPosL[3]].fitness ? parentPosL[2] : parentPosL[3];
        }
        //Fitness is based in the number of rooms, number of keys and locks, the linear coefficient and the number of locks used by the A*
        public float Fitness(Dungeon ind, int nV, int nK, int nL, float lCoef)
        {
            
            float avgUsedRoom = 0.0f;
            DFS[] dfs = new DFS[3];
            //Only use the A* if there is a lock in the dungeon
            if (ind.nLocks > 0)
            {
                //System.Console.WriteLine("Begin A*");
                ind.neededLocks = AStar.FindRoute(ind);
                Parallel.For(0, 3, (i) =>
                {
                    
                    lock (dfsLock)
                    {
                        dfs[i] = new DFS(ind);
                    }
                    dfs[i].FindRoute();
                    lock (avgRoomLock)
                    {
                        avgUsedRoom += dfs[i].NVisitedRooms;
                    }
                });
                /*for (int i = 0; i < 3; ++i)
                {
                    dfs[i] = new DFS(ind);
                    
                    dfs[i].FindRoute();
                    avgUsedRoom += dfs[i].NVisitedRooms;
                }*/
                ind.neededRooms = avgUsedRoom / 3.0f;
                //System.Console.WriteLine("Needed Rooms: " + ind.neededRooms);
                if (ind.neededRooms > ind.RoomList.Count)
                {
                    Debug.Log("SOMETHING IS REALLY WRONG! Nrooms: " + ind.RoomList.Count + "  Used: " + ind.neededRooms);
                    System.Console.ReadKey();
                }
                if (ind.neededLocks > ind.nLocks)
                    Debug.Log("SOMETHING IS REALLY WRONG!");
                return (2*(System.Math.Abs(nV - ind.RoomList.Count) + System.Math.Abs(nK - ind.nKeys) + System.Math.Abs(nL - ind.nLocks) + System.Math.Abs(lCoef - ind.AvgChildren)) + (ind.nLocks - ind.neededLocks) + (ind.RoomList.Count - ind.neededRooms));
            }
            else
                return (2*(System.Math.Abs(nV - ind.RoomList.Count) + System.Math.Abs(nK - ind.nKeys) + System.Math.Abs(nL - ind.nLocks) + System.Math.Abs(lCoef - ind.AvgChildren)));
            //return System.Math.Abs(nV - ind.RoomList.Count) + System.Math.Abs(nK - ind.nKeys) + System.Math.Abs(nL - ind.nLocks) + System.Math.Abs(lCoef - ind.AvgChildren);
        }

    }
}
