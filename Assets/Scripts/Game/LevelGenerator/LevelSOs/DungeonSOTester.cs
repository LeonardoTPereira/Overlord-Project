using System;
using System.Collections.Generic;
using Game.ExperimentControllers;
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
            foreach (var room in dungeon.Parts)
            {
                CurrentColor = Color.HSVToRGB(260/360f, 0, 0f);
                var position = new Vector3(room.Coordinates.X, room.Coordinates.Y, 0);
                if (room.Type == Constants.RoomTypeString.Corridor || room.Type == Constants.RoomTypeString.LockedCorridor)
                {
                    if (room.Type == Constants.RoomTypeString.Corridor)
                    {
                        CurrentColor = Color.HSVToRGB(293/360f, 0, 0.77f);
                    }
                    else
                    {
                        if (room.Locks?.Count <= 0) continue;
                        if (room.Locks != null)
                            CurrentColor = Math.Abs(room.Locks[0]) switch
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
                    newCorridor.GetComponent<DungeonVisualizerRoomData>().roomData = room;
                    _objectsInScene.Add(newCorridor);
                }
                else
                {
                    CurrentColor = Color.HSVToRGB(0.0f, Constants.LogNormalization(room.TotalEnemies
                        , 0, maxEnemies, 0, 1.0f), 1.0f-Constants.LogNormalization(room.TotalEnemies
                        , 0, maxEnemies, 0, 0.5f));
                    var newRoom = Instantiate(RoomPrefab, position, RoomPrefab.transform.rotation);
                    newRoom.GetComponent<SpriteRenderer>().color = CurrentColor;
                    newRoom.GetComponent<DungeonVisualizerRoomData>().roomData = room;
                    _objectsInScene.Add(newRoom);
                    if (room.Type == Constants.RoomTypeString.Start)
                    {
                        CurrentColor = Color.HSVToRGB(300/360f, 0.8f, 0.90f);
                        position = new Vector3(room.Coordinates.X, room.Coordinates.Y, 0);
                        var startEnd = Instantiate(StartEndPrefab, position, StartEndPrefab.transform.rotation);
                        startEnd.GetComponent<SpriteRenderer>().color = CurrentColor;
                        _objectsInScene.Add(startEnd);
                    }
                    else if (room.Type == Constants.RoomTypeString.Boss)
                    {
                        CurrentColor = Color.HSVToRGB(300/360f, 0.5f, 0.70f);
                        position = new Vector3(room.Coordinates.X, room.Coordinates.Y, 0);
                        var startEnd = Instantiate(StartEndPrefab, position, StartEndPrefab.transform.rotation);
                        startEnd.GetComponent<SpriteRenderer>().color = CurrentColor;
                        _objectsInScene.Add(startEnd);
                    }
                    if (room.Type != Constants.RoomTypeString.Key) continue;
                    CurrentColor = Math.Abs(room.Keys[0]) switch
                    {
                        1 => Color.HSVToRGB(81/360f, 0.65f, 0.90f),
                        2 => Color.HSVToRGB(141/360f, 0.85f, 0.90f),
                        3 => Color.HSVToRGB(171/360f, 0.65f, 0.70f),
                        4 => Color.HSVToRGB(201/360f, 0.85f, 0.90f),
                        5 => Color.HSVToRGB(221/360f, 0.65f, 0.90f),
                        6 => Color.HSVToRGB(241/360f, 0.65f, 0.90f),
                        _ => Color.magenta
                    };
                    position = new Vector3(room.Coordinates.X, room.Coordinates.Y, 0);
                    var newKey = Instantiate(KeyPrefab, position, KeyPrefab.transform.rotation);
                    newKey.GetComponent<SpriteRenderer>().color = CurrentColor;
                    _objectsInScene.Add(newKey);
                }
            }
        }
        
        public void DrawDungeonAsGizmos(DungeonFileSo dungeon)
        {
            foreach (var room in dungeon.Parts)
            {
                var size = new Vector3(3, 3, 0);
                // Draw a yellow sphere at the transform's position
                if (room.Type == Constants.RoomTypeString.Corridor)
                {
                    CurrentColor = Color.gray;
                }
                else
                {
                    size = new Vector3(5, 5, 0);
                    CurrentColor = room.Type == Constants.RoomTypeString.Start ? Color.white : Color.magenta;
                }
                var position = new Vector3(room.Coordinates.X * 10, room.Coordinates.Y * 10, 0);
                Gizmos.DrawCube(position, size);
                if (room.Keys?.Count > 0)
                {
                    CurrentColor = room.Keys[0] switch
                    {
                        1 => Color.yellow,
                        2 => Color.blue,
                        3 => Color.green,
                        4 => Color.red,
                        5 => new Color(255, 165, 0),
                        6 => new Color(230, 230, 250),
                        _ => Color.cyan
                    };
                    position = new Vector3(room.Coordinates.X * 10, room.Coordinates.Y * 10, 5);
                    Gizmos.DrawSphere(position, 2);
                }

                if (room.Locks?.Count <= 0) continue;
                if (room.Locks != null)
                    CurrentColor = Math.Abs(room.Locks[0]) switch
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
