using System;
using UnityEngine;

namespace Game.Events
{
    public delegate void StartRoomEvent(object sender, StartRoomEventArgs e);
    public class StartRoomEventArgs : EventArgs
    {
        public Vector3 position;
        public StartRoomEventArgs(Vector3 position)
        {
            this.position = position;
        }
    }
}