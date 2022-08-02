using ScriptableObjects;
using Util;

namespace Game.Quests
{
    public class QuestExploreRoomEventArgs : QuestElementEventArgs
    {
        public Coordinates RoomCoordinates { get; set; }

        public QuestExploreRoomEventArgs(Coordinates roomCoordinates)
        {
            RoomCoordinates = roomCoordinates;
        }
    }
}