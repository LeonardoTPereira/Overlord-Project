using System.Collections.Generic;
using UnityEngine;

public class DungeonLockedCorridor : DungeonCorridor
{

    public List<int> lockIDs;
    public DungeonLockedCorridor(Coordinates coordinates, List<int> lockIDs) : base(coordinates, Type.LOCKED)
    {
        this.lockIDs = lockIDs;
    }
}
