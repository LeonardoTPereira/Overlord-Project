using System;
using Util;

namespace Game.Events
{
    public delegate void MarkRoomOnMiniMapEvent(object sender, MarkRoomOnMinimapEventArgs e);

    public class MarkRoomOnMinimapEventArgs : EventArgs
    {
        public Coordinates RoomCoordinates { get; set; }

        public MarkRoomOnMinimapEventArgs(string text)
        {
            var x = int.Parse(text.Split(',')[0]);
            var y = int.Parse(text.Split(',')[1]);
            RoomCoordinates = new Coordinates(x, y);
        }
    }
}