using System.Collections.Generic;
using Util;

namespace Game.LevelManager.DungeonLoader
{
    public class DungeonLockedCorridor : DungeonCorridor
    {

        private List<int> lockIDs;
        public DungeonLockedCorridor(Coordinates coordinates, List<int> lockIDs) : base(coordinates, Constants.RoomTypeString.LockedCorridor)
        {
            LockIDs = lockIDs;
        }

        public List<int> LockIDs { get => lockIDs; set => lockIDs = value; }
    }
}