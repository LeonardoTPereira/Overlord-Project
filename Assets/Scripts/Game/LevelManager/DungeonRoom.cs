using System.Collections.Generic;
using Game.GameManager;
using UnityEngine;

namespace Game.LevelManager
{
    public class DungeonRoom : DungeonPart
    {
        protected Dimensions dimensions; // inicializar valores antes de acessar os tiles
        private int[,] tiles = null;
        private List<int> keyIDs;
        protected int difficulty, treasure, enemyType;
        public int Treasure { get => treasure; set => treasure = value; }
        public int Difficulty { get => difficulty; set => difficulty = value; }
        public int EnemyType { get => enemyType; set => enemyType = value; }
        public Dimensions Dimensions { get => dimensions; set => dimensions = value; }
        public int[,] Tiles { get => tiles; set => tiles = value; }
        public List<int> KeyIDs { get => keyIDs; set => keyIDs = value; }
        public RoomBHV RoomBHV { get => roomBHV; set => roomBHV = value; }
        public int NpcID { get => npcID; set => npcID = value; }

        private RoomBHV roomBHV;
        private int npcID;

        public DungeonRoom(Coordinates coordinates, string code, List<int> keyIDs, int difficulty, int treasure, int enemyType, int npc) : base(coordinates, code)
        {
            KeyIDs = keyIDs;
            Treasure = treasure;
            Difficulty = difficulty;
            EnemyType = enemyType;
            NpcID = npc;
        }

        public void InitializeTiles()
        { // prepara a memória para receber os valores dos tiles
            Tiles = new int[dimensions.Width, dimensions.Height];
        }

        public Vector3 GetCenterMostFreeTilePosition()
        {
            GameManagerSingleton gm = GameManagerSingleton.instance;
            Vector3 roomSelfCenter = new Vector3(dimensions.Width / 2.0f - 0.5f, dimensions.Height / 2.0f - 0.5f, 0);

            DungeonRoom room = (DungeonRoom)gm.GetMap().DungeonPartByCoordinates[Coordinates];
            float minSqDist = Mathf.Infinity;
            int minX = 0; //será modificado
            int minY = 0; //será modificado
            for (int ix = 0; ix < dimensions.Width; ix++)
            {
                for (int iy = 0; iy < dimensions.Height; iy++)
                {
                    if (room.Tiles[ix, iy] != 1)
                    { //é passável?
                        float sqDist = Mathf.Pow(ix - roomSelfCenter.x, 2) + Mathf.Pow(iy - roomSelfCenter.y, 2);
                        if (sqDist <= minSqDist)
                        {
                            minSqDist = sqDist;
                            minX = ix;
                            minY = iy;
                        }
                    }
                }
            }
            return new Vector3(minX, dimensions.Height - 1 - minY, 0) - roomSelfCenter;
        }
    }
}