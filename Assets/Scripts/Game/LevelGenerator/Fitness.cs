using LevelGenerator;
using System;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Fitness
{
    [SerializeField, Range(2, 200)]
    protected int desiredRooms;
    [SerializeField, Range(0, 50)]
    protected int desiredKeys, desiredLocks;
    [SerializeField, Range(1.0f, 3.0f)]
    protected float desiredLinearity;

    /*
    * Locks for multi-thread operations
    */
    protected readonly object dfsLock = new object();
    protected readonly object avgRoomLock = new object();

    public int DesiredRooms { get => desiredRooms; set => desiredRooms = value; }
    public int DesiredKeys { get => desiredKeys; set => desiredKeys = value; }
    public int DesiredLocks { get => desiredLocks; set => desiredLocks = value; }
    public float DesiredLinearity { get => desiredLinearity; set => desiredLinearity = value; }

    public Fitness(int desiredRooms, int desiredKeys, int desiredLocks, float desiredLinearity)
    {
        this.desiredRooms = desiredRooms;
        this.desiredKeys = desiredKeys;
        this.desiredLocks = desiredLocks;
        this.desiredLinearity = desiredLinearity;
    }

    /* 
    * Calculates the fitness function
    * Fitness is based in the number of rooms, number of keys and locks, the linear coefficient and the number of locks used by the A* and DFS
    */
    public float CalculateFitness(Dungeon dungeon, int matrixOffset)
    {

        float avgUsedRoom = 0.0f;
        DFS[] dfs = new DFS[3];
        AStar aStar = new AStar();
        //Only use the A* and DFS if there is a lock in the dungeon
        if (dungeon.nLocks > 0)
        {
            //The A* finds the number of locks needed to finish the dungeon using the heuristic that is close to optimal.
            try
            {
                dungeon.neededLocks = aStar.FindRoute(dungeon, matrixOffset);
            }
            catch (UnfeasibleLevelException exception)
            {
                throw;
            }
            //Execute 3 times the DFS to minimize the randomness
            //Execute them in parallel to make things faster
            //The DFS finds the number of rooms needed to finish the dungeon be exploring blindly.
            Parallel.For(0, 3, (i) =>
            {

                lock (dfsLock)
                {
                    dfs[i] = new DFS(dungeon);
                }
                dfs[i].FindRoute();
                lock (avgRoomLock)
                {
                    avgUsedRoom += dfs[i].NVisitedRooms;
                }
            });
            dungeon.neededRooms = avgUsedRoom / 3.0f;

            //If we need more rooms than the rooms that really exist, something is wrong.
            if (dungeon.neededRooms > dungeon.RoomList.Count)
            {
                Debug.Log("SOMETHING IS REALLY WRONG! Nrooms: " + dungeon.RoomList.Count + "  Used: " + dungeon.neededRooms);
                System.Console.ReadKey();
            }
            //Also, if we find more locks that really exist
            if (dungeon.neededLocks > dungeon.nLocks)
                Debug.Log("SOMETHING IS REALLY WRONG!");
            //If everything is ok, return the fitness. We are currently giving 5 times more emphasis to the linearity as a small difference results in very different dungeons
            //We also are trying to make 80% of the rooms needed to finish, the rest are optional.
            return (2 * (System.Math.Abs(DesiredRooms - dungeon.RoomList.Count) + System.Math.Abs(DesiredKeys - dungeon.nKeys) + System.Math.Abs(DesiredLocks - dungeon.nLocks)
                + System.Math.Abs(DesiredLinearity - dungeon.AvgChildren) * 5) + (dungeon.nLocks - dungeon.neededLocks) + System.Math.Abs(dungeon.RoomList.Count * 0.8f - dungeon.neededRooms));
        }
        //If there are no locks, the fitness is based only in the map layout
        else
            return (2 * (System.Math.Abs(DesiredRooms - dungeon.RoomList.Count) + System.Math.Abs(DesiredKeys - dungeon.nKeys) + System.Math.Abs(DesiredLocks - dungeon.nLocks) + System.Math.Abs(DesiredLinearity - dungeon.AvgChildren)));
    }

}