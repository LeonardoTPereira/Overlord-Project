using System;
using Game.EnemyManager;
using Game.LevelManager;
using UnityEditor;
using UnityEngine;
using Util;

namespace Game.DataCollection
{
    [Serializable]
    public class RoomData : ScriptableObject
    {
        //Max theoretical dungeon width for hashing the coordinates into an id
        private static readonly int dungeonWidth = 10000;
        [field: SerializeField] public int RoomId { get; private set; }
        [field: SerializeField] public bool HasEnemies { get; set; }
        [field: SerializeField] public int NEnemies { get; private set; }
        public EnemyByAmountDictionary EnemiesByAmount { get; set; }
        public Coordinates RoomCoordinates { get; set; }
        public Dimensions RoomDimensions { get; set; }
        [field: SerializeField] public float TimeToFinish { get; private set; }

        private float _startTime;

        public void Init(Coordinates roomCoordinates, Dimensions roomDimensions, EnemyByAmountDictionary enemiesByAmount, float enterTime)
        {
            RoomCoordinates = roomCoordinates;
            EnemiesByAmount = enemiesByAmount;
            RoomDimensions = roomDimensions;
            _startTime = enterTime;
            RoomId = GetRoomIdFromCoordinates(roomCoordinates);
            HasEnemies = enemiesByAmount == null;
            SetNEnemies();
        }

        public int GetRoomIdFromCoordinates(Coordinates coordinates)
        {
            return coordinates.X * dungeonWidth + coordinates.Y;
        }

        public void ExitRoom()
        {
            TimeToFinish = Time.realtimeSinceStartup - _startTime;
        }

        private void SetNEnemies()
        {
            foreach (var enemies in EnemiesByAmount)
            {
                NEnemies += enemies.Value.QuestIds.Count;
            }
        }

#if UNITY_EDITOR
        public void CreateAsset(string assetPath, int revisitIndex)
        {
            var fileName = assetPath + Constants.SeparatorCharacter + "Room"+RoomId+"Data"+revisitIndex+".asset";
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(fileName);
            AssetDatabase.CreateAsset(this, uniquePath);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }
#endif
    }
}