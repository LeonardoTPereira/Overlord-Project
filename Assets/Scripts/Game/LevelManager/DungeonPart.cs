namespace Game.LevelManager
{
    public abstract class DungeonPart
    {
        private string type;
        private Coordinates coordinates;

        public Coordinates Coordinates { get => coordinates; set => coordinates = value; }
        public string Type { get => type; set => type = value; }

        public static class PartType
        {
            public const string START_ROOM = "s";
            public const string FINAL_ROOM = "B";
            public const string TREASURE_ROOM = "T";
            public const string CORRIDOR = "c";
            public const string LOCKED = "L";
        }
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
            return Type?.Equals(PartType.START_ROOM) ?? false;
        }

        public bool IsFinalRoom()
        {
            return Type?.Equals(PartType.FINAL_ROOM) ?? false;
        }
        
        public bool IsTreasureRoom()
        {
            return Type?.Equals(PartType.TREASURE_ROOM) ?? false;
        }

    }
}