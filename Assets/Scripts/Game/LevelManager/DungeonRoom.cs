using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : DungeonPart
{
    protected Dimensions dimensions; // inicializar valores antes de acessar os tiles
    public int[,] tiles = null;
    public List<int> keyIDs;
    protected int difficulty, treasure, enemyType;
    public int Treasure { get => treasure; set => treasure = value; }
    public int Difficulty { get => difficulty; set => difficulty = value; }
    public int EnemyType { get => enemyType; set => enemyType = value; }
    public Dimensions Dimensions { get => dimensions; set => dimensions = value; }
    public RoomBHV roomBHV;

    public DungeonRoom(Coordinates coordinates, string code, List<int> keyIDs, int difficulty, int treasure, int enemyType, int items, int npcs) : base(coordinates, code)
    {        
        this.keyIDs = keyIDs;
        Treasure = treasure;
        Difficulty = difficulty;
        EnemyType = enemyType;
    }

    public void InitializeTiles()
    { // prepara a memória para receber os valores dos tiles
        tiles = new int[dimensions.Width, dimensions.Height];
    }

    public Vector3 GetCenterMostFreeTilePosition()
    {
        GameManager gm = GameManager.instance;
        Transform roomTransf = gm.roomBHVMap[coordinates].transform;
        Vector3 roomSelfCenter = new Vector3(dimensions.Width / 2.0f - 0.5f, dimensions.Height / 2.0f - 0.5f, 0);

        DungeonRoom room = (DungeonRoom)gm.GetMap().dungeonPartByCoordinates[coordinates];
        float minSqDist = Mathf.Infinity;
        int minX = 0; //será modificado
        int minY = 0; //será modificado
                      //Debug.Log("Center: " + roomSelfCenter.x + "," + roomSelfCenter.y);
        for (int ix = 0; ix < dimensions.Width; ix++)
        {
            for (int iy = 0; iy < dimensions.Height; iy++)
            {
                //Debug.Log ("Min Dist: " + minSqDist + "; MinX: " + minX + "; MinY: " + minY);
                if (room.tiles[ix, iy] != 1)
                { //é passável?
                    float sqDist = Mathf.Pow(ix - roomSelfCenter.x, 2) + Mathf.Pow(iy - roomSelfCenter.y, 2);
                    if (sqDist <= minSqDist)
                    {
                        minSqDist = sqDist;
                        minX = ix;
                        minY = iy;
                        //Debug.Log ("NEW! Min Dist: " + minSqDist + "; MinX: " + minX + "; MinY: " + minY);
                    }
                }
            }
        }
        return (new Vector3(minX, dimensions.Height - 1 - minY, 0) - roomSelfCenter);
    }
}

