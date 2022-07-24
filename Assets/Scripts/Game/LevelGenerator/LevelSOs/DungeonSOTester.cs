using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Game.LevelGenerator.LevelSOs
{
    public class DungeonSOTester : MonoBehaviour
    {
        [field:SerializeField] private DungeonFileSo CurrentDungeonSo { get; set; }
        [field:SerializeField] private GameObject RoomPrefab { get; set; }
        [field:SerializeField] private GameObject KeyPrefab { get; set; }
        [field:SerializeField] private GameObject CorridorPrefab { get; set; }
        [field:SerializeField] private GameObject StartEndPrefab { get; set; }
        [field:SerializeField] private Color CurrentColor { get; set; }

        private List<GameObject> _objectsInScene;

        private void Awake()
        {
            _objectsInScene = new List<GameObject>();
        }

        private const float Tolerance = 0.01f;

        public void OnDrawGizmos()
        {
            if (CurrentDungeonSo == null) return;
            DrawDungeonAsGizmos(CurrentDungeonSo);
        }

        public void DrawDungeonSprites(DungeonFileSo dungeon, int maxEnemies, Vector3 center)
        {
            foreach (var objectInScene in _objectsInScene)
            {
                Destroy(objectInScene);
            }
            _objectsInScene.Clear();
            Camera.main.transform.position = center;
            foreach (var room in dungeon.Rooms)
            {
                var position = new Vector3(room.coordinates.X, room.coordinates.Y, 0);
                if (room.type == Constants.RoomTypeString.CORRIDOR || room.type == Constants.RoomTypeString.LOCK)
                {
                    if (room.type == Constants.RoomTypeString.CORRIDOR)
                    {
                        CurrentColor = Color.HSVToRGB(293/360f, 0, 0.77f);
                    }
                    else
                    {
                        if (room.locks?.Count <= 0) continue;
                        if (room.locks != null)
                            CurrentColor = Math.Abs(room.locks[0]) switch
                            {
                                1 => Color.HSVToRGB(81/360f, 0.65f, 0.90f),
                                2 => Color.HSVToRGB(141/360f, 0.85f, 0.90f),
                                3 => Color.HSVToRGB(171/360f, 0.65f, 0.70f),
                                4 => Color.HSVToRGB(201/360f, 0.85f, 0.90f),
                                5 => Color.HSVToRGB(221/360f, 0.65f, 0.90f),
                                6 => Color.HSVToRGB(241/360f, 0.65f, 0.90f),
                                _ => Color.magenta
                            };
                    }

                    var rotation = CorridorPrefab.transform.rotation;
                    if (Math.Abs(position.y % 2 - 1) < Tolerance)
                    {
                        rotation *= Quaternion.Euler(0, 0, 90);
                    }
                    var newCorridor = Instantiate(CorridorPrefab, position, rotation);
                    newCorridor.GetComponent<SpriteRenderer>().color = CurrentColor;
                    _objectsInScene.Add(newCorridor);
                }
                else
                {
                    CurrentColor = Color.HSVToRGB(0.0f, Constants.LogNormalization(room.TotalEnemies
                        , 0, maxEnemies, 0, 1.0f), 1.0f-Constants.LogNormalization(room.TotalEnemies
                        , 0, maxEnemies, 0, 0.5f));
                    var newRoom = Instantiate(RoomPrefab, position, RoomPrefab.transform.rotation);
                    newRoom.GetComponent<SpriteRenderer>().color = CurrentColor;
                    _objectsInScene.Add(newRoom);
                    if (room.type == Constants.RoomTypeString.START)
                    {
                        CurrentColor = Color.HSVToRGB(300/360f, 0.8f, 0.90f);
                        position = new Vector3(room.coordinates.X, room.coordinates.Y, 0);
                        var startEnd = Instantiate(StartEndPrefab, position, StartEndPrefab.transform.rotation);
                        startEnd.GetComponent<SpriteRenderer>().color = CurrentColor;
                        _objectsInScene.Add(startEnd);
                    }
                    else if (room.type == Constants.RoomTypeString.BOSS)
                    {
                        CurrentColor = Color.HSVToRGB(300/360f, 0.5f, 0.70f);
                        position = new Vector3(room.coordinates.X, room.coordinates.Y, 0);
                        var startEnd = Instantiate(StartEndPrefab, position, StartEndPrefab.transform.rotation);
                        startEnd.GetComponent<SpriteRenderer>().color = CurrentColor;
                        _objectsInScene.Add(startEnd);
                    }
                    if (room.type != Constants.RoomTypeString.KEY) continue;
                    CurrentColor = Math.Abs(room.keys[0]) switch
                    {
                        1 => Color.HSVToRGB(81/360f, 0.65f, 0.90f),
                        2 => Color.HSVToRGB(141/360f, 0.85f, 0.90f),
                        3 => Color.HSVToRGB(171/360f, 0.65f, 0.70f),
                        4 => Color.HSVToRGB(201/360f, 0.85f, 0.90f),
                        5 => Color.HSVToRGB(221/360f, 0.65f, 0.90f),
                        6 => Color.HSVToRGB(241/360f, 0.65f, 0.90f),
                        _ => Color.magenta
                    };
                    position = new Vector3(room.coordinates.X, room.coordinates.Y, 0);
                    var newKey = Instantiate(KeyPrefab, position, KeyPrefab.transform.rotation);
                    newKey.GetComponent<SpriteRenderer>().color = CurrentColor;
                    _objectsInScene.Add(newKey);
                }
            }
        }
        
        public void DrawDungeonAsGizmos(DungeonFileSo dungeon)
        {
            foreach (var room in dungeon.Rooms)
            {
                var size = new Vector3(3, 3, 0);
                // Draw a yellow sphere at the transform's position
                if (room.type == Constants.RoomTypeString.CORRIDOR)
                {
                    CurrentColor = Color.gray;
                }
                else
                {
                    size = new Vector3(5, 5, 0);
                    CurrentColor = room.type == Constants.RoomTypeString.START ? Color.white : Color.magenta;
                }
                var position = new Vector3(room.coordinates.X * 10, room.coordinates.Y * 10, 0);
                Gizmos.DrawCube(position, size);
                if (room.keys?.Count > 0)
                {
                    CurrentColor = room.keys[0] switch
                    {
                        1 => Color.yellow,
                        2 => Color.blue,
                        3 => Color.green,
                        4 => Color.red,
                        5 => new Color(255, 165, 0),
                        6 => new Color(230, 230, 250),
                        _ => Color.cyan
                    };
                    position = new Vector3(room.coordinates.X * 10, room.coordinates.Y * 10, 5);
                    Gizmos.DrawSphere(position, 2);
                }

                if (room.locks?.Count <= 0) continue;
                if (room.locks != null)
                    CurrentColor = Math.Abs(room.locks[0]) switch
                    {
                        1 => Color.yellow,
                        2 => Color.blue,
                        3 => Color.green,
                        4 => Color.red,
                        5 => new Color(255, 165, 0),
                        6 => new Color(230, 230, 250),
                        _ => Color.cyan
                    };
                Gizmos.DrawCube(position, size);
            }
        }
    }    
}
