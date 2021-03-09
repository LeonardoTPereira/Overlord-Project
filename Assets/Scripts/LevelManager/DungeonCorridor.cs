public class DungeonCorridor : DungeonPart
{
    public DungeonCorridor(Coordinates coordinates, string code) : base(coordinates, code){    }

    public virtual bool HasLock => false;

}
