using Game.LevelManager;
using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void EnterRoomEvent(object sender, EnterRoomEventArgs e);
public class EnterRoomEventArgs : EventArgs
{
    private Coordinates roomCoordinates;
    private bool roomHasEnemies;
    private List<int> roomEnemiesIndex;
    private int playerHealthWhenEntering;
    private Vector3 roomPosition;
    private Dimensions roomDimensions;

    public EnterRoomEventArgs(Coordinates roomCoordinates, bool roomHasEnemies, List<int> roomEnemiesIndex, int playerHealthWhenEntering, Vector3 roomPosition, Dimensions roomDimensions)
    {
        RoomCoordinates = roomCoordinates;
        RoomHasEnemies = roomHasEnemies;
        RoomEnemiesIndex = roomEnemiesIndex;
        PlayerHealthWhenEntering = playerHealthWhenEntering;
        RoomPosition = roomPosition;
        RoomDimensions = roomDimensions;
    }

    public Coordinates RoomCoordinates { get => roomCoordinates; set => roomCoordinates = value; }
    public bool RoomHasEnemies { get => roomHasEnemies; set => roomHasEnemies = value; }
    public List<int> RoomEnemiesIndex { get => roomEnemiesIndex; set => roomEnemiesIndex = value; }
    public int PlayerHealthWhenEntering { get => playerHealthWhenEntering; set => playerHealthWhenEntering = value; }
    public Vector3 RoomPosition { get => roomPosition; set => roomPosition = value; }
    public Dimensions RoomDimensions { get => roomDimensions; set => roomDimensions = value; }
}
