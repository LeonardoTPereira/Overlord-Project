using Util;

namespace Game.LevelManager.DungeonLoader
{
    public abstract class DungeonPart
    {
        public Coordinates Coordinates { get; }

        private string Type { get; }

        protected DungeonPart(Coordinates coordinates, string type)
        {
            Coordinates = coordinates;
            Type = type;
        }
        public Coordinates GetCoordinates()
        {
            return Coordinates;
        }

        public bool IsRoom()
        {
            return Coordinates.X % 2 + Coordinates.Y % 2 == 0;
        }

        public bool IsStartRoom()
        {
            return Type?.Equals(Constants.RoomTypeString.Start) ?? false;
        }

        public bool IsFinalRoom()
        {
            return Type?.Equals(Constants.RoomTypeString.Boss) ?? false;
        }
        
        public bool IsLeafNode()
        {
            return Type?.Equals(Constants.RoomTypeString.Leaf) ?? false;
        }
        
        public bool IsLockedNode()
        {
            return Type?.Equals(Constants.RoomTypeString.LockedRoom) ?? false;
        }

    }
}