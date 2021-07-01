using Game.LevelManager;
using System;
using System.Collections.Generic;

public delegate void EnterRoomEvent(object sender, EnterRoomEventArgs e);
public class EnterRoomEventArgs : EventArgs
{
    private Coordinates roomCoordinates;
    private bool roomHasEnemies;
    private List<int> roomEnemiesIndex;
    private int playerHealthWhenEntering;

    public EnterRoomEventArgs(Coordinates roomCoordinates, bool roomHasEnemies, List<int> roomEnemiesIndex, int playerHealthWhenEntering)
    {
        RoomCoordinates = roomCoordinates;
        RoomHasEnemies = roomHasEnemies;
        RoomEnemiesIndex = roomEnemiesIndex;
        PlayerHealthWhenEntering = playerHealthWhenEntering;
    }

    public Coordinates RoomCoordinates { get => roomCoordinates; set => roomCoordinates = value; }
    public bool RoomHasEnemies { get => roomHasEnemies; set => roomHasEnemies = value; }
    public List<int> RoomEnemiesIndex { get => roomEnemiesIndex; set => roomEnemiesIndex = value; }
    public int PlayerHealthWhenEntering { get => playerHealthWhenEntering; set => playerHealthWhenEntering = value; }
}
