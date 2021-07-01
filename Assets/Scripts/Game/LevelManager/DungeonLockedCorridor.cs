using System.Collections.Generic;

namespace Game.LevelManager
{
    public class DungeonLockedCorridor : DungeonCorridor
    {

        private List<int> lockIDs;
        public DungeonLockedCorridor(Coordinates coordinates, List<int> lockIDs) : base(coordinates, PartType.LOCKED)
        {
            LockIDs = lockIDs;
        }

        public List<int> LockIDs { get => lockIDs; set => lockIDs = value; }
    }
}