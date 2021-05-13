using System;
using UnityEngine;

public delegate void StartRoomEventHandler(object sender, StartRoomEventArgs e);
public class StartRoomEventArgs : EventArgs
{
    public Vector3 position;
    public StartRoomEventArgs(Vector3 position)
    {
        this.position = position;
    }
}
