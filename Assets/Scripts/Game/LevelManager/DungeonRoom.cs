using System;
using System.Collections.Generic;
using Game.GameManager;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using ScriptableObjects;
using UnityEngine;
using Util;

namespace Game.LevelManager
{
    [Serializable]
    public class DungeonRoom : DungeonPart
    {
        protected Dimensions dimensions; // inicializar valores antes de acessar os tiles
        private int[,] tiles = null;
        [SerializeField]
        private List<int> keyIDs;
        [SerializeField]
        protected int treasure;
        private RoomBhv roomBHV;
        [SerializeField]
        private int npcID;
        [SerializeField]
        private EnemiesByType _enemiesByType;
        [SerializeField]
        private int _totalEnemies;

        public DungeonRoom(Coordinates coordinates, string code, List<int> keyIDs, int treasure, int totalEnemies, int npc) : base(coordinates, code)
        {
            KeyIDs = keyIDs;
            Treasure = treasure;
            TotalEnemies = totalEnemies;
            NpcID = npc;
            EnemiesByType = null;
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
                    if (room.Tiles[ix, iy] == (int) Enums.TileTypes.Floor)
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

        public void CreateRoom(Dimensions roomDimensions)
        {
            Dimensions = roomDimensions;
            InitializeTiles(); // aloca memória para os tiles
            int roomType = RandomSingleton.GetInstance().Random.Next((int)Enums.RoomTypes.COUNT);
            DefaultRoomCreator.CreateRoomOfType(this, roomType);
        }

        public int TotalEnemies 
        {
            get => _totalEnemies;
            set => _totalEnemies = value;
        }
        public EnemiesByType EnemiesByType 
        {
            get => _enemiesByType;
            set => _enemiesByType = value;
        }

        public int Treasure 
        {
            get => treasure; 
            set => treasure = value;
        }

        public Dimensions Dimensions 
        {
            get => dimensions; 
            set => dimensions = value;
        }

        public int[,] Tiles
        {
            get => tiles; 
            set => tiles = value;
        }

        public List<int> KeyIDs
        {
            get => keyIDs; 
            set => keyIDs = value;
        }

        public int NpcID
        {
            get => npcID; 
            set => npcID = value;
        }
    }
}