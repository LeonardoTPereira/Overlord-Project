using System.Collections.Generic;
using Game.NPCs;

namespace Game.LevelManager.DungeonLoader
{
    public static class NpcDispenser
    {
        public static void DistributeNpcsInDungeon(Map map, List<NpcSo> availableNpcs)
        {
            var npcQueue = new Queue<NpcSo>(availableNpcs);
            DistributeNpcsInPriorityRooms(map, npcQueue);
            if (npcQueue.Count <= 0) return;
            DistributeNpcsInAnyRoom(map, npcQueue);
        }

        private static void DistributeNpcsInAnyRoom(Map map, Queue<NpcSo> npcQueue)
        {
            foreach (var dungeonPart in map.DungeonPartByCoordinates)
            {
                if (dungeonPart.Value is not DungeonRoom dungeonRoom || dungeonRoom.IsStartRoom()) continue;
                dungeonRoom.Npcs = new List<NpcSo> {npcQueue.Dequeue()};
                if (npcQueue.Count > 0) continue;
                break;
            }
        }

        private static void DistributeNpcsInPriorityRooms(Map map, Queue<NpcSo> npcQueue)
        {
            foreach (var dungeonPart in map.DungeonPartByCoordinates)
            {
                if (dungeonPart.Value is not DungeonRoom dungeonRoom || dungeonRoom.IsStartRoom() ||
                    !dungeonRoom.HasItemPreference) continue;
                dungeonRoom.Npcs = new List<NpcSo> {npcQueue.Dequeue()};
                if (npcQueue.Count > 0) continue;
                break;
            }
        }
    }
}