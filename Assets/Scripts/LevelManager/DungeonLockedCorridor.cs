using System.Collections.Generic;

public class DungeonLockedCorridor : DungeonCorridor
{

    public List<int> lockIDs;
    public DungeonLockedCorridor(Coordinates coordinates, List<int> lockIDs) : base(coordinates, Type.LOCKED)
    {
        this.lockIDs = lockIDs;
    }
    public override bool HasLock => true;
}
