using System;
using UnityEngine;

namespace Game.LevelManager.DungeonLoader
{
    [CreateAssetMenu(fileName = "RoomInput", menuName = "RoomGenerator/RoomInput")]
    [Serializable]
    public class RoomGeneratorInput : ScriptableObject
    {
        [field:SerializeField] public Vector2 Size { get; set; }
        [field:SerializeField] public int DoorNorth { get; set; }
        [field:SerializeField] public int DoorSouth { get; set; }
        [field:SerializeField] public int DoorEast { get; set; }
        [field:SerializeField] public int DoorWest { get; set; }

        public bool DoorExists(int doorValue)
        {
            return doorValue >= 0;
        }

        public bool IsNormalDoor(int doorValue)
        {
            return doorValue == 0;
        }
    }
}