using Game.LevelManager;
using System;
using System.Collections.Generic;

public delegate void ExitRoomEvent(object sender, ExitRoomEventArgs e);
public class ExitRoomEventArgs : EventArgs
{
    private Coordinates roomCoordinates;
    private int playerHealthWhenExiting;

    public ExitRoomEventArgs(Coordinates roomCoordinates, int playerHealthWhenExiting)
    {
        RoomCoordinates = roomCoordinates;
        PlayerHealthWhenExiting = playerHealthWhenExiting;
    }

    public Coordinates RoomCoordinates { get => roomCoordinates; set => roomCoordinates = value; }
    public int PlayerHealthWhenExiting { get => playerHealthWhenExiting; set => playerHealthWhenExiting = value; }
}
