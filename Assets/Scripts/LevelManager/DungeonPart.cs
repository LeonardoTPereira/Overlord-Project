using System.Collections.Generic;

public abstract class DungeonPart
{
    public string type;
    public Coordinates coordinates;

    public static class Type
    {
        public const string START_ROOM = "s";
        public const string FINAL_ROOM = "B";
        public const string TREASURE_ROOM = "T";
        public const string CORRIDOR = "c";
        public const string LOCKED = "L";
    }
    public DungeonPart(Coordinates coordinates, string type)
    {
        this.coordinates = coordinates;
        this.type = type;
    }
    public Coordinates GetCoordinates()
    {
        return coordinates;
    }

    public bool IsRoom()
    {
        return ((coordinates.X % 2) + (coordinates.Y % 2)) == 0;
    }

    public bool IsStartRoom()
    {
        return type.Equals(Type.START_ROOM);
    }

    public bool IsFinalRoom()
    {
        return type.Equals(Type.FINAL_ROOM);
    }

}
