using Util;

namespace Game.LevelManager.DungeonLoader
{
    public abstract class DungeonPart
    {
        private string type;
        private Coordinates coordinates;

        public Coordinates Coordinates { get => coordinates; set => coordinates = value; }
        public string Type { get => type; set => type = value; }
        
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
        
        public bool IsTreasureRoom()
        {
            return Type?.Equals(Constants.RoomTypeString.Treasure) ?? false;
        }

    }
}