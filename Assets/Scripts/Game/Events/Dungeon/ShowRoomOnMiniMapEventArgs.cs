using System;
using UnityEngine;

namespace Game.Events
{
    public delegate void ShowRoomOnMiniMapEvent(object sender, ShowRoomOnMiniMapEventArgs e);

    public class ShowRoomOnMiniMapEventArgs : EventArgs
    {

        public Vector3 RoomPosition { get; set; }

        public ShowRoomOnMiniMapEventArgs(Vector3 position)
        {
            RoomPosition = position;
        }
    }
}