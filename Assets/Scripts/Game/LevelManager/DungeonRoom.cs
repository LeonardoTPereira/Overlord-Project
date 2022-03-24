using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using Game.NPCs;
using ScriptableObjects;
using UnityEngine;
using Util;
using Enums = Util.Enums;

namespace Game.LevelManager
{
    [Serializable]
    public class DungeonRoom : DungeonPart
    {
        [SerializeField]
        private List<int> keyIDs;
        [SerializeField]
        protected ItemsAmount items;
        [SerializeField]
        private List<NpcSo> npcs;
        [SerializeField]
        private EnemiesByType enemiesByType;
        [SerializeField]
        private int totalEnemies;

        [field: SerializeField] public bool HasItemPreference { get; set; }

        [field: SerializeField] public bool HasNpcPreference { get; set; }

        public DungeonRoom(Coordinates coordinates, string code, List<int> keyIDs, int treasure, int totalEnemies, int npc) : base(coordinates, code)
        {
            KeyIDs = keyIDs;
            HasItemPreference = treasure > 0;
            TotalEnemies = totalEnemies;
            HasNpcPreference = npc > 0;
            EnemiesByType = null;
            Items = null;
            Npcs = null;
        }

        public void InitializeTiles()
        { // prepara a memória para receber os valores dos tiles
            Tiles = new int[Dimensions.Width, Dimensions.Height];
        }

        public Vector3 GetCenterMostFreeTilePosition()
        {
            Vector3 roomSelfCenter = new Vector3(Dimensions.Width / 2.0f - 0.5f, Dimensions.Height / 2.0f - 0.5f, 0);
            float minSqDist = Mathf.Infinity;
            int minX = 0; //será modificado
            int minY = 0; //será modificado
            for (int ix = 0; ix < Dimensions.Width; ix++)
            {
                for (int iy = 0; iy < Dimensions.Height; iy++)
                {
                    if (Tiles[ix, iy] == (int) Enums.TileTypes.Floor)
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
            return new Vector3(minX, Dimensions.Height - 1 - minY, 0) - roomSelfCenter;
        }
        public Vector3 GetNextAvailablePosition(Vector3 currentPosition)
        {
            var roomSelfCenter = new Vector3(Dimensions.Width / 2.0f - 0.5f, Dimensions.Height / 2.0f - 0.5f, 0);
            var newFreePosition = new Vector3(currentPosition.x, currentPosition.y, 0) + roomSelfCenter;
            do
            {
                newFreePosition.x += 1;
                if (newFreePosition.x <= 3 * Dimensions.Width / (float)4) continue;
                newFreePosition.x = Dimensions.Width/(float)4;
                newFreePosition.y += 1;
            } while (Tiles[(int)newFreePosition.x, (int)newFreePosition.y] != (int) Enums.TileTypes.Floor);

            return new Vector3(newFreePosition.x, Dimensions.Height - 1 - newFreePosition.y, 0) - roomSelfCenter;
        }

        public void CreateRoom(Dimensions roomDimensions)
        {
            Dimensions = roomDimensions;
            InitializeTiles(); // aloca memória para os tiles
            int roomType = RandomSingleton.GetInstance().Random.Next((int)Enums.RoomPatterns.COUNT);
            DefaultRoomCreator.CreateRoomOfType(this, roomType);
        }

        public int TotalEnemies 
        {
            get => totalEnemies;
            set => totalEnemies = value;
        }
        public EnemiesByType EnemiesByType 
        {
            get => enemiesByType;
            set => enemiesByType = value;
        }

        public ItemsAmount Items
        {
            get => items; 
            set => items = value;
        }
        public Dimensions Dimensions { get; set; }
        public int[,] Tiles { get; set; }
        public List<int> KeyIDs
        {
            get => keyIDs; 
            set => keyIDs = value;
        }

        public List<NpcSo> Npcs
        {
            get => npcs; 
            set => npcs = value;
        }
    }
}