using System;
using UnityEngine;
using Util;

namespace Game.Events
{
    public delegate void ExitRoomEvent(object sender, ExitRoomEventArgs e);
    public class ExitRoomEventArgs : EventArgs
    {
        private Coordinates roomCoordinates;
        private Vector3 entrancePosition;
        private int playerHealthWhenExiting;

        public ExitRoomEventArgs(Coordinates roomCoordinates, int playerHealthWhenExiting, Vector3 entrancePosition)
        {
            RoomCoordinates = roomCoordinates;
            PlayerHealthWhenExiting = playerHealthWhenExiting;
            EntrancePosition = entrancePosition;
        }

        public Coordinates RoomCoordinates { get => roomCoordinates; set => roomCoordinates = value; }
        public int PlayerHealthWhenExiting { get => playerHealthWhenExiting; set => playerHealthWhenExiting = value; }
        public Vector3 EntrancePosition { get => entrancePosition; set => entrancePosition = value; }
    }
}